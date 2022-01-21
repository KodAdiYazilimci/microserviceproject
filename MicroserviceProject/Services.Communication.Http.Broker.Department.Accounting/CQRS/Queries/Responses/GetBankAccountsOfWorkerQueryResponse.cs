using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses
{
    public class GetBankAccountsOfWorkerQueryResponse
    {
        public List<BankAccountModel> BankAccounts { get; set; }
    }
}
