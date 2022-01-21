using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses
{
    public class GetSalaryPaymentsOfWorkerQueryResponse
    {
        public List<SalaryPaymentModel> SalaryPayments { get; set; }
    }
}
