
using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Gateway.Public.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Gateway.Public
{
    /// <summary>
    /// İnsan kaynakları servisi için iletişim kurucu sınıf
    /// </summary>
    public class HRCommunicator : IDisposable
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
        /// İnsan kaynakları servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public HRCommunicator(
            ServiceCommunicator serviceCommunicator)
        {
            _serviceCommunicator = serviceCommunicator;
        }

        /// <summary>
        /// Departmanları verir
        /// </summary>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<DepartmentModel>> departmentsServiceResult =
                    await
                    _serviceCommunicator.Call<List<DepartmentModel>>(
                        serviceName: "gateway.public.hr.getdepartments",
                        postData: null,
                        queryParameters: null,
                        headers: new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                        },
                        cancellationTokenSource: cancellationTokenSource);

            return departmentsServiceResult;
        }


        /// <summary>
        /// İptal edilmesi istenilen bir session ın düşürülmesi talebini iletir
        /// </summary>
        /// <param name="tokenKey">Düşürülecek session a ait token</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(string tokenKey, CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel serviceResult =
                await _serviceCommunicator.Call(
                    serviceName: "gateway.public.identity.removesessionifexistsincache",
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
