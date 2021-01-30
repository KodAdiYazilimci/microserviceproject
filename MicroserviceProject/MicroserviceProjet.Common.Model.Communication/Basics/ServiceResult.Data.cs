using System;
using System.Collections.Generic;
using System.Text;

namespace MicroserviceProject.Common.Model.Communication.Basics
{
    /// <summary>
    /// Bir servisten dönen yanıt
    /// </summary>
    /// <typeparam name="TResult">Servisten dönecek datanın tipi</typeparam>
    public class ServiceResult<TResult> : ServiceResult
    {
        /// <summary>
        /// Yanıtın içerisindeki data
        /// </summary>
        public TResult Data { get; set; }
    }
}
