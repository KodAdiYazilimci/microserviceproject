using Infrastructure.Communication.Mq.Abstraction;
using Infrastructure.Communication.Mq.Configuration;

using MassTransit;

namespace Infrastructure.Communication.Mq.MassTransit
{
    public class Publisher<TModel> : IPublisher<TModel>, IDisposable where TModel : class
    {
        private bool disposed = false;
        private IBusControl? busControl = null;
        private readonly IRabbitConfiguration _rabbitConfiguration;

        public Publisher(IRabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
        }

        public async Task PublishAsync(TModel model, CancellationTokenSource cancellationTokenSource)
        {
            busControl = Bus.Factory.CreateUsingRabbitMq(factory =>
            {
                factory.Host(_rabbitConfiguration.Host, hostConfig =>
                {
                    hostConfig.Username(_rabbitConfiguration.UserName);
                    hostConfig.Password(_rabbitConfiguration.Password);
                });
            });

            await busControl.StartAsync(cancellationTokenSource.Token);

            await busControl.Publish<TModel>(model, cancellationTokenSource.Token);

            await busControl.StopAsync(cancellationTokenSource.Token);
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
