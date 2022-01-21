using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses
{
    public class GetCurrenciesQueryResponse
    {
        public List<CurrencyModel> Currencies { get; set; }
    }
}
