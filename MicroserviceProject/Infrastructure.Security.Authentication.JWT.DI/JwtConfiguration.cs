using Infrastructure.Security.Authentication.JWT.Providers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security.Authentication.JWT.DI
{
    /// <summary>
    /// JWT DI sınıfı
    /// </summary>
    public static class JwtConfiguration
    {
        /// <summary>
        /// JWT yi enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
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
