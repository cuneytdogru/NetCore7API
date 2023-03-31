using System.Net;
using System.Text.Json.Serialization;

namespace NetCore7API.Models
{
    public class ApiResponse<T> : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Result { get; set; } = default;

        public static ApiResponse<T> Create(int statusCode, string? message, T result)
        {
            return new ApiResponse<T> { StatusCode = statusCode, Message = message, ErrorMessage = null, Identifier = null, Result = result };
        }
    }

    public class ApiResponse
    {
        public static ApiResponse Create(int statusCode, string? message, string? errorMessage, string? identifier)
        {
            return new ApiResponse { StatusCode = statusCode, Message = message, ErrorMessage = errorMessage, Identifier = identifier };
        }

        public static ApiResponse Create(int statusCode, string message, string errorMessage)
        {
            return Create(statusCode, message, errorMessage, null);
        }

        public static ApiResponse Create(int statusCode, string message)
        {
            return Create(statusCode, message, null, null);
        }

        public int StatusCode { get; set; }

        public bool IsError
        {
            get { return ((int)StatusCode < 200) || ((int)StatusCode > 299); }
        }

        public string? Message { get; set; } = null;

        public string? ErrorMessage { get; set; } = null;

        public string? Identifier { get; set; } = null;

        public static string GetStatusCodeMessage(HttpStatusCode statusCode)
        {
            return GetStatusCodeMessage((int)statusCode);
        }

        public static string GetStatusCodeMessage(int statusCode)
        {
            if (statusCode >= 200 && statusCode < 300)
                return "Request successful.";

            switch (statusCode)
            {
                case 400:
                    return "Bad request.";

                case 401:
                    return "Unauthorized access.";

                case 402:
                    return "Payment required.";

                case 403:
                    return "Forbidden access.";

                case 404:
                    return "Resource not found.";

                case 405:
                    return "Method not allowed.";

                case 406:
                    return "Not acceptable.";

                case 407:
                    return "Proxy authentication required.";

                case 408:
                    return "Request timeout.";

                case 409:
                    return "Conflict";

                case 410:
                    return "Resource is gone.";

                case 411:
                    return "Length is required.";

                case 500:
                    return "Internal server error.";

                case 501:
                    return "Not implemented.";

                case 502:
                    return "Bad gateway.";

                case 503:
                    return "Service unavailable.";

                case 504:
                    return "Gateway timeout.";

                case 505:
                    return "HTTP version not supported.";
            }
            return "";
        }
    }
}