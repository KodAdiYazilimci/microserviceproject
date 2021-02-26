using MicroserviceProject.Infrastructure.Communication.Model;
using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;
using MicroserviceProject.Infrastructure.Validation.Exceptions;

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

                ServiceResultModel serviceResult = new ServiceResultModel()
                {
                    IsSuccess = true,
                    Transaction = new Infrastructure.Communication.Model.TransactionModel()
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
                var serviceResult = new ServiceResultModel()
                {
                    IsSuccess = false,
                    Validation = vex.ValidationResult
                };

                return new BadRequestObjectResult(serviceResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = ex.ToString() },
                    Transaction = new Infrastructure.Communication.Model.TransactionModel()
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

                ServiceResultModel<TResult> serviceResult = new ServiceResultModel<TResult>()
                {
                    IsSuccess = true,
                    Data = result,
                    Transaction = new TransactionModel()
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
                var serviceResult = new ServiceResultModel()
                {
                    IsSuccess = false,
                    Validation = vex.ValidationResult
                };

                return new BadRequestObjectResult(serviceResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = ex.ToString() },
                    Transaction = new TransactionModel()
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

                ServiceResultModel serviceResult = new ServiceResultModel()
                {
                    IsSuccess = true,
                    Transaction = new TransactionModel()
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
                var serviceResult = new ServiceResultModel()
                {
                    IsSuccess = false,
                    Validation = vex.ValidationResult
                };

                return new BadRequestObjectResult(serviceResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = ex.ToString() },
                    Transaction = new TransactionModel()
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

                ServiceResultModel<TResult> serviceResult = new ServiceResultModel<TResult>()
                {
                    IsSuccess = true,
                    Data = result,
                    Transaction = new TransactionModel()
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
                var serviceResult = new ServiceResultModel()
                {
                    IsSuccess = false,
                    Validation = vex.ValidationResult
                };

                return new BadRequestObjectResult(serviceResult);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ServiceResultModel()
                {
                    IsSuccess = false,
                    ErrorModel = new ErrorModel() { Description = ex.ToString() },
                    Transaction = new TransactionModel()
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
