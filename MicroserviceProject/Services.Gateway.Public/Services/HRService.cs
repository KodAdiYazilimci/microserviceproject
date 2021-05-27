using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Model.Basics;
using Infrastructure.Communication.Broker;
using Infrastructure.Routing.Providers;
using Infrastructure.Transaction.ExecutionHandler;

using Services.Gateway.Public.Models.HR;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Gateway.Public.Services
{
    /// <summary>
    /// İnsan kaynakları servisi
    /// </summary>
    public class HRService : BaseService, IDisposable, IDisposableInjections
    {
        /// <summary>
        /// Rota isim sağlayıcısı nesnesi
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Servis iletişimcisi nesnesi
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// İnsan kaynakları servisi
        /// </summary>
        /// <param name="routeNameProvider">Rota isim sağlayıcısı nesnesi</param>
        /// <param name="serviceCommunicator">Servis iletişimcisi nesnesi</param>
        public HRService(
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;
        }

        public override string ApiServiceName => "Services.Gateway.Public";
        public override string ServiceName => "Services.Gateway.Public.Services.HRService";

        public void DisposeInjections()
        {
            _routeNameProvider.Dispose();
            _serviceCommunicator.Dispose();
        }

        /// <summary>
        /// Departmanları getirir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<DepartmentModel>> departmentsServiceResult =
                    await
                    _serviceCommunicator.Call<List<DepartmentModel>>(
                        serviceName: _routeNameProvider.HR_GetDepartments,
                        postData: null,
                        queryParameters: null,
                        headers: new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("TransactionIdentity", TransactionIdentity)
                        },
                        cancellationTokenSource: cancellationTokenSource);

            if (departmentsServiceResult.IsSuccess)
            {
                return departmentsServiceResult.Data;
            }
            else
            {
                throw new CallException(
                        message: departmentsServiceResult.ErrorModel.Description,
                        endpoint:
                        !string.IsNullOrEmpty(departmentsServiceResult.SourceApiService)
                        ?
                        departmentsServiceResult.SourceApiService
                        :
                        $"{ApiServiceName}).{nameof(HRService)}.{nameof(GetDepartmentsAsync)}",
                        error: departmentsServiceResult.ErrorModel,
                        validation: departmentsServiceResult.Validation);
            }
        }
    }
}
