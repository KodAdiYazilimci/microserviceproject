using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses
{
    public class AccountingGetBankAccountsOfWorkerQueryResponse
    {
        public List<AccountingBankAccountModel> BankAccounts { get; set; }
    }
}
