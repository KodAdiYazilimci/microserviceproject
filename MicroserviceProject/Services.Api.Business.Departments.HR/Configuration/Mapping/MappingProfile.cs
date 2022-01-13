using AutoMapper;

using Infrastructure.Transaction.Recovery;

using Services.Api.Business.Departments.HR.Entities.Sql;
using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Api.Business.Departments.HR.Configuration.Mapping
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
