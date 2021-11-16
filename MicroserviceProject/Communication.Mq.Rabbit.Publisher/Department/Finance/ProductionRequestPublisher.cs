using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Finance;
using Communication.Mq.Rabbit.Publisher.Department.Finance.Models;

using System;

namespace Communication.Mq.Rabbit.Publisher.Department.Finance
{
    /// <summary>
    /// Finans departmanına üretilmesi istenilen ürünler için talep açar
    /// </summary>
    public class ProductionRequestPublisher : BasePublisher<ProductionRequestModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Finans departmanına üretilmesi istenilen ürünler için talep açar
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public ProductionRequestPublisher(
            ProductionRequestRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }
    }
}
