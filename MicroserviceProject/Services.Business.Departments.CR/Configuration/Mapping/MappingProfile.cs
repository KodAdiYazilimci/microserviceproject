using AutoMapper;

using Services.Communication.Http.Broker.Department.CR.Models;

using Infrastructure.Transaction.Recovery;

using Services.Business.Departments.CR.Entities.EntityFramework;

namespace Services.Business.Departments.CR.Configuration.Mapping
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

            CreateMap<CustomerModel, CustomerEntity>();

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model

            CreateMap<CustomerEntity, CustomerModel>();

            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}
