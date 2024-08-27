WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(configuration));

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddSwaggerGenWithAuth()
    .AddInfrastructure(configuration)
    .AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapEndpoints();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerWithUi();

    using IServiceScope scope = app.Services.CreateScope();

    using ApplicationDbContext dbContext =
        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
}

app.UseApplicationServices();

await app.RunAsync();

public partial class Program;
