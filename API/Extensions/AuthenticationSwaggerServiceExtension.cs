using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class AuthenticationSwaggerServiceExtension
    {
        public static IServiceCollection AddAuthenticationSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with Jwt",
                    Type = SecuritySchemeType.Http
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }

                });
            });
            return services;
        }
    }
}