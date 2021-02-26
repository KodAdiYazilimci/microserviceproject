using AutoMapper;

using MicroserviceProject.Services.Business.Departments.Buying.Entities.Sql;
using MicroserviceProject.Services.Model.Department.Buying;
using MicroserviceProject.Services.Transaction.Models;

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

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model

            CreateMap<InventoryRequestEntity, InventoryRequestModel>();
            
            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}
