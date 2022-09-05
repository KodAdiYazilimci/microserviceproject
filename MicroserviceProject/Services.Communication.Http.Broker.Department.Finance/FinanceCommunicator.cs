using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.Finance.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Finance.Models;

namespace Services.Communication.Http.Broker.Department.Finance
{
    /// <summary>
    /// Finans servisi için iletişim kurucu sınıf
    /// </summary>
    public class FinanceCommunicator : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Finans servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public FinanceCommunicator(
            ServiceCommunicator serviceCommunicator)
        {
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Finans departmanı için masraf kararı oluşturur
        /// </summary>
        /// <param name="request">Masraf kararı modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> CreateCostAsync(
            CreateCostCommandRequest request, 
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "finance.cost.createcost",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Finans departmanı için ürün üretim kararı oluşturur
        /// </summary>
        /// <param name="request">Ürün üretim kararı modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> CreateProductionRequestAsync(
            CreateProductionRequestCommandRequest request, 
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "finance.productionrequest.createproductionrequest",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Finans departmanındaki karar verilen masrafları verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<DecidedCostModel>>> GetDecidedCostsAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<DecidedCostModel>>(
                serviceName: "finance.cost.getdecidedcosts",
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Finans departmanındaki masrafa onay veya red verir
        /// </summary>
        /// <param name="request">Masraf modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> DecideCostAsync(
            DecideCostCommandRequest request, 
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "finance.cost.decidecost",
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
                    serviceName: "finance.identity.removesessionifexistsincache",
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
                    _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
