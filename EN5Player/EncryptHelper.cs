using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EN5Player
{
    internal static class EncryptHelper
    {
        public static string Encrypt(string source)
        {
            return EncryptDes(source, DesKey, DesIv);
        }

        public static string Decrypt(string source)
        {
            return DecryptDes(source, DesKey, DesIv);
        }

        private static byte[] GetDesKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length > 8)
            {
                key = key.Substring(0, 8);
            }
            if (key.Length < 8)
            {
                key = key.PadRight(8, '0');
            }
            return Encoding.UTF8.GetBytes(key);
        }

        private static string EncryptDes(string source, string key, byte[] iv)
        {
            using (var desProvider = new DESCryptoServiceProvider())
            {
                byte[] rgbKeys = GetDesKey(key), rgbIvs = iv, inputByteArray = Encoding.UTF8.GetBytes(source);
                using (var memoryStream = new MemoryStream())
                {
                    var transform = desProvider.CreateEncryptor(rgbKeys, rgbIvs);
                    using (var stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        stream.Write(inputByteArray, 0, inputByteArray.Length);
                        stream.FlushFinalBlock();

                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        private static string DecryptDes(string source, string key, byte[] iv)
        {
            using (var desProvider = new DESCryptoServiceProvider())
            {
                byte[] rgbKeys = GetDesKey(key), rgbIvs = iv, inputByteArray = Convert.FromBase64String(source);
                using (var memoryStream = new MemoryStream())
                {
                    var transform = desProvider.CreateDecryptor(rgbKeys, rgbIvs);
                    using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                        cryptoStream.FlushFinalBlock();

                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }

        private static byte[] DesIv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private const string DesKey = "WhoAreU?";
    }
}