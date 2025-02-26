using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class Lock: Form
    {
        private string selectedFolderPath = "";
        public Lock()
        {
            InitializeComponent();
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

            if (txtPassword.Text.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters long!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match! Please re-enter the password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string keyFilePath = Path.Combine(selectedFolderPath, "lock.key");

            // Check if the folder is already locked
            if (File.Exists(keyFilePath) || Directory.GetFiles(selectedFolderPath, "*.locked").Length > 0)
            {
                MessageBox.Show("This folder is already locked!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string password = txtPassword.Text;
            byte[] key = GenerateKey(password);

            try
            {
                EncryptFolder(selectedFolderPath, key);

                // Save the encryption key inside the locked folder
                File.WriteAllBytes(keyFilePath, key);

                // Make the lock key file hidden
                File.SetAttributes(keyFilePath, FileAttributes.Hidden);

                // Copy FolderUnlocker.exe and required runtime files inside the folder
                CopyRequiredFiles(selectedFolderPath);

                MessageBox.Show("Folder Locked Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error locking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
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

        private void CopyRequiredFiles(string folderPath)
        {
            CopyAndHideFile("FolderUnlocker.exe", folderPath);
            CopyAndHideFile("FolderUnlocker.runtimeconfig.json", folderPath);
            CopyAndHideFile("FolderUnlocker.pdb", folderPath);
            CopyAndHideFile("FolderUnlocker.dll", folderPath);
            CopyAndHideFile("FolderUnlocker.deps.json", folderPath);
            CreateUnlockBatchFile(folderPath);
        }

        private void CopyAndHideFile(string fileName, string destinationFolder)
        {
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string destinationFile = Path.Combine(destinationFolder, fileName);

            if (File.Exists(sourceFile))
            {
                File.Copy(sourceFile, destinationFile, true);
                File.SetAttributes(destinationFile, FileAttributes.Hidden);
            }
        }

        private void CreateUnlockBatchFile(string folderPath)
        {
            string batchFilePath = Path.Combine(folderPath, "_Open this to unlock the files.bat");
            string batchContent = "@echo off\n" +
                                  "cd /d \"%~dp0\"\n" +
                                  "start \"\" \"FolderUnlocker.exe\"\n" +
                                  "exit";

            File.WriteAllText(batchFilePath, batchContent, Encoding.UTF8);
        }

    
    }
}
