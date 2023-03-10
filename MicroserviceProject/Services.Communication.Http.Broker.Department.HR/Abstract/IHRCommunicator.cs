using Infrastructure.Communication.Http.Models;

using Services.Communication.Http.Broker.Department.HR.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.HR.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Communication.Http.Broker.Department.HR.Abstract
{
    public interface IHRCommunicator
    {
        Task<ServiceResultModel<List<DepartmentModel>>> GetDepartmentsAsync(
           string transactionIdentity,
           CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateDepartmentAsync(
            CreateDepartmentCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<PersonModel>>> GetPeopleAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreatePersonAsync(
            CreatePersonCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<TitleModel>>> GetTitlesAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateTitleAsync(
          CreateTitleCommandRequest request,
          string transactionIdentity,
          CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel<List<WorkerModel>>> GetWorkersAsync(
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> CreateWorkerAsync(
            CreateWorkerCommandRequest request,
            string transactionIdentity,
            CancellationTokenSource cancellationTokenSource);

        Task<ServiceResultModel> RemoveSessionIfExistsInCacheAsync(
           string tokenKey,
           CancellationTokenSource cancellationTokenSource);
    }
}
