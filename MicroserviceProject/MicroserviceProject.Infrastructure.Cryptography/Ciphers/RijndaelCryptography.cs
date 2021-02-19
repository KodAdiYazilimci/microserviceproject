
using MicroserviceProject.Infrastructure.Cryptography.Models;

using System.IO;
using System.Security.Cryptography;

namespace MicroserviceProject.Infrastructure.Cryptography.Ciphers
{
    /// <summary>
    /// Rijndael kriptolama işlemleri sınıfı
    /// </summary>
    public static class RijndaelCryptography
    {
        /// <summary>
        /// Kriptolama işlemini yapar
        /// </summary>
        /// <param name="value">Kriptolanacak data</param>
        /// <returns></returns>
        public static CryptModel Crypt(byte[] value)
        {
            using (System.Security.Cryptography.Rijndael myRijndael = System.Security.Cryptography.Rijndael.Create())
            {
                // Encrypt the string to an array of bytes.
                byte[] encrypted = EncryptRijndael(value, myRijndael.Key, myRijndael.IV);

                return new CryptModel
                {
                    Encrypted = encrypted,
                    Key = myRijndael.Key,
                    IV = myRijndael.IV
                };
            }
        }

        /// <summary>
        /// Kriptolanmış veriyi çözer
        /// </summary>
        /// <param name="encrypted">Kriptolanmış veri</param>
        /// <param name="key">Kriptolanmış verinin özel anahtarı</param>
        /// <param name="iv">Kriptolanmış verinin public anahtarı</param>
        /// <returns></returns>
        public static CryptModel DeCrypt(byte[] encrypted, byte[] key, byte[] iv)
        {
            // Decrypt the bytes to a string.
            byte[] decrypted = DecryptRijndael(encrypted, key, iv);

            return new CryptModel
            {
                DeCrypted = decrypted,
                Key = key,
                IV = iv
            };
        }


        // Encrypt a byte array into a byte array using a key and an IV 
        private static byte[] EncryptRijndael(byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the
            // next line with something like 
            //      TripleDES alg = TripleDES.Create(); 
            System.Security.Cryptography.Rijndael alg = System.Security.Cryptography.Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because
            // the algorithm is operating in its default 
            // mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) 
            // of the data before it is encrypted, and then each
            // encrypted block is XORed with the 
            // following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and
            // there is no more data coming in, 
            // and it is now a good time to apply the padding and
            // finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        private static byte[] DecryptRijndael(byte[] cipherData,
                                    byte[] Key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. 
            // We are going to use Rijndael because it is strong and
            // available on all platforms. 
            // You can use other algorithms, to do so substitute the next
            // line with something like 
            //     TripleDES alg = TripleDES.Create(); 
            System.Security.Cryptography.Rijndael alg = System.Security.Cryptography.Rijndael.Create();

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            alg.Key = Key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be
            // pumping our data. 
            // CryptoStreamMode.Write means that we are going to be
            // writing data to the stream 
            // and the output will be written in the MemoryStream
            // we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption
            // and there is no more data coming in, 
            // and it is now a good time to remove the padding
            // and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }
    }
}
