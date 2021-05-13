using System.Collections.Generic;

namespace Presentation.UI.WindowsForm.Infrastructure.Communication.Model
{
    /// <summary>
    /// İşlem süreci nesnesi
    /// </summary>
    public class Transaction
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
