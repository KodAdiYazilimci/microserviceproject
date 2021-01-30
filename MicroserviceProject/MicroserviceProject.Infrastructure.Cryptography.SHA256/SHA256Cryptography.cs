﻿using System.Text;

namespace MicroserviceProject.Infrastructure.Cryptography.SHA256
{
    public static class SHA256Cryptography
    {
        /// <summary>
        /// Bir metni SHA256 algoritmasına göre hashler
        /// </summary>
        /// <param name="rawData">Kriptolanacak metin</param>
        /// <returns></returns>
        public static string Crypt(string rawData)
        {
            // Create a SHA256   
            using (System.Security.Cryptography.SHA256 sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
