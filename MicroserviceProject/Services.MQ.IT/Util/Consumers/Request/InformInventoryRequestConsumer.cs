using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Mq.Rabbit;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.IT;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.IT.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.IT.Util.Consumers.Inventory
{
    /// <summary>
    /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
    /// </summary>
    public class InformInventoryRequestConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<InventoryRequestModel> _consumer;

        /// <summary>
        /// Kuyruktan alınan verinin iletileceği servisin adını veren nesne
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Kuyruktan alınan verinin iletileceği servisle iletişimi kuracak nesne
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="routeNameProvider">Kuyruktan alınan verinin iletileceği servisin adını veren nesne</param>
        /// <param name="serviceCommunicator">Kuyruktan alınan verinin iletileceği servisle iletişimi kuracak nesne</param>
        public InformInventoryRequestConsumer(
            ITInformInventoryRequestRabbitConfiguration rabbitConfiguration,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;

            _consumer = new Consumer<InventoryRequestModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(InventoryRequestModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            _ = await _serviceCommunicator.Call<int>(
                serviceName: _routeNameProvider.IT_InformInventoryRequest,
                postData: data,
                queryParameters: null,
                headers: null,
                cancellationTokenSource: cancellationTokenSource);
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
                    _routeNameProvider.Dispose();
                    _serviceCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
