using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.Models;

namespace Services.Communication.Http.Broker.Department.Accounting.Abstract
{
    public interface IAccountingCommunicator : IDisposable
    {
        Task<ServiceResultModel<List<AccountingBankAccountModel>>> GetBankAccountsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateBankAccountAsync(
            AccountingCreateBankAccountCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<AccountingCurrencyModel>>> GetCurrenciesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateCurrencyAsync(
            AccountingCreateCurrencyCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<AccountingSalaryPaymentModel>>> GetSalaryPaymentsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateSalaryPaymentAsync(
           AccountingCreateSalaryPaymentCommandRequest request,
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource);
    }
}
