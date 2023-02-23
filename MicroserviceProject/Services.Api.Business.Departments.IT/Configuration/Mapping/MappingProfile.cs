using AutoMapper;

using Services.Api.Business.Departments.IT.Entities.Sql;
using Services.Communication.Http.Broker.Department.IT.Models;

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

            CreateMap<ITInventoryModel, InventoryEntity>();

            // Entity => Model

            CreateMap<InventoryEntity, ITInventoryModel>();
        }
    }
}
