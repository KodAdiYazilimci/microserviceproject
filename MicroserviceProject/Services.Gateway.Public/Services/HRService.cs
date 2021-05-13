using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Model.Basics;
using Infrastructure.Communication.Model.Department.HR;
using Infrastructure.Communication.Moderator;
using Infrastructure.Routing.Providers;
using Infrastructure.Transaction.ExecutionHandler;
using Infrastructure.Validation.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Gateway.Public.Services
{
    public class HRService : BaseService, IDisposable, IDisposableInjections
    {
        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;

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
