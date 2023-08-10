using System.Security.Cryptography;
using System.Text;
using NCRFiscalManager.Core.Constants;

namespace NCRFiscalManager.Core.Utilities
{
    public class DecodeUtilities
    {
        /// <summary>
        /// Método para decoficar en base64 un texto.
        /// </summary>
        /// <param name="strEncodeBase64">Texto codificado</param>
        /// <returns>Retorna texto decodificado</returns>
        public static string DecodeBase64(string strEncodeBase64)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(strEncodeBase64);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Método para desencriptar un texto.
        /// </summary>
        /// <param name="strKey">Llave para poder desencriptar</param>
        /// <param name="strEncodeText">Texto codificado</param>
        /// <returns>Retorna texto decodificado</returns>
        public static string DecryptText(string strKey, string strEncodeText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(strEncodeText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(strKey);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método para decodificar un texto.
        /// </summary>
        /// <param name="strEncodeText">Texto a decodificar</param>
        /// <returns>Retorna texto decodificado</returns>
        public static string GetDecodeText(string strEncodeText)
        {
            return DecodeUtilities.DecryptText(DecodeUtilities.DecodeBase64(Constant.Key), DecodeUtilities.DecodeBase64(strEncodeText));
        }
    }
}