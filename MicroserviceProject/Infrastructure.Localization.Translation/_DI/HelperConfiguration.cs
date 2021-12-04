using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Localization.Translation.DI
{
    public static class HelperConfiguration
    {
        public static IServiceCollection RegisterTranslationHelpers(this IServiceCollection services)
        {
            services.AddSingleton<TranslationHelper>();

            return services;
        }
    }
}
