using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Routing.Providers;
using MicroserviceProject.Services.Communication.Configuration.Rabbit.IT;
using MicroserviceProject.Services.Model.Department.HR;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.MQ.IT.Util.Consumers.Inventory
{
    /// <summary>
    /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
    /// </summary>
    public class AssignInventoryToWorkerConsumer
    {
        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<WorkerModel> _consumer;

        /// <summary>
        /// Kuyruktan alınan verinin iletileceği servisin adını veren nesne
        /// </summary>
        private readonly RouteNameProvider _routeNameProvider;

        /// <summary>
        /// Kuyruktan alınan verinin iletileceği servisle iletişimi kuracak nesne
        /// </summary>
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="routeNameProvider">Kuyruktan alınan verinin iletileceği servisin adını veren nesne</param>
        /// <param name="serviceCommunicator">Kuyruktan alınan verinin iletileceği servisle iletişimi kuracak nesne</param>
        public AssignInventoryToWorkerConsumer(
            ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration,
            RouteNameProvider routeNameProvider,
            ServiceCommunicator serviceCommunicator)
        {
            _routeNameProvider = routeNameProvider;
            _serviceCommunicator = serviceCommunicator;

            _consumer = new Consumer<WorkerModel>(rabbitConfiguration);
            _consumer.OnConsumed += _consumer_OnConsumed;
        }

        private async Task _consumer_OnConsumed(WorkerModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var serviceResult =
                await _serviceCommunicator.Call<int>(
                 serviceName: _routeNameProvider.IT_AssignInventoryToWorker,
                 postData: data,
                 queryParameters: null,
                 cancellationToken: cancellationTokenSource.Token);
        }

        /// <summary>
        /// Kayıtları yakalamaya başlar
        /// </summary>
        public void StartToConsume()
        {
            _consumer.StartToConsume();
        }
    }
}
