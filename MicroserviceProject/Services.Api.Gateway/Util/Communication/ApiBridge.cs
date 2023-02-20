using Infrastructure.Communication.Http.Exceptions;
using Infrastructure.Communication.Http.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Gateway.Util.Communication
{
    /// <summary>
    /// Api servislerle iletişim köprüsü kuran sınıf
    /// </summary>
    public class ApiBridge
    {
        /// <summary>
        /// İletişim esnasında aktarılacak transaction kimliği
        /// </summary>
        public string TransactionIdentity { get; set; }// = Guid.NewGuid().ToString();

        /// <summary>
        /// Api servisiyle iletişim kurar
        /// </summary>
        /// <typeparam name="TResult">Api servisinden dönecek yanıtın tipi</typeparam>
        /// <param name="func">İletişim kuracak method</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        /// <exception cref="CallException">Servis çağrısı esnasında ortaya çıkan sorunlarda fırlatılacak istisnai durum</exception>
        public async Task<TResult> CallAsync<TResult>(
            Func<string, CancellationTokenSource, Task<ServiceResultModel<TResult>>> func, 
            CancellationTokenSource cancellationTokenSource)
        {
            ServiceResultModel<TResult> apiServiceResult = await func(TransactionIdentity, cancellationTokenSource);

            if (apiServiceResult.IsSuccess)
            {
                return apiServiceResult.Data;
            }
            else
            {
                throw new CallException(
                        message: apiServiceResult.ErrorModel.Description,
                        endpoint: apiServiceResult.SourceApiService,
                        error: apiServiceResult.ErrorModel,
                        validation: apiServiceResult.Validation);
            }
        }
    }
}
