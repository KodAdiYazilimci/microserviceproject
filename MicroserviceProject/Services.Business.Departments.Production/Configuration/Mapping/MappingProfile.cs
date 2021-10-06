using AutoMapper;

using Services.Business.Departments.Production.Entities.EntityFramework;
using Services.Business.Departments.Production.Models;

namespace Services.Business.Departments.Production.Configuration.Mapping
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
