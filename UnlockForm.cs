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
    public partial class UnlockForm: Form
    {
        private string selectedFolderPath = "";
        public UnlockForm()
        {

            InitializeComponent();
        }
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtPassword.UseSystemPasswordChar ? Properties.Resources.eye : Properties.Resources.hidden;
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
            selectedFolderPath = lblFolderPath.Text;
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

            try
            {
                buttonUnlock.Enabled = false;
                lblStatus.Text = "Processing...";
                lblStatus.Visible = false;
                progressBarLoading.Visible = true;

                byte[] storedKey = File.ReadAllBytes(keyFilePath);
                byte[] inputKey = GenerateKey(txtPassword.Text);

                if (storedKey.SequenceEqual(inputKey))
                {
                    RestoreFolderAttributes(selectedFolderPath);
                    DecryptFolder(selectedFolderPath, inputKey);
                    File.Delete(keyFilePath);
                    DeleteRequiredFiles(selectedFolderPath);

                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = "Status: Folder unlocked successfully!";
                    MessageBox.Show("Folder Unlocked Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Status: Incorrect Password!";
                    MessageBox.Show("Incorrect Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Status: Error occurred!";
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarLoading.Visible = false;
                buttonUnlock.Enabled = true;
            }
        }

        private static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
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

        private byte[] DecryptFile(byte[] data, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(data, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(data, iv.Length, data.Length - iv.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        private void DeleteRequiredFiles(string folderPath)
        {
            string[] filesToDelete =
            {
                "FolderUnlocker.exe",
                "FolderUnlocker.runtimeconfig.json",
                "FolderUnlocker.dll",
                "FolderUnlocker.deps.json",
                "FolderUnlocker.pdb",
                "_Open this to unlock the files.bat",
                "desktop.ini",
                "lock.ico",
                "recovery.dat",
                "recoveryemail.dat",
            };

            foreach (string file in filesToDelete)
            {
                string filePath = Path.Combine(folderPath, file);
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }

        private void RestoreFolderAttributes(string folderPath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                di.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.System);

                string desktopIniPath = Path.Combine(folderPath, "desktop.ini");
                if (File.Exists(desktopIniPath))
                {
                    File.SetAttributes(desktopIniPath, FileAttributes.Normal);
                    File.Delete(desktopIniPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error restoring folder attributes: " + ex.Message);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            selectedFolderPath = lblFolderPath.Text;
            if (string.IsNullOrEmpty(selectedFolderPath) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please select a folder and enter the current password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string keyFilePath = Path.Combine(selectedFolderPath, "lock.key");
            if (!File.Exists(keyFilePath))
            {
                MessageBox.Show("This folder is not locked!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                File.SetAttributes(keyFilePath, FileAttributes.Normal); // Remove ReadOnly/System attribute

                byte[] storedKey = File.ReadAllBytes(keyFilePath); // Read current key
                byte[] inputKey = GenerateKey(txtPassword.Text);

                if (storedKey.SequenceEqual(inputKey))
                {
                    using (ChangePasswordForm changePasswordForm = new ChangePasswordForm())
                    {
                        if (changePasswordForm.ShowDialog() == DialogResult.OK)
                        {
                            string newPassword = changePasswordForm.NewPassword;

                            // Generate new key from the new password
                            byte[] newKey = GenerateKey(newPassword);

                            // Decrypt all files with old key
                            DecryptFolder(selectedFolderPath, inputKey);

                            // Save the new key
                            File.WriteAllBytes(keyFilePath, newKey);

                            // Update recovery.dat with new password
                            string recoveryFilePath = Path.Combine(selectedFolderPath, "recovery.dat");

                            if (File.Exists(recoveryFilePath))
                            {
                                try
                                {
                                    // Remove hidden and system attributes before modifying
                                    File.SetAttributes(recoveryFilePath, FileAttributes.Normal);

                                    // Read the existing recovery email
                                    string[] recoveryData = File.ReadAllLines(recoveryFilePath);
                                    string recoveryEmail = recoveryData.Length > 0 ? recoveryData[0] : "unknown@example.com"; // Fallback if empty

                                    // Encrypt the new password
                                    string encryptedNewPassword = EncryptData(newPassword, "your_secret_key");

                                    // Save updated recovery data
                                    string updatedRecoveryData = $"{recoveryEmail}\n{encryptedNewPassword}";
                                    File.WriteAllText(recoveryFilePath, updatedRecoveryData);

                                    // Reapply hidden and system attributes
                                    File.SetAttributes(recoveryFilePath, FileAttributes.Hidden | FileAttributes.System);
                                }
                                catch (Exception ex)
                                {
                                    File.SetAttributes(recoveryFilePath, FileAttributes.Hidden | FileAttributes.System);
                                    MessageBox.Show($"Error updating recovery file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }



                            // Re-encrypt the files with the new key
                            // List of essential files that should not be encrypted
                            HashSet<string> excludedFiles = new HashSet<string>
{
    "FolderUnlocker.exe",
    "FolderUnlocker.runtimeconfig.json",
    "FolderUnlocker.dll",
    "FolderUnlocker.deps.json",
    "FolderUnlocker.pdb",
    "_Open this to unlock the files.bat",
    "desktop.ini",
    "lock.ico",
    "recoveryemail.dat",
    "recovery.dat",
    "lock.key" // Ensure the lock key is not mistakenly encrypted
};

                            foreach (string file in Directory.GetFiles(selectedFolderPath))
                            {
                                string fileName = Path.GetFileName(file);

                                // Skip encryption for essential files
                                if (!excludedFiles.Contains(fileName) && !file.EndsWith(".locked"))
                                {
                                    byte[] fileData = File.ReadAllBytes(file);
                                    byte[] encryptedData = EncryptFile(fileData, newKey);
                                    File.WriteAllBytes(file + ".locked", encryptedData);
                                    File.Delete(file);
                                }
                            }

                            // Hide the key file again after writing
                            File.SetAttributes(keyFilePath, FileAttributes.Hidden | FileAttributes.System);

                            lblStatus.Visible = true;
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Status: Password changed successfully!";
                            MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    File.SetAttributes(keyFilePath, FileAttributes.Hidden); // Remove ReadOnly/System attribute
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Status: Incorrect Password!";
                    MessageBox.Show("Incorrect Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                File.SetAttributes(keyFilePath, FileAttributes.Hidden); // Remove ReadOnly/System attribute
                lblStatus.Visible = true;
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Status: Error occurred!";
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string EncryptData(string text, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                aes.IV = new byte[16]; // Default IV (all zeros)

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(text);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        private byte[] EncryptFile(byte[] data, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // IV is necessary for AES

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                    return ms.ToArray();
                }
            }
        }



    }
}
