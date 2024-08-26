WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddSwaggerGenWithAuth()
    .AddInfrastructure(builder.Configuration)
    .AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
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
