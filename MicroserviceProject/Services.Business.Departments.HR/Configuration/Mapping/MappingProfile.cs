using AutoMapper;

using MicroserviceProject.Infrastructure.Communication.Model.Department.HR;
using MicroserviceProject.Infrastructure.Transaction.Recovery;
using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Mapping
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
