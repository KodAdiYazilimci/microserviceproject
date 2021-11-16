using AutoMapper;

using Communication.Http.Department.Storage.Models;

using Services.Business.Departments.Storage.Entities.EntityFramework;

namespace Services.Business.Departments.Storage.Configuration.Mapping
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

            CreateMap<StockModel, StockEntity>();

            // Entity => Model

            CreateMap<StockEntity, StockModel>();
        }
    }
}
