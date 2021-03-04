using AutoMapper;

using MicroserviceProject.Services.Business.Departments.Finance.Entities.Sql;
using MicroserviceProject.Services.Model.Department.Finance;
using MicroserviceProject.Services.Transaction.Models;

namespace MicroserviceProject.Services.Business.Departments.Finance.Configuration.Mapping
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
