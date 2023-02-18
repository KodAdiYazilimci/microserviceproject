using MediatR;

using System;

namespace Infrastructure.Mock.Factories
{
    public class MediatorFactory
    {
        public static IMediator GetInstance(Type type)
        {
            return new Mediator(x =>
            {
                return type;
            });
        }
    }

}
