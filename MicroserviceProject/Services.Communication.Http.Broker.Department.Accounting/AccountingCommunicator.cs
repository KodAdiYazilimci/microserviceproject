//using Infrastructure.Communication.Http.Broker;
//using Infrastructure.Communication.Http.Models;

//using Services.Communication.Http.Broker.Department.Accounting.CQRS.Commands.Requests;
//using Services.Communication.Http.Broker.Department.Accounting.Models;

//namespace Services.Communication.Http.Broker.Department.Accounting
//{
//    /// <summary>
//    /// Muhasebe servisi için iletişim kurucu sınıf
//    /// </summary>
//    public class AccountingCommunicator : IDisposable
//    {
//        /// <summary>
//        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
//        /// </summary>
//        private bool disposed = false;

//        /// <summary>
//        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
//        /// </summary>
//        private readonly ServiceCommunicator _serviceCommunicator;

//        /// <summary>
//        /// Muhasebe servisi için iletişim kurucu sınıf
//        /// </summary>
//        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
//        public AccountingCommunicator(
//            ServiceCommunicator serviceCommunicator)
//        {
//            _serviceCommunicator = serviceCommunicator;
//        }

//        /// <summary>
//        /// Çalışanın banka hesaplarını getirir
//        /// </summary>
//        /// <param name="workerId">Çalışanın Id değeri</param>
//        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel<List<BankAccountModel>>> GetBankAccountsOfWorkerAsync(
//            int workerId,
//            string transactionIdentity,
//            CancellationTokenSource cancellationTokenSource)
//        {
//            ServiceResultModel<List<BankAccountModel>> bankAccountsServiceResult =
//                 await _serviceCommunicator.Call<List<BankAccountModel>>(
//                     serviceName: "accounting.bankaccounts.getbankaccountsofworker",
//                     postData: null,
//                     queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("workerId", workerId.ToString()) },
//                     headers: new List<KeyValuePair<string, string>>()
//                     {
//                         new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
//                     },
//                     cancellationTokenSource: cancellationTokenSource);

//            return bankAccountsServiceResult;
//        }

//        /// <summary>
//        /// Çalışan için banka hesabı oluşturur
//        /// </summary>
//        /// <param name="request">Çalışan modeli</param>
//        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel> CreateBankAccountAsync(
//            CreateBankAccountCommandRequest request,
//            string transactionIdentity,
//            CancellationTokenSource cancellationTokenSource)
//        {
//            return await _serviceCommunicator.Call(
//               serviceName: "accounting.bankaccounts.createbankaccount",
//               postData: request,
//               queryParameters: null,
//               headers: new List<KeyValuePair<string, string>>()
//               {
//                   new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
//               },
//               cancellationTokenSource: cancellationTokenSource);
//        }

//        /// <summary>
//        /// Para birimlerini getirir
//        /// </summary>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel<List<CurrencyModel>>> GetCurrenciesAsync(
//            CancellationTokenSource cancellationTokenSource)
//        {
//            return await _serviceCommunicator.Call<List<CurrencyModel>>(
//                serviceName: "accounting.bankaccounts.getcurrencies",
//                postData: null,
//                queryParameters: null,
//                headers: null,
//                cancellationTokenSource: cancellationTokenSource);
//        }

//        /// <summary>
//        /// Para birimi oluşturur
//        /// </summary>
//        /// <param name="request">Para birimi modeli</param>
//        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel> CreateCurrencyAsync(
//            CreateCurrencyCommandRequest request,
//            string transactionIdentity,
//            CancellationTokenSource cancellationTokenSource)
//        {
//            return await _serviceCommunicator.Call(
//                serviceName: "accounting.bankaccounts.createcurrency",
//                postData: request,
//                queryParameters: null,
//                headers: new List<KeyValuePair<string, string>>()
//                {
//                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
//                },
//                cancellationTokenSource: cancellationTokenSource);
//        }

//        /// <summary>
//        /// Çalışanın maaş ödemelerini verir
//        /// </summary>
//        /// <param name="workerId">Maaş bilgileri getirilecek çalışanın Id değeri</param>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel<List<SalaryPaymentModel>>> GetSalaryPaymentsOfWorker(
//            int workerId,
//            CancellationTokenSource cancellationTokenSource)
//        {
//            return await _serviceCommunicator.Call<List<SalaryPaymentModel>>(
//                serviceName: "accounting.bankaccounts.getsalarypaymentsofworker",
//                postData: null,
//                queryParameters: new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("workerId", workerId.ToString()) },
//                headers: null,
//                cancellationTokenSource: cancellationTokenSource);
//        }

//        /// <summary>
//        /// Çalışana maaş ödemesi oluşturur
//        /// </summary>
//        /// <param name="request">Maaş ödeme modeli</param>
//        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel> CreateSalaryPayment(
//            CreateSalaryPaymentCommandRequest request,
//            string transactionIdentity,
//            CancellationTokenSource cancellationTokenSource)
//        {
//            return await _serviceCommunicator.Call(
//                serviceName: "accounting.bankaccounts.createsalarypayment",
//                postData: request,
//                queryParameters: null,
//                headers: new List<KeyValuePair<string, string>>()
//                {
//                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
//                },
//                cancellationTokenSource: cancellationTokenSource);
//        }

//        /// <summary>
//        /// İptal edilmesi istenilen bir session ın düşürülmesi talebini iletir
//        /// </summary>
//        /// <param name="tokenKey">Düşürülecek session a ait token</param>
//        /// <param name="cancellationTokenSource">İptal tokenı</param>
//        /// <returns></returns>
//        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
//            string tokenKey,
//            CancellationTokenSource cancellationTokenSource)
//        {
//            ServiceResultModel serviceResult =
//                await _serviceCommunicator.Call(
//                    serviceName: "accounting.identity.removesessionifexistsincache",
//                    postData: null,
//                    queryParameters: new List<KeyValuePair<string, string>>()
//                    {
//                        new KeyValuePair<string, string>("tokenKey",tokenKey)
//                    },
//                    headers: null,
//                    cancellationTokenSource);

//            return serviceResult;
//        }

//        /// <summary>
//        /// Kaynakları serbest bırakır
//        /// </summary>
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        /// <summary>
//        /// Kaynakları serbest bırakır
//        /// </summary>
//        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
//        public virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (!disposed)
//                {
//                    _serviceCommunicator.Dispose();
//                }

//                disposed = true;
//            }
//        }
//    }
//}
