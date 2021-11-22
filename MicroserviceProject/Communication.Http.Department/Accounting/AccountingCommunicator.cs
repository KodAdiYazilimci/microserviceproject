using Communication.Http.Department.Accounting.Models;

using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Http.Department.Accounting
{
    /// <summary>
    /// Muhasebe servisi için iletişim kurucu sınıf
    /// </summary>
    public class AccountingCommunicator : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Muhasebe servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public AccountingCommunicator(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Çalışanın banka hesaplarını getirir
        /// </summary>
        /// <param name="workerId">Çalışanın Id değeri</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<BankAccountModel>>> GetBankAccountsOfWorkerAsync(int workerId, string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<BankAccountModel>> bankAccountsServiceResult =
                 await _serviceCommunicator.Call<List<BankAccountModel>>(
                     serviceName: _routeNameProvider.Accounting_GetBankAccountsOfWorker,
                     postData: null,
                     queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("workerId", workerId.ToString()) },
                     headers: new List<KeyValuePair<string, string>>()
                     {
                         new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                     },
                     cancellationTokenSource: cancellationTokenSource);

            return bankAccountsServiceResult;
        }

        /// <summary>
        /// Çalışan için banka hesabı oluşturur
        /// </summary>
        /// <param name="workerModel">Çalışan modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> CreateBankAccountAsync(WorkerModel workerModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
               serviceName: _routeNameProvider.Accounting_CreateBankAccount,
               postData: workerModel,
               queryParameters: null,
               headers: null,
               cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Para birimlerini getirir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<CurrencyModel>>> GetCurrenciesAsync(CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<CurrencyModel>>(
                serviceName: _routeNameProvider.Accounting_GetCurrencies,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Para birimi oluşturur
        /// </summary>
        /// <param name="currencyModel">Para birimi modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> CreateCurrencyAsync(CurrencyModel currencyModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.Accounting_CreateCurrency,
                postData: currencyModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Çalışanın maaş ödemelerini verir
        /// </summary>
        /// <param name="workerId">Maaş bilgileri getirilecek çalışanın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<SalaryPaymentModel>>> GetSalaryPaymentsOfWorker(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<SalaryPaymentModel>>(
                serviceName: _routeNameProvider.Accounting_GetSalaryPaymentsOfWorker,
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("workerId", workerId.ToString()) },
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Çalışana maaş ödemesi oluşturur
        /// </summary>
        /// <param name="salaryPaymentModel">Maaş ödeme modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> CreateSalaryPayment(SalaryPaymentModel salaryPaymentModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.Accounting_CreateSalaryPayment,
                postData: salaryPaymentModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _routeNameProvider.Dispose();
                    _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
