using AutoMapper;

namespace Services.Api.Localization.Configuration.Mapping
{
    /// <summary>
    /// Mapping profili sınıfı
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Mapping profili sınıfı
        /// </summary>
        public MappingProfile()
        {
            // Http Model => Localization Model

            CreateMap<Communication.Http.Broker.Localization.Models.TranslationModel, Infrastructure.Localization.Translation.Models.TranslationModel>();

            // Localization Model => Http Model

            CreateMap<Infrastructure.Localization.Translation.Models.TranslationModel, Communication.Http.Broker.Localization.Models.TranslationModel>();
        }
    }
}
