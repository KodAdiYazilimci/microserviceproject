using AutoMapper;

using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Services.Business.Configuration.Communication.Rabbit.AA;
using MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql;
using MicroserviceProject.Services.Business.Departments.AA.Services;
using MicroserviceProject.Services.Business.Departments.AA.Util.Consumers.Inventory;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MicroserviceProject.Services.Business.Departments.AA.DI
{
    /// <summary>
    /// Rabbit kuyruk tüketici sınıfların DI sınıfı
    /// </summary>
    public static class ConsumerConfiguration
    {
        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="hostBuilder">Hosting nesnesi</param>
        /// <returns></returns>
        public static IHost RegisterConsumers(this IHost hostBuilder)
        {

            IConfiguration configuration =
                (IConfiguration)hostBuilder.Services.GetService(typeof(IConfiguration));

            AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration =
                (AssignInventoryToWorkerRabbitConfiguration)hostBuilder.Services.GetService(typeof(AssignInventoryToWorkerRabbitConfiguration));

            IMapper mapper = (IMapper)hostBuilder.Services.GetService(typeof(IMapper));

            CacheDataProvider cacheDataProvider =
                (CacheDataProvider)hostBuilder.Services.GetService(typeof(CacheDataProvider));

            IUnitOfWork unitOfWork = new UnitOfWork(configuration);

            InventoryRepository inventoryRepository = new InventoryRepository(unitOfWork);

            WorkerInventoryRepository workerInventoryRepository = new WorkerInventoryRepository(unitOfWork);

            InventoryService inventoryService =
                new InventoryService(mapper, unitOfWork, cacheDataProvider, inventoryRepository, workerInventoryRepository);

            AssignInventoryToWorkerConsumer assignInventoryToWorkerConsumer =
                new AssignInventoryToWorkerConsumer(rabbitConfiguration, inventoryService);

            assignInventoryToWorkerConsumer.StartToConsume();


            return hostBuilder;
        }
    }
}
