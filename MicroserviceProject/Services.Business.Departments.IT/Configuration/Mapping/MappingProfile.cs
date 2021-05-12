using AutoMapper;

using MicroserviceProject.Infrastructure.Communication.Model.Department.IT;
using MicroserviceProject.Services.Business.Departments.IT.Entities.Sql;

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
