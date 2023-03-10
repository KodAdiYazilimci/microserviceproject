using Services.Communication.Http.Broker.Abstract;

namespace Services.Communication.Http.Broker.Department.Abstract
{
    public interface IDepartmentCommunicator : ICommunicator
    {
        Task<string> GetServiceToken(CancellationTokenSource cancellationTokenSource);
    }
}
