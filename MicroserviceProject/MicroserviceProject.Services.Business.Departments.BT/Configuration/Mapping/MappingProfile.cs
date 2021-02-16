using AutoMapper;

using MicroserviceProject.Services.Business.Departments.IT.Entities.Sql;
using MicroserviceProject.Services.Model.Department.IT;

namespace MicroserviceProject.Services.Business.Departments.IT.Configuration.Mapping
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

            CreateMap<InventoryModel, InventoryEntity>();

            // Entity => Model

            CreateMap<InventoryEntity, InventoryModel>();
        }
    }
}
