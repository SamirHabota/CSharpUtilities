using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CSharpUtilities.Security.Cryptography
{
    public class Cryptography
    {
        #region SaltAndHashGeneration

        /// <summary>
        /// Will generate a unique and random salt.
        /// </summary>
        /// <returns>base64 string - generated salt</returns>
        public static string GenerateSalt()
        {
            var buffer = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Will generate a hash, based on a unique salt and password.
        /// </summary>
        /// <param name="salt" type="string">the encoding salt</param>
        /// <param name="password" type="string">password for hash generation</param>
        /// <returns>base64 string - genearted hash</returns>
        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
        #endregion

        #region ApiPasswordDecryptor
        #region PrivateMethods
        private static string[] ConvertToArray(string content)
        {
            var stringArray = content.Trim('[', ']')
                  .Split(",")
                  .Select(x => x.Trim('"'))
                  .ToArray();
            if (!content.Contains("[") || !content.Contains("]") || stringArray.Length <= 1) return new string[] { "1", "1" };
            return stringArray;
        }

        private static string TrimKey(string key)
        {
            return key.Split("-")[0];
        }

        private static List<int> GetAsciiArray(string[] codesArray, string key)
        {
            List<int> resultAsciiArray = new List<int>();
            for (int i = 0; i < codesArray.Length; i++)
                resultAsciiArray.Add(Convert.ToInt32(codesArray[i]) - ((int)key[i % key.Length]));
            return resultAsciiArray;
        }

        private static string CreatePassword(List<int> resultAsciiArray)
        {
            string plainPassword = "";
            for (int i = 0; i < resultAsciiArray.Count; i++) plainPassword += (char)resultAsciiArray[i];
            return plainPassword;
        }
        #endregion

        /// <summary>
        /// Passwords should not travel in plain text format, from the client to the server.
        /// This method will decrypt the password on the server.
        /// The encryption process is noted below, inside the Javascript section
        /// </summary>
        /// <param name="encryptedPassword" type="string">the encrypted password</param>
        /// <param name="key" type="string">the unique key</param>
        /// <returns>string - decrypted password</returns>
        public static string Decrypt(string encryptedPassword, string key)
        {
            return CreatePassword(GetAsciiArray(ConvertToArray(encryptedPassword), TrimKey(key)));
        }
        #endregion

        #region JavascriptEncyption
        /*
         function encrypt(plainPassword, key) {
              key = key.split("-")[0];
              var asciiArray = [];
              for (var i = 0; i < plainPassword.length; i++) {
                asciiArray.push(
                  plainPassword.charCodeAt(i) + key.charCodeAt(i % key.length)
                );
              }
              return JSON.stringify(asciiArray);
        }         
         */
        #endregion

    }
}
