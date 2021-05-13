using AutoMapper;

using Infrastructure.Communication.Model.Department.IT;
using Services.Business.Departments.IT.Entities.Sql;

namespace Services.Business.Departments.IT.Configuration.Mapping
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
