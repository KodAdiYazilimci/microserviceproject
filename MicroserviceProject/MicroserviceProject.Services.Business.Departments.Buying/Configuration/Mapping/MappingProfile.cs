using AutoMapper;

using MicroserviceProject.Services.Business.Departments.Buying.Entities.Sql;
using MicroserviceProject.Services.Model.Department.Buying;

namespace MicroserviceProject.Services.Business.Departments.Buying.Configuration.Mapping
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

            CreateMap<InventoryRequestModel, InventoryRequestEntity>();

            // Entity => Model

            CreateMap<InventoryRequestEntity, InventoryRequestModel>();
        }
    }
}
