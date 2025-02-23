using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class Form1 : Form
    {
        private string selectedFolderPath = "";
        private string lockDirectory = @"C:\\LockedFolders"; // Store encryption keys here

        public Form1()
        {
            InitializeComponent();
            Directory.CreateDirectory(lockDirectory); // Ensure lock directory exists
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = folderDialog.SelectedPath;
                    lblFolderPath.Text = selectedFolderPath;
                }
            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please select a folder and enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = txtPassword.Text;
            byte[] key = GenerateKey(password);
            string keyFilePath = Path.Combine(selectedFolderPath, "lock.key");

            try
            {
                EncryptFolder(selectedFolderPath, key);

                // Save the encryption key inside the locked folder
                File.WriteAllBytes(keyFilePath, key);

                // Copy FolderLock.exe and required runtime files inside the folder
                CopyRequiredFiles(selectedFolderPath);

              

                MessageBox.Show("Folder Locked Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error locking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please select a folder and enter the password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string keyFilePath = Path.Combine(selectedFolderPath, "lock.key");
            if (!File.Exists(keyFilePath))
            {
                MessageBox.Show("This folder is not locked!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] key = File.ReadAllBytes(keyFilePath);
            if (ValidatePassword(txtPassword.Text, key))
            {
                DecryptFolder(selectedFolderPath, key);

                // Remove lock key and executable files
                File.Delete(keyFilePath);
                DeleteRequiredFiles(selectedFolderPath);

                MessageBox.Show("Folder Unlocked Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Incorrect Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool ValidatePassword(string password, byte[] storedKey)
        {
            byte[] inputKey = GenerateKey(password);
            return StructuralComparisons.StructuralEqualityComparer.Equals(inputKey, storedKey);
        }

        private void EncryptFolder(string folderPath, byte[] key)
        {
            foreach (string file in Directory.GetFiles(folderPath))
            {
                byte[] encryptedData = EncryptFile(File.ReadAllBytes(file), key);
                File.WriteAllBytes(file + ".locked", encryptedData);
                File.Delete(file);
            }
        }

        private void DecryptFolder(string folderPath, byte[] key)
        {
            foreach (string file in Directory.GetFiles(folderPath, "*.locked"))
            {
                byte[] decryptedData = DecryptFile(File.ReadAllBytes(file), key);
                File.WriteAllBytes(file.Replace(".locked", ""), decryptedData);
                File.Delete(file);
            }
        }

        private byte[] EncryptFile(byte[] data, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                        return ms.ToArray();
                    }
                }
            }
        }

        private byte[] DecryptFile(byte[] data, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(data, 0, iv, 0, iv.Length);
                aes.IV = iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, iv.Length, data.Length - iv.Length);
                        cs.FlushFinalBlock();
                        return ms.ToArray();
                    }
                }
            }
        }

        private void CopyRequiredFiles(string folderPath)
        {
            CopyFileIfExists("FolderLock.exe", folderPath);
            CopyFileIfExists("FolderLock.runtimeconfig.json", folderPath);
            CopyFileIfExists("FolderLock.pdb", folderPath);
            CopyFileIfExists("FolderLock.dll", folderPath);
            CopyFileIfExists("FolderLock.deps.json", folderPath);
        }

        private void DeleteRequiredFiles(string folderPath)
        {
            File.Delete(Path.Combine(folderPath, "FolderLock.exe"));
            File.Delete(Path.Combine(folderPath, "FolderLock.runtimeconfig.json"));
            File.Delete(Path.Combine(folderPath, "FolderLock.dll"));
            File.Delete(Path.Combine(folderPath, "FolderLock.deps.json"));
            File.Delete(Path.Combine(folderPath, "FolderLock.pdb"));
            File.Delete(Path.Combine(folderPath, "Open.bat"));
        }

        private void CopyFileIfExists(string fileName, string destinationFolder)
        {
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string destinationFile = Path.Combine(destinationFolder, fileName);

            if (File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile, true);
            }
        }

      
    }
}
