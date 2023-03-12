namespace Infrastructure.Communication.Mq.Abstraction
{
    public interface IPublisher<TModel> where TModel : class
    {
        Task PublishAsync(TModel model, CancellationTokenSource cancellationTokenSource);
    }
}
