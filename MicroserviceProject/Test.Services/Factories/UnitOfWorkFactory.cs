using MicroserviceProject.Infrastructure.Transaction.UnitOfWork;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure
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
