namespace Infrastructure.Communication.Mq.Abstraction
{
    public interface IConsumer
    {
        Task StartConsumeAsync(CancellationTokenSource cancellationTokenSource);
        Task StopConsumeAsync(CancellationTokenSource cancellationTokenSource);
    }
}
