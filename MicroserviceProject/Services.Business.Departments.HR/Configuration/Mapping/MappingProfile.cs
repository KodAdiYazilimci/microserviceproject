using AutoMapper;

using Infrastructure.Transaction.Recovery;

using Services.Business.Departments.HR.Entities.Sql;
using Services.Business.Departments.HR.Models;

namespace Services.Business.Departments.HR.Configuration.Mapping
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

            CreateMap<DepartmentModel, DepartmentEntity>();
            CreateMap<PersonModel, PersonEntity>();
            CreateMap<TitleModel, TitleEntity>();
            CreateMap<WorkerModel, WorkerEntity>();

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model

            CreateMap<DepartmentEntity, DepartmentModel>();
            CreateMap<PersonEntity, PersonModel>();
            CreateMap<TitleEntity, TitleModel>();
            CreateMap<WorkerEntity, WorkerModel>();

            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}
