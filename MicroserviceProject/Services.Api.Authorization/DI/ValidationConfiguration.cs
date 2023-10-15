using Microsoft.Extensions.DependencyInjection;

using Services.Api.Authorization.Configuration.Validation.Auth.GetToken;
using Services.Api.Authorization.Util.Validation.Auth.GetToken;

namespace Services.Api.Authorization.DI
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services.AddSingleton<CredentialRule>();
            services.AddSingleton<GetTokenValidator>();

            return services;
        }
    }
}
