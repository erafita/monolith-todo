namespace Todo.Web.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApplicationServices(this WebApplication app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        app.UseExceptionHandler()
           .UseAuthentication()
           .UseAuthorization();

        return app;
    }

    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
