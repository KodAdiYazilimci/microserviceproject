using MicroserviceProject.Infrastructure.Communication.Moderator;
using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Services.Business.Model.Department.HR;
using MicroserviceProject.Services.Configuration.Communication.Rabbit.AA;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.MQ.AA.Util.Consumers.Inventory
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

        private readonly RouteNameProvider _routeNameProvider;
        private readonly ServiceCommunicator _serviceCommunicator;

        /// <summary>
        /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="inventoryService">Yakalanan kayıtları işleyecek envanter servisi nesnesi</param>
        public AssignInventoryToWorkerConsumer(
            AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration,
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
                 serviceName: _routeNameProvider.AA_AssignInventoryToWorker,
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
