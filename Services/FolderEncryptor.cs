using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UCUFolderLocker.Services
{
    public static class FolderEncryptor
    {
        public static void EncryptFolder(string folderPath, string password)
        {
            foreach (var file in Directory.GetFiles(folderPath))
            {
                if (file.EndsWith("lock.info")) continue;

                byte[] fileBytes = File.ReadAllBytes(file);
                byte[] encryptedBytes = Encrypt(fileBytes, password);
                File.WriteAllBytes(file, encryptedBytes);
            }
        }

        public static void DecryptFolder(string folderPath, string password)
        {
            foreach (var file in Directory.GetFiles(folderPath))
            {
                if (file.EndsWith("lock.info")) continue;

                byte[] fileBytes = File.ReadAllBytes(file);
                byte[] decryptedBytes = Decrypt(fileBytes, password);
                File.WriteAllBytes(file, decryptedBytes);
            }
        }

        private static byte[] Encrypt(byte[] data, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = GenerateKey(password, aes.KeySize / 8);
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(key, iv))
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        private static byte[] Decrypt(byte[] data, string password)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] key = GenerateKey(password, aes.KeySize / 8);
                byte[] iv = new byte[aes.IV.Length];
                Array.Copy(data, iv, iv.Length);

                using (var decryptor = aes.CreateDecryptor(key, iv))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, iv.Length, data.Length - iv.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        private static byte[] GenerateKey(string password, int keySize)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SALT"), 1000))
            {
                return deriveBytes.GetBytes(keySize);
            }
        }
    }
}
