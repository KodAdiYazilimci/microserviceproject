using AutoMapper;

using Infrastructure.Transaction.Recovery;

using Services.Business.Departments.Selling.Entities.EntityFramework;
using Services.Business.Departments.Selling.Models;

namespace Services.Business.Departments.Selling.Configuration.Mapping
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

            CreateMap<SellModel, SellEntity>();

            // Entity => Model

            CreateMap<SellEntity, SellModel>();
        }
    }
}
