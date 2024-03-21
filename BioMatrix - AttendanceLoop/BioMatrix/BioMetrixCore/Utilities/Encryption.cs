using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Utilities
{
    class Encryption
    {
        //string hash = "f0xle@rn";//Create a hash key
        public static string Encrypt(string text)
        {
            string hash = "f0xle@rn";//Create a hash key
            string encryptedText;
            byte[] data = UTF8Encoding.UTF8.GetBytes(text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));//Get hash key
                //Encrypt data by hash key
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    encryptedText = Convert.ToBase64String(results, 0, results.Length);
                }
            }
            return encryptedText;
        }

        public static string Decrypt(string encryptedText)
        {
            string hash = "f0xle@rn";//Create a hash key
            string decryptedText;
            //Convert a string to byte array
            byte[] data = Convert.FromBase64String(encryptedText);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));//Get hash key
                                                                                //Decrypt data by hash key
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    decryptedText = UTF8Encoding.UTF8.GetString(results);
                }
            }
            return decryptedText;
        }
    }
}
