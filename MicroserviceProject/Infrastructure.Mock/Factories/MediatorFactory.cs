using MediatR;

using System;

namespace Infrastructure.Mock.Factories
{
    public class MediatorFactory
    {
        private static IMediator mediator;

        public static IMediator GetInstance(Type type)
        {
            if (mediator == null)
            {
                mediator = new Mediator(x =>
                {
                    return type;
                });
            }
            return mediator;
        }
    }

}
