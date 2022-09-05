using Infrastructure.Communication.Http.Broker;
using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

namespace Services.Communication.Http.Broker.Department.HR
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
        public async Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<DepartmentModel>> departmentsServiceResult =
                    await
                    _serviceCommunicator.Call<List<DepartmentModel>>(
                        serviceName: "hr.department.getdepartments",
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
        /// <param name="request">Departman modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task<ServiceResultModel> CreateDepartmentAsync(
            CreateDepartmentCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "hr.department.createdepartment",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Kişi listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel<List<PersonModel>>> GetPeopleAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<PersonModel>>(
                serviceName: "hr.person.getpeople",
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Kişi oluşturur
        /// </summary>
        /// <param name="request">Kişi modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ServiceResultModel> CreatePersonAsync(
            CreatePersonCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "hr.person.createperson",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Ünvanları verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenu</param>
        public async Task<ServiceResultModel<List<TitleModel>>> GetTitlesAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<TitleModel>>(
                serviceName: "hr.person.gettitles",
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni bir ünvan oluşturur
        /// </summary>
        /// <param name="request">Ünvan modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel> CreateTitleAsync(
            CreateTitleCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<int>(
                serviceName: "hr.person.createtitle",
                postData: request,
                queryParameters: null,
                headers: new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("TransactionIdentity", transactionIdentity)
                },
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Çalışanları verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel<List<WorkerModel>>> GetWorkersAsync(
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call<List<WorkerModel>>(
                serviceName: "hr.person.getworkers",
                postData: null,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
        }

        /// <summary>
        /// Yeni bir çalışan oluşturur
        /// </summary>
        /// <param name="request">Çalışan modeli</param>
        /// <param name="transactionIdentity">Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public async Task<ServiceResultModel> CreateWorkerAsync(
            CreateWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource)
        {
            return await _serviceCommunicator.Call(
                serviceName: "hr.person.createworker",
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
                    serviceName: "hr.identity.removesessionifexistsincache",
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
