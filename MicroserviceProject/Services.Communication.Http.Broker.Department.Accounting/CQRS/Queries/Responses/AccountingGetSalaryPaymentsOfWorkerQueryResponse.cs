using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses
{
    public class AccountingGetSalaryPaymentsOfWorkerQueryResponse
    {
        public List<AccountingSalaryPaymentModel> SalaryPayments { get; set; }
    }
}
