namespace Infrastructure.Cryptography.Models
{
    /// <summary>
    /// Rijndael kriptolama modeli
    /// </summary>
    public class CryptModel
    {
        /// <summary>
        /// Kriptolanmış veri
        /// </summary>
        public byte[] Encrypted { get; set; }

        /// <summary>
        /// Çözülmüş veri
        /// </summary>
        public byte[] DeCrypted { get; set; }

        /// <summary>
        /// Özel anahtar
        /// </summary>
        public byte[] Key { get; set; }

        /// <summary>
        /// Ortak anahtar
        /// </summary>
        public byte[] IV { get; set; }
    }
}
