using AutoMapper;

using Services.Communication.Http.Broker.Department.IT.Models;

using Services.Api.Business.Departments.IT.Entities.Sql;

namespace Services.Api.Business.Departments.IT.Configuration.Mapping
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
