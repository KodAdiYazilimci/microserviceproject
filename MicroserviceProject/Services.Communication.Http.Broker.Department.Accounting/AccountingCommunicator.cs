﻿using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Providers;

using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Responses;
using Services.Communication.Http.Broker.Department.Accounting.CQRS.Queries.Responses;

namespace Services.Communication.Http.Broker.Department.Accounting
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
        public async Task<ServiceResultModel<GetBankAccountsOfWorkerQueryResponse>> GetBankAccountsOfWorkerAsync(
            int workerId,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<GetBankAccountsOfWorkerQueryResponse> bankAccountsServiceResult =
                 await _serviceCommunicator.Call<GetBankAccountsOfWorkerQueryResponse>(
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
        /// <param name="request">Çalışan modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<CreateBankAccountCommandResponse>> CreateBankAccountAsync(
            CreateBankAccountCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<CreateBankAccountCommandResponse>(
               serviceName: _routeNameProvider.Accounting_CreateBankAccount,
               postData: request,
               queryParameters: null,
               headers: new List<KeyValuePair<string, string>>()
               {
                   new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
               },
               cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Para birimlerini getirir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<GetCurrenciesQueryResponse>> GetCurrenciesAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<GetCurrenciesQueryResponse>(
                serviceName: _routeNameProvider.Accounting_GetCurrencies,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Para birimi oluşturur
        /// </summary>
        /// <param name="request">Para birimi modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<CreateCurrencyCommandResponse>> CreateCurrencyAsync(
            CreateCurrencyCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<CreateCurrencyCommandResponse>(
                serviceName: _routeNameProvider.Accounting_CreateCurrency,
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Çalışanın maaş ödemelerini verir
        /// </summary>
        /// <param name="workerId">Maaş bilgileri getirilecek çalışanın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<GetSalaryPaymentsOfWorkerQueryResponse>> GetSalaryPaymentsOfWorker(
            int workerId,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<GetSalaryPaymentsOfWorkerQueryResponse>(
                serviceName: _routeNameProvider.Accounting_GetSalaryPaymentsOfWorker,
                postData: null,
                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("workerId", workerId.ToString()) },
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Çalışana maaş ödemesi oluşturur
        /// </summary>
        /// <param name="request">Maaş ödeme modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<CreateSalaryPaymentCommandResponse>> CreateSalaryPayment(
            CreateSalaryPaymentCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<CreateSalaryPaymentCommandResponse>(
                serviceName: _routeNameProvider.Accounting_CreateSalaryPayment,
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// İptal edilmesi istenilen bir session ın düşürülmesi talebini iletir
        /// </summary>
        /// <param name="tokenKey">Düşürülecek session a ait token</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
            string tokenKey,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel serviceResult =
                await _serviceCommunicator.Call(
                    serviceName: _routeNameProvider.Accounting_RemoveSessionIfExistsInCache,
                    postData: null,
                    queryParameters: new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("tokenKey",tokenKey)
                    },
                    headers: null,
                    cancellationTokenSource);

            return serviceResult;
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
