namespace Infrastructure.Runtime.Timing
{
    /// <summary>
    /// Çalışma zamanı sırasını uyarlayan arayüz
    /// </summary>
    public interface IExecutionTime
    {
        /// <summary>
        /// Çalışma zamanı sırası
        /// </summary>
        ExecutionType ExecutionType { get; }
    }
}
