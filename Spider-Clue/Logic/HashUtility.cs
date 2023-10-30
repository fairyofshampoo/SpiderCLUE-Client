using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Spider_Clue.Logic
{
    internal class HashUtility
    {
        private HashUtility() { }
        public static string CalculateSHA1Hash(string textToHash)
        {
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(textToHash);
                byte[] hash = sha1.ComputeHash(bytes);

                StringBuilder textToHashBuilder = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    textToHashBuilder.Append(hash[i].ToString("x2"));
                }

                return textToHashBuilder.ToString();
            }
        }
    }
}
