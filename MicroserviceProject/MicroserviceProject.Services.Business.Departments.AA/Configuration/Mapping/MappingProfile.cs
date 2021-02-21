using AutoMapper;

using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
using MicroserviceProject.Services.Model.Department.AA;
using MicroserviceProject.Services.Transaction.Models;

namespace MicroserviceProject.Services.Business.Departments.AA.Configuration.Mapping
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

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model

            CreateMap<InventoryEntity, InventoryModel>();

            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}
