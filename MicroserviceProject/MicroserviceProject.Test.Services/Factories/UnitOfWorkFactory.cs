using MicroserviceProject.Services.UnitOfWork;

namespace MicroserviceProject.Services.Business.Departments.HR.Test.Prepreations.Infrastructure
{
    public class UnitOfWorkFactory
    {
        private static IUnitOfWork unitOfWork = null;

        public static IUnitOfWork GetInstance(AppConfigurationProvider appConfigurationProvider)
        {
            if (unitOfWork == null)
            {
                unitOfWork = new UnitOfWork.UnitOfWork(appConfigurationProvider);
            }

            return unitOfWork;
        }
    }
}
