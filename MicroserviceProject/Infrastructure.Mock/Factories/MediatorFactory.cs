using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using System;
using System.Linq;
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

        public static IMediator GetInstance(Type type)
        {
            return new Mediator(x => type);
        }

        public static TResponse GetInstance<TRequest, TResponse>(TRequest request, IRequestHandler<TRequest, TResponse> requestHandler) where TRequest : IRequest<TResponse>
        {
            Mock<IMediator> mockedMediator = new Mock<IMediator>();

            TResponse handled = requestHandler.Handle(request, new CancellationToken()).GetAwaiter().GetResult();

            return handled;
        }
    }
}
