using NetCore7API.Domain.Exceptions;
using System.Net;

namespace NetCore7API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error occured while processing request: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorMessage = string.Empty;
            var identifier = string.Empty;

            switch (exception)
            {
                case UserException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorMessage = exception.Message;
                    break;

                default:
                    identifier = Guid.NewGuid().ToString();
                    errorMessage = "An internal error occured while processing your request.";

                    _logger.LogError(exception, "An internal error occured while processing your request. Use {Identifier} to inspect exception details.", identifier);

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    break;
            }

            context.Items.Add("exception", exception);
            context.Items.Add("exceptionMessage", errorMessage);
            context.Items.Add("identifier", identifier);
        }
    }
}