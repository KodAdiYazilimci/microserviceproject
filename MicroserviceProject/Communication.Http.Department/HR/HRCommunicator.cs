using Communication.Http.Department.HR.Models;

using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Http.Department.HR
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
        /// Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// İnsan kaynakları servisi için iletişim kurucu sınıf
        /// </summary>
        /// <param name="routeNameProvider">Servis rotalarına ait endpoint isimlerini sağlayan sınıfın nesnesi</param>
        /// <param name="serviceCommunicator">Yetki denetimi destekli servis iletişim sağlayıcı sınıfın nesnesi</param>
        public HRCommunicator(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
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
                        serviceName: _routeNameProvider.HR_GetDepartments,
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
        /// Yeni departman oluşturur
        /// </summary>
        /// <param name="departmentModel">Departman modeli</param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> CreateDepartmentAsync(DepartmentModel departmentModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.HR_CreateDepartment,
                postData: departmentModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Kişi listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<PersonModel>>> GetPeopleAsync(CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<PersonModel>>(
                serviceName: _routeNameProvider.HR_GetPeople,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Kişi oluşturur
        /// </summary>
        /// <param name="personModel">Kişi modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<int>> CreatePersonAsync(PersonModel personModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.HR_CreatePerson,
                postData: personModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Ünvanları verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenu</param>
        public async Task<ServiceResultModel<List<TitleModel>>> GetTitlesAsync(CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<TitleModel>>(
                serviceName: _routeNameProvider.HR_GetTitles,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni bir ünvan oluşturur
        /// </summary>
        /// <param name="titleModel">Ünvan modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel<int>> CreateTitleAsync(TitleModel titleModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.HR_CreateTitle,
                postData: titleModel,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Çalışanları verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel<List<WorkerModel>>> GetWorkersAsync(CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<WorkerModel>>(
                serviceName: _routeNameProvider.HR_GetWorkers,
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni bir çalışan oluşturur
        /// </summary>
        /// <param name="workerModel">Çalışan modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel<int>> CreateWorkerAsync(WorkerModel workerModel, CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.HR_CreateWorker,
                postData: workerModel,
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
