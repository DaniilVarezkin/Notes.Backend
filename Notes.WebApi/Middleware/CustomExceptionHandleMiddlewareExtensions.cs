namespace Notes.WebApi.Middleware
{
    public static class CustomExceptionHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandleMiddleware>();
        }
    }
}
