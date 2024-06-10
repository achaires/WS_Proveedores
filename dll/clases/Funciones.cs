using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace dll.clases
{
    public class Funciones
    {

        #region Encriptar
        public string Encriptar(string textoQueEncriptaremos)
        {
            return Encriptar(textoQueEncriptaremos,
              "GIS17d09/@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);
        }
        private string Encriptar(string textoQueEncriptaremos,
           string passBase, string saltValue, string hashAlgorithm,
           int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,
              initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor,
             CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }
        #endregion

        #region Desencriptar
        public string Desencriptar(string textoEncriptado)
        {
            try
            {
                return Desencriptar(textoEncriptado, "GIS17d09/@avz10", "s@lAvz", "MD5",
             1, "@1B2c3D4e5F6g7H8", 128);
            }
            catch (Exception)
            {
                return "";
            }

        }
        private string Desencriptar(string textoEncriptado, string passBase,
          string saltValue, string hashAlgorithm, int passwordIterations,
          string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,
              saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,
              initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor,
              CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0,
              plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0,
              decryptedByteCount);
            return plainText;
        }
        #endregion

        public void addError(int idFuente, String msgError, ref StringBuilder sb_Proceso)
        {

            try
            {
                sb_Proceso.AppendLine(idFuente.ToString() + "|" + msgError.Replace("|", "").Replace("~", "") + "~");
            }
            catch (Exception)
            {

            }

        }

        public Stream GenerateStreamFromString(string strData)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream,System.Text.Encoding.UTF8);
                writer.Write(strData);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
            catch (Exception)
            {
                stream = null;
            }
            return stream;
        }

        public byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = null;
            try
            {
                stream.Position = 0;
                buffer = new byte[stream.Length];
                for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
                    totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);

            }
            catch (Exception)
            {

            }
            return buffer;
        }

        /// <summary>Recibe una fecha con formato yyyyMMddHHmmss y la transforma en yyyy-MM-ddTHH:mm:ss
        /// </summary>
        /// <param name="fechaEmision"></param>
        /// <returns></returns>
        public String getPrettyDateFormat(String fechaEmision)
        {

            String valReturn = null;

            try
            {

                if (fechaEmision.Length >= 8)
                {
                    valReturn = fechaEmision.Substring(0, 4).ToString() + "-" + fechaEmision.Substring(4, 2).ToString() + "-" + fechaEmision.Substring(6, 2).ToString() + "T";
                }

                if (fechaEmision.Length >= 10)
                {
                    valReturn = valReturn + fechaEmision.Substring(8, 2).ToString() + ":";
                }
                else
                {
                    valReturn = valReturn + "00" + ":";
                }

                if (fechaEmision.Length >= 12)
                {
                    valReturn = valReturn + fechaEmision.Substring(10, 2).ToString() + ":";
                }
                else
                {
                    valReturn = valReturn + "00" + ":";
                }

                if (fechaEmision.Length >= 4)
                {
                    valReturn = valReturn + fechaEmision.Substring(12, 2).ToString();
                }
                else
                {
                    valReturn = valReturn + "00";
                }


            }
            catch (Exception)
            {
            }

            return valReturn;
        }

        public Boolean validaExpRegEx(String texto, String RegEx)
        {
            Boolean boolProcess = true;

            try
            {
                Regex rgx = new Regex(RegEx);

                if (!rgx.IsMatch(texto))
                {
                    boolProcess = false;
                }
            }
            catch (Exception)
            {
                boolProcess = false;
            }

            return boolProcess;

        }

    }
}
