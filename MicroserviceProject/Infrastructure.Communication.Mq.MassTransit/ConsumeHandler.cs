using MassTransit;

namespace Infrastructure.Communication.Mq.MassTransit
{
    public abstract class ConsumeHandler<TModel> : IConsumer<TModel> where TModel : class
    {
        public abstract Task Consume(ConsumeContext<TModel> context);
    }
}
