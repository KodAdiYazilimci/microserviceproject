using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Threading;

namespace Infrastructure.Mock.Factories
{
    public class MediatorFactory
    {
        public static IMediator GetInstance(IServiceCollection services)
        {
            IMediator mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();

            return mediator;
        }

        public static TResponse GetInstance<TRequest, TResponse>(TRequest request, IRequestHandler<TRequest, TResponse> requestHandler) where TRequest : IRequest<TResponse>
        {
            TResponse handled = requestHandler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            return handled;
        }
    }
}
