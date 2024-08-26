namespace Todo.Web.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Name = "TODO APP JWT Authentication",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "TODO APP JWT Authentication Description"
            };

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },

                    []
                }
            };

            options.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}
