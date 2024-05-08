using NetCore7API.Models;
using System.Net;
using System.Text.Json;

namespace NetCore7API.Middleware
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsSwagger(context))
                await this._next(context);
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    context.Response.ContentType = "application/json";

                    await _next(context);

                    //Get original response to wrap
                    var originalBodyObject = await GetOriginalBody(context);

                    //Clear original response from stream
                    responseBody.SetLength(0);

                    if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                    {
                        if (context.Response.StatusCode != 204)
                            await HandleSuccessRequestAsync(context, originalBodyObject);
                    }
                    else
                    {
                        await HandleNotSuccessRequestAsync(context, context.Response.StatusCode, originalBodyObject);
                    }

                    //Copy wrapped response stream to original response stream
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }

        private async Task<object> GetOriginalBody(HttpContext context)
        {
            var originalBodyString = await FormatResponse(context.Response);

            object originalBodyObject;

            if (IsValidJson(originalBodyString))
                originalBodyObject = JsonSerializer.Deserialize<object>(originalBodyString);
            else
            {
                originalBodyObject = string.IsNullOrWhiteSpace(originalBodyString) ? null : originalBodyString;
            }

            return originalBodyObject;
        }

        private async Task<ApiResponse<object>> CreateResponseFromBody(int statusCode, object originalBodyObject)
        {
            return ApiResponse<object>.Create(
                statusCode,
                ApiResponse.GetStatusCodeMessage(statusCode),
                originalBodyObject);
        }

        private async Task HandleSuccessRequestAsync(HttpContext context, object originalBodyObject)
        {
            var result = await CreateResponseFromBody(context.Response.StatusCode, originalBodyObject);

            await context.Response.WriteAsync(JsonSerializer.Serialize(result, DefaultJsonOptions.Web));
        }

        private async Task HandleNotSuccessRequestAsync(HttpContext context, int code, object originalBodyObject)
        {
            ApiResponse apiResponse;
            string errorMessage = string.Empty;
            string identifier = string.Empty;

            if (context.Items["exception"] != null)
            {
                errorMessage = context.Items["exceptionMessage"].ToString();
                identifier = context.Items["identifier"].ToString();
            }

            if (code == (int)HttpStatusCode.NotFound)
                apiResponse = ApiResponse.Create(code, ApiResponse.GetStatusCodeMessage(code), "The specified URI does not exist. Please verify and try again.");
            else if (code == (int)HttpStatusCode.BadRequest)
            {
                apiResponse = await CreateResponseFromBody(code, originalBodyObject);
            }
            else if (code == (int)HttpStatusCode.Unauthorized)
            {
                apiResponse = ApiResponse.Create(code, ApiResponse.GetStatusCodeMessage(code), "You are not logged in.");
            }
            else if (code == (int)HttpStatusCode.Forbidden)
            {
                apiResponse = ApiResponse.Create(code, ApiResponse.GetStatusCodeMessage(code), "You are not allowed to call this request.");
            }
            else
                apiResponse = ApiResponse.Create(code, ApiResponse.GetStatusCodeMessage(code), "Your request cannot be processed. Please contact a support.");

            if (!string.IsNullOrWhiteSpace(errorMessage))
                apiResponse.ErrorMessage = errorMessage;

            if (!string.IsNullOrWhiteSpace(identifier))
                apiResponse.Identifier = identifier;

            await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse, DefaultJsonOptions.Web));
        }

        private bool IsValidJson(string s)
        {
            var response = false;

            try
            {
                JsonSerializer.Deserialize<object>(s);
                response = true;
            }
            catch (JsonException)
            {
                response = false;
            }

            return response;
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        private static bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }
    }
}