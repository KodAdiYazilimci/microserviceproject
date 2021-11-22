using Communication.Http.Department.HR;
using Communication.Http.Department.HR.Models;

using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Wrapper;

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
        /// İnsan kaynakları servis iletişimcisi
        /// </summary>
        private readonly HRCommunicator _hrCommunicator;

        /// <summary>
        /// İnsan kaynakları servisi
        /// </summary>
        /// <param name="hrCommunicator">İnsan kaynakları servis iletişimcisi</param>
        public HRService(HRCommunicator hrCommunicator)
        {
            _hrCommunicator = hrCommunicator;
        }

        public override string ApiServiceName => "Services.Gateway.Public";
        public override string ServiceName => "Services.Gateway.Public.Services.HRService";

        public void DisposeInjections()
        {
            _hrCommunicator.Dispose();
        }

        /// <summary>
        /// Departmanları getirir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<List<DepartmentModel>> departmentsServiceResult = await _hrCommunicator.GetDepartmentsAsync(TransactionIdentity, cancellationTokenSource);

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
