using Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class UnitOfWorkFactory
    {
        private static IUnitOfWork unitOfWork = null;

        public static IUnitOfWork GetInstance(IConfiguration configuration)
        {
            if (unitOfWork == null)
            {
                unitOfWork = new UnitOfWork(configuration);
            }

            return unitOfWork;
        }
    }
}
