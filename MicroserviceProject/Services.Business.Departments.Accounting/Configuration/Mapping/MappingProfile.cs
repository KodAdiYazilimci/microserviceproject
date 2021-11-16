using AutoMapper;

using Communication.Http.Department.Accounting.Models;

using Services.Business.Departments.Accounting.Entities.Sql;

namespace Services.Business.Departments.Accounting.Configuration.Mapping
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

            CreateMap<BankAccountModel, BankAccountEntity>();
            CreateMap<CurrencyModel, CurrencyEntity>();
            CreateMap<SalaryPaymentModel, SalaryPaymentEntity>();

            // Entity => Model
            CreateMap<BankAccountEntity, BankAccountModel>();
            CreateMap<CurrencyEntity, CurrencyModel>();
            CreateMap<SalaryPaymentEntity, SalaryPaymentModel>();
        }
    }
}
