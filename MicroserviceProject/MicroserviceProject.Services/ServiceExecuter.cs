using MicroserviceProject.Infrastructure.Communication.Moderator.Exceptions;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceProject.Services
{
    /// <summary>
    /// İş mantığı servislerini çalıştırarak Http response aşamasına getiren sınıf
    /// </summary>
    public class ServiceExecuter
    {
        /// <summary>
        /// İş mantığı servislerini çalıştırır ve Http response üretir
        /// </summary>
        /// <param name="func">Servisin çalıştırıldığı fonksiyon gövdesi</param>
        /// <param name="services">Çalıştırılan servisler</param>
        /// <returns></returns>
        public static IActionResult ExecuteService(Action func, params BaseService[] services)
        {
            try
            {
                func();

                ServiceResult serviceResult = new ServiceResult()
                {
                    IsSuccess = true,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                };

                return new OkObjectResult(serviceResult);
            }
            catch (ValidationException vex)
            {
                return new BadRequestObjectResult(vex.ValidationResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() },
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                });
            }
        }

        /// <summary>
        /// İş mantığı servislerini çalıştırır ve Http response üretir
        /// </summary>
        /// <typeparam name="TResult">Http response gövdesindeki verinin tipi</typeparam>
        /// <param name="func">Servisin çalıştırıldığı fonksiyon gövdesi</param>
        /// <param name="services">Çalıştırılan servisler</param>
        /// <returns></returns>
        public static IActionResult ExecuteService<TResult>(Func<TResult> func, params BaseService[] services)
        {
            try
            {
                TResult result = func();

                ServiceResult<TResult> serviceResult = new ServiceResult<TResult>()
                {
                    IsSuccess = true,
                    Data = result,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                };

                return new OkObjectResult(serviceResult);
            }
            catch (ValidationException vex)
            {
                return new BadRequestObjectResult(vex.ValidationResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() },
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                });
            }
        }

        /// <summary>
        /// İş mantığı servislerini çalıştırır ve Http response üretir
        /// </summary>
        /// <param name="func">Servisin çalıştırıldığı fonksiyon gövdesi</param>
        /// <param name="services">Çalıştırılan servisler</param>
        /// <returns></returns>
        public static async Task<IActionResult> ExecuteServiceAsync(Func<Task> func, params BaseService[] services)
        {
            try
            {
                await func();

                ServiceResult serviceResult = new ServiceResult()
                {
                    IsSuccess = true,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                };

                return new OkObjectResult(serviceResult);
            }
            catch (ValidationException vex)
            {
                return new BadRequestObjectResult(vex.ValidationResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() },
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                });
            }
        }

        /// <summary>
        /// İş mantığı servislerini çalıştırır ve Http response üretir
        /// </summary>
        /// <typeparam name="TResult">Http response gövdesindeki verinin tipi</typeparam>
        /// <param name="func">Servisin çalıştırıldığı fonksiyon gövdesi</param>
        /// <param name="services">Çalıştırılan servisler</param>
        /// <returns></returns>
        public static async Task<IActionResult> ExecuteServiceAsync<TResult>(Func<Task<TResult>> func, params BaseService[] services)
        {
            try
            {
                TResult result = await func();

                ServiceResult<TResult> serviceResult = new ServiceResult<TResult>()
                {
                    IsSuccess = true,
                    Data = result,
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                };

                return new OkObjectResult(serviceResult);
            }
            catch (ValidationException vex)
            {
                return new BadRequestObjectResult(vex.ValidationResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResult()
                {
                    IsSuccess = false,
                    Error = new Error() { Description = ex.ToString() },
                    Transaction = new Infrastructure.Communication.Moderator.Models.Transaction()
                    {
                        TransactionIdentity =
                            services.Any(x => !string.IsNullOrEmpty(x.TransactionIdentity))
                            ?
                            services.FirstOrDefault(x => !string.IsNullOrEmpty(x.TransactionIdentity)).TransactionIdentity
                            :
                            string.Empty,
                        Modules = services.Select(x => x.ServiceName).ToList()
                    }
                });
            }
        }
    }
}
