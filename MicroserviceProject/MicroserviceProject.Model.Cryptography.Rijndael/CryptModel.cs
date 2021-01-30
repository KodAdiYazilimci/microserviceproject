namespace MicroserviceProject.Model.Cryptography.Rijndael
{
    public class CryptModel
    {
        public byte[] Encrypted { get; set; }
        public byte[] DeCrypted { get; set; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
    }
}
