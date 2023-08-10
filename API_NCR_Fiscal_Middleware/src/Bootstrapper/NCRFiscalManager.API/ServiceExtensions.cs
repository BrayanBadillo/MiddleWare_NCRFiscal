using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using NCRFiscalManager.API.Middleware;

namespace NCRFiscalManager.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSwaggerServices();
            services.AddAuthenticationService();
            return services;
        }
        
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiddleWare Web API", Version = "v1" });
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Description = "Basic auth added to authorization header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Basic",
                    Type = SecuritySchemeType.Http
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        private static IServiceCollection AddAuthenticationService(this IServiceCollection services)
        {
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

            return services;
        }
    }
}
