using AutoMapper;

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

            CreateMap<Communication.Http.Department.HR.Models.DepartmentModel, Models.HR.DepartmentModel>();
        }
    }
}
