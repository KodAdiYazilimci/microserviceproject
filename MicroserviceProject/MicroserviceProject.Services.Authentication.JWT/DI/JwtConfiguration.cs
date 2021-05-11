using MicroserviceProject.Infrastructure.Security.Authentication.JWT.Providers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MicroserviceProject.Services.Authentication.JWT.DI
{
    public static class JwtConfiguration
    {
        public static IServiceCollection RegisterJWT(this IServiceCollection services)
        {
            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(services.BuildServiceProvider().GetRequiredService<SecretProvider>().Bytes),
                    ValidIssuer = services.BuildServiceProvider().GetRequiredService<IssuerProvider>().Issuer,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = services.BuildServiceProvider().GetRequiredService<IssuerProvider>().Audience,
                    ValidateAudience = true
                };
            });

            return services;
        }
    }
}
