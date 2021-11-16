using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Mq.Rabbit;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Finance;
using Communication.Mq.Rabbit.Publisher.Department.Finance.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.Http.Department.Finance;

namespace Services.MQ.Finance.Util.Consumers.Request
{
    /// <summary>
    /// Finans departmanına üretilmesi istenilen ürünleri tüketen sınıf
    /// </summary>
    public class ProductionRequestConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<ProductionRequestModel> _consumer;

        /// <summary>
        /// Finans departmanı servis iletişimcisi
        /// </summary>
        private readonly FinanceCommunicator _financeCommunicator;

        /// <summary>
        /// Finans departmanına üretilmesi istenilen ürünleri tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="financeCommunicator">Finans departmanı servis iletişimcisi</param>
        public ProductionRequestConsumer(
            ProductionRequestRabbitConfiguration rabbitConfiguration,
            FinanceCommunicator financeCommunicator)
        {
            _financeCommunicator = financeCommunicator;
            _consumer = new Consumer<ProductionRequestModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProductionRequestModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Department.Finance.Models.ProductionRequestModel productionRequest = new Communication.Http.Department.Finance.Models.ProductionRequestModel
            {
                Amount = data.Amount,
                DepartmentId = data.DepartmentId,
                ProductId = data.ProductId,
                ReferenceNumber = data.ReferenceNumber
            };

            await _financeCommunicator.CreateProductionRequestAsync(productionRequest, cancellationTokenSource);
        }

        /// <summary>
        /// Kayıtları yakalamaya başlar
        /// </summary>
        public void StartToConsume()
        {
            _consumer.StartToConsume();
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
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _consumer.Dispose();
                    _financeCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
