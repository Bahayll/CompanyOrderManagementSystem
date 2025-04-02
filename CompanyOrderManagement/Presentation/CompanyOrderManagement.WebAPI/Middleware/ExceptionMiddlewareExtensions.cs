using CompanyOrderManagement.WebAPI.Middleware;

namespace WebAPI.Middleware;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
