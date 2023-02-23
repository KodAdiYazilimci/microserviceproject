using AutoMapper;

using Services.Communication.Http.Broker.Department.Accounting.Models;

using Services.Api.Business.Departments.Accounting.Entities.Sql;

namespace Services.Api.Business.Departments.Accounting.Configuration.Mapping
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

            CreateMap<AccountingBankAccountModel, BankAccountEntity>();
            CreateMap<AccountingCurrencyModel, CurrencyEntity>();
            CreateMap<AccountingSalaryPaymentModel, SalaryPaymentEntity>();

            // Entity => Model
            CreateMap<BankAccountEntity, AccountingBankAccountModel>();
            CreateMap<CurrencyEntity, AccountingCurrencyModel>();
            CreateMap<SalaryPaymentEntity, AccountingSalaryPaymentModel>();
        }
    }
}
