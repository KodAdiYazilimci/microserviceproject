using AutoMapper;

using Infrastructure.Transaction.Recovery;

using Services.Api.Business.Departments.CR.Entities.EntityFramework;
using Services.Communication.Http.Broker.Department.CR.Models;

namespace Services.Api.Business.Departments.CR.Configuration.Mapping
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
