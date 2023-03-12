namespace Infrastructure.Communication.Mq.Abstraction
{
    public interface IBulkPublisher<TModel> where TModel : class
    {
        Task PublishAsync(List<TModel> models, CancellationTokenSource cancellationTokenSource);
    }
}
