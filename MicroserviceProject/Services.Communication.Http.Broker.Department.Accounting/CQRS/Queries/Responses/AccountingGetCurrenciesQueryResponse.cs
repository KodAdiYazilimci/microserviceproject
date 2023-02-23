using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses
{
    public class AccountingGetCurrenciesQueryResponse
    {
        public List<AccountingCurrencyModel> Currencies { get; set; }
    }
}
