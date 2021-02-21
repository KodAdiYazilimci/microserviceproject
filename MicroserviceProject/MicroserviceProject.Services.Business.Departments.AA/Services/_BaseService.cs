
using System;

namespace MicroserviceProject.Services.Business.Departments.AA.Services
{
    /// <summary>
    /// Servis sınıflarının temeli
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// İşlem kimliği
        /// </summary>
        private string transactionIdentity;

        /// <summary>
        /// Servislerin işlem süreçleri boyunca izleyeceği işlem kimliği
        /// </summary>
        public string TransactionIdentity
        {
            get
            {
                if (string.IsNullOrEmpty(transactionIdentity))
                {
                    transactionIdentity = Guid.NewGuid().ToString();
                }

                return transactionIdentity;
            }
            set
            {
                transactionIdentity = value;
            }
        }
    }
}
