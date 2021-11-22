using System.Collections.Generic;

namespace Infrastructure.Communication.Http.Models
{
    /// <summary>
    /// İşlem süreci nesnesi
    /// </summary>
    public class TransactionModel
    {
        /// <summary>
        /// İşlem kimliğinin gerçekleştiği modüller
        /// </summary>
        public List<string> Modules { get; set; }

        /// <summary>
        /// Servis işlemleri boyunca geçerli olacak işlem kimliği
        /// </summary>
        public string TransactionIdentity { get; set; }
    }
}
