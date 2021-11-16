using AutoMapper;

using Communication.Http.Department.Production.Models;

using Services.Business.Departments.Production.Entities.EntityFramework;

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
