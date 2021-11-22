using AutoMapper;

using Services.Communication.Http.Broker.Department.Production.Models;

using Services.Api.Business.Departments.Production.Entities.EntityFramework;

namespace Services.Api.Business.Departments.Production.Configuration.Mapping
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

            CreateMap<ProductModel, ProductEntity>();

            // Entity => Model

            CreateMap<ProductEntity, ProductModel>();
        }
    }
}
