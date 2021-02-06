using AutoMapper;

using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Model.Department.HR;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Mapping
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
            // Model => Entity

            CreateMap<DepartmentModel, DepartmentEntity>();

            // Entity => Model

            CreateMap<DepartmentEntity, DepartmentModel>();
        }
    }
}
