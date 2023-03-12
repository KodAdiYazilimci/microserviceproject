using Infrastructure.Communication.Mq.Configuration;

using MassTransit;

namespace Infrastructure.Communication.Mq.MassTransit
{
    public class Consumer<TConsumer, TModel> : IConsumer, IDisposable where TConsumer : ConsumeHandler<TModel>, new() where TModel : class
    {
        private bool disposed = false;
        private IBusControl? busControl = null;
        private readonly IRabbitConfiguration _rabbitConfiguration;

        public Consumer(IRabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
        }

        public async Task StartConsumeAsync(CancellationToken cancellationToken = default)
        {
            busControl = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(_rabbitConfiguration.Host, hostConfig =>
                {
                    hostConfig.Username(_rabbitConfiguration.UserName);
                    hostConfig.Password(_rabbitConfiguration.Password);
                });

                factory.ReceiveEndpoint(_rabbitConfiguration.QueueName, config =>
                {
                    factory.ReceiveEndpoint(_rabbitConfiguration.QueueName, endpointConfig =>
                    {
                        endpointConfig.Consumer<TConsumer>();
                    });
                });
            });

            await busControl.StartAsync(cancellationToken);
        }

        public async Task StopConsumeAsync(CancellationToken cancellationToken = default)
        {
            if (busControl != null)
            {
                await busControl.StopAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    busControl = null;

                    disposed = true;
                }
            }
        }
    }
}
