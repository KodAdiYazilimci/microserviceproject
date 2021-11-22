using AutoMapper;

using Services.Communication.Http.Broker.Gateway.Public.Models;

namespace Presentation.UI.Web.Configuration.Mapping
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
            // Gateway Model => Local Model

            CreateMap<DepartmentModel, Models.HR.DepartmentModel>();
        }
    }
}
