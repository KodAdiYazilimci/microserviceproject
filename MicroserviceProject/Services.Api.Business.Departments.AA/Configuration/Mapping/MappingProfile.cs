﻿using AutoMapper;

using Services.Communication.Http.Broker.Department.AA.Models;

using Infrastructure.Transaction.Recovery;

using Services.Api.Business.Departments.AA.Entities.Sql;

namespace Services.Api.Business.Departments.AA.Configuration.Mapping
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

            CreateMap<InventoryModel, InventoryEntity>();

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model

            CreateMap<InventoryEntity, InventoryModel>();

            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}