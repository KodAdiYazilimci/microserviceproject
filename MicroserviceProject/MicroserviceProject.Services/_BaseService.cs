
using System;

namespace MicroserviceProject.Services
{
    /// <summary>
    /// Servis sınıflarının temeli
    /// </summary>
    public abstract class BaseService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        protected bool Disposed = false;

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

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public abstract string ServiceName { get; }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Disposed)
                {
                    TransactionIdentity = string.Empty;
                }

                Disposed = true;
            }
        }
    }
}
