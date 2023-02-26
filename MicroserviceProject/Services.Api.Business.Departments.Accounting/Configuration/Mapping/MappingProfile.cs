using AutoMapper;

using Services.Communication.Http.Broker.Department.Accounting.Models;

using Services.Api.Business.Departments.Accounting.Entities.Sql;
using Infrastructure.Transaction.Recovery;

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
            CreateMap<AccountingCurrencyModel, CurrencyEntity>()
                .ForMember(x => x.SalaryPayments, y => y.Ignore());

            CreateMap<AccountingSalaryPaymentModel, SalaryPaymentEntity>();

            CreateMap<RollbackModel, RollbackEntity>();
            CreateMap<RollbackItemModel, RollbackItemEntity>();

            // Entity => Model
            CreateMap<BankAccountEntity, AccountingBankAccountModel>();
            CreateMap<CurrencyEntity, AccountingCurrencyModel>();
            CreateMap<SalaryPaymentEntity, AccountingSalaryPaymentModel>();

            CreateMap<RollbackEntity, RollbackModel>();
            CreateMap<RollbackItemEntity, RollbackItemModel>();
        }
    }
}
