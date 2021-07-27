using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    /// <summary>
    /// İş birimi arayüzünü taklit eden sınıf
    /// </summary>
    public class UnitOfWorkFactory
    {
        /// <summary>
        /// İş birimi arayüzü
        /// </summary>
        private static IUnitOfWork unitOfWork = null;

        /// <summary>
        /// İş birimi arayüzünün bir örneğini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma ayarları nesnesi</param>
        /// <returns></returns>
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
