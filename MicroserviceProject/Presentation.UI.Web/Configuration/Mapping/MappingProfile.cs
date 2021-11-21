using AutoMapper;

using Communication.Http.Gateway.Public.Models;

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
