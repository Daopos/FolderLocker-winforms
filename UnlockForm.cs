﻿using System;
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
    public partial class UnlockForm: Form
    {
        private string selectedFolderPath = "";
        private string lockDirectory = @"C:\\LockedFolders"; // Store encryption keys here
        public UnlockForm()
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

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            selectedFolderPath = lblFolderPath.Text; // Update selectedFolderPath from the textbox

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


        private void DecryptFolder(string folderPath, byte[] key)
        {
            foreach (string file in Directory.GetFiles(folderPath, "*.locked"))
            {
                byte[] decryptedData = DecryptFile(File.ReadAllBytes(file), key);
                File.WriteAllBytes(file.Replace(".locked", ""), decryptedData);
                File.Delete(file);
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

        private void DeleteRequiredFiles(string folderPath)
        {
            File.Delete(Path.Combine(folderPath, "FolderUnlocker.exe"));
            File.Delete(Path.Combine(folderPath, "FolderUnlocker.runtimeconfig.json"));
            File.Delete(Path.Combine(folderPath, "FolderUnlocker.dll"));
            File.Delete(Path.Combine(folderPath, "FolderUnlocker.deps.json"));
            File.Delete(Path.Combine(folderPath, "FolderUnlocker.pdb"));
            File.Delete(Path.Combine(folderPath, "Open.bat"));
        }

    }
}
