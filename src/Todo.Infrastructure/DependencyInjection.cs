namespace Todo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(
                connectionString, npgsqlOptions =>
                    npgsqlOptions
                        //.CommandTimeout(5)
                        //.EnableRetryOnFailure(settings.MaxRetryCount, TimeSpan.FromSeconds(5), null);
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, ApplicationDbContext.Schema)
                );

            options.AddInterceptors(sp.GetRequiredService<AuditableEntitiesInterceptor>());

            //options.EnableDetailedErrors(settings.EnableDetailedErrors)
            //       .EnableSensitiveDataLogging(settings.EnableSensitiveDataLogging);
        });

        services.AddSingleton<AuditableEntitiesInterceptor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoItemRepository, TodoItemRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.MapInboundClaims = false;
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
            });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddScoped<PermissionProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
