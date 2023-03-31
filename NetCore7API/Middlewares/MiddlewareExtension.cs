using NetCore7API.Middleware;

namespace NetCore7API.Middlewares
{
    public static class MiddlewareExtension
    {
        public static void ConfigureCustomResponseWrapperMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResponseWrapperMiddleware>();
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}