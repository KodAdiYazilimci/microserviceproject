using AutoMapper;

using Infrastructure.Transaction.Recovery;

using Services.Business.Departments.CR.Entities.EntityFramework;
using Services.Business.Departments.CR.Models;

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
