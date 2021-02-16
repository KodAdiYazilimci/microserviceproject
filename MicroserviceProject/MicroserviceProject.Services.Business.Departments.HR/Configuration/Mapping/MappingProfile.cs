﻿using AutoMapper;

using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Model.Department.HR;

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

            // Entity => Model

            CreateMap<DepartmentEntity, DepartmentModel>();
            CreateMap<PersonEntity, PersonModel>();
            CreateMap<TitleEntity, TitleModel>();
            CreateMap<WorkerEntity, WorkerModel>();
        }
    }
}
