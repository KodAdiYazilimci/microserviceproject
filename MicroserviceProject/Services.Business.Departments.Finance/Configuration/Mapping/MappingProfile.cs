using AutoMapper;

using Infrastructure.Transaction.Recovery;
using Services.Business.Departments.Finance.Entities.Sql;
using Services.Model.Department.Finance;

namespace Services.Business.Departments.Finance.Configuration.Mapping
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

            CreateMap<DecidedCostModel, DecidedCostEntity>();

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model

            CreateMap<DecidedCostEntity, DecidedCostModel>();

            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}
