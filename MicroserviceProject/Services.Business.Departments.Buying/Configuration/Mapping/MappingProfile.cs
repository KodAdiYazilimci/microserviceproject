using AutoMapper;

using MicroserviceProject.Infrastructure.Communication.Model.Department.Buying;
using MicroserviceProject.Infrastructure.Transaction.Recovery;
using MicroserviceProject.Services.Business.Departments.Buying.Entities.Sql;

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
