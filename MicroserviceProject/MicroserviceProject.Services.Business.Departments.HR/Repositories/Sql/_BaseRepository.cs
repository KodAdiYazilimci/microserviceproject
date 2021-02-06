using MicroserviceProject.Services.Business.Departments.HR.Util.UnitOfWork;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// Repository sınıfları için temel sınıf
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Veritabanı işlemlerini kapsayan iş birimi nesnesi
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Repository sınıfları için temel sınıf
        /// </summary>
        /// <param name="configuration">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
