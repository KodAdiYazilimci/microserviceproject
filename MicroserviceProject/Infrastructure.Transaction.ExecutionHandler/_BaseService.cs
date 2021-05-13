
using System;
using System.Globalization;

namespace Infrastructure.Transaction.ExecutionHandler
{
    /// <summary>
    /// Servis sınıflarının temeli
    /// </summary>
    public abstract class BaseService : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public abstract string ApiServiceName { get; }

        /// <summary>
        /// Servisin çalışacağı bölgesen ayar
        /// </summary>
        public string Region { get; set; }

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
                if (!disposed)
                {
                    TransactionIdentity = string.Empty;
                }

                disposed = true;
            }
        }
    }
}
