using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class UnlockForm : Form
    {
        private string selectedFolderPath = "";

        public UnlockForm()
        {
            InitializeComponent();
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Modify the OpenFileDialog to show all files
                openFileDialog.Filter = "Locked Files (*.lock)|*.lock";
                openFileDialog.ValidateNames = false;
                openFileDialog.CheckFileExists = false;
                openFileDialog.ReadOnlyChecked = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = openFileDialog.FileName;

                    // Try to temporarily adjust permissions just to verify the file exists
                    try
                    {
                        // First check if the file exists using File.Exists which may work even with restricted permissions
                        if (File.Exists(selectedPath))
                        {
                            lblFolderPath.Text = selectedPath;
                        }
                        else
                        {
                            MessageBox.Show("Selected file does not exist or cannot be accessed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Even if we got an access exception, still set the path
                        // We'll deal with permissions when unlocking
                        lblFolderPath.Text = selectedPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error selecting file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnUnlock_Click(object sender, EventArgs e)
        {
            string lockedFilePath = lblFolderPath.Text;
            if (string.IsNullOrEmpty(lockedFilePath))
            {
                MessageBox.Show("Please select a locked file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Unlock failed: No file selected.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter the password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Unlock failed: No password entered.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }


            progressBarLoading.Visible = true;

            // Check if NTFS protection exists before removing it
            bool hadNTFSProtection = CheckIfHasNTFSProtection(lockedFilePath);

            if (hadNTFSProtection)
            {
                RemoveNTFSProtection(lockedFilePath);
            }

            string originalFolderName = Path.GetFileNameWithoutExtension(lockedFilePath);
            string destinationFolder = Path.Combine(Path.GetDirectoryName(lockedFilePath), originalFolderName);
            try
            {
                byte[] key = GenerateKey(txtPassword.Text);
                DecryptAndExtractFolder(lockedFilePath, destinationFolder, key);
                // Delete the locked file after successful extraction
                File.Delete(lockedFilePath);

                lblStatus.Text = "Folder unlocked successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;

                txtPassword.Clear();
                lblFolderPath.Clear();

                MessageBox.Show("Folder unlocked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (CryptographicException)
            {
                // Only reapply protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(lockedFilePath);
                }
                lblStatus.Text = "Wrong password entered.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;

                MessageBox.Show("Wrong password entered. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Only reapply protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(lockedFilePath);
                }
                lblStatus.Text = "Wrong password entered.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                MessageBox.Show("Error unlocking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarLoading.Visible = false;
            }
        }

        private void DecryptAndExtractFolder(string encryptedFilePath, string destinationFolder, byte[] key)
        {
            // Create a temporary file to work with
            string tempFilePath = Path.GetTempFileName();

            try
            {
                // Read the original encrypted file, excluding the recovery information
                byte[] fileData = File.ReadAllBytes(encryptedFilePath);

                // The recovery information is 32 bytes (IV + encrypted password) at the end
                // We need to remove it before decryption
                int recoveryInfoSize = 32; // Adjust this if you changed the size
                int actualDataSize = fileData.Length;

                // Check if we might have recovery info appended
                if (fileData.Length > recoveryInfoSize)
                {
                    // Remove potential recovery info - write only the encrypted folder data
                    actualDataSize = fileData.Length - recoveryInfoSize;
                    File.WriteAllBytes(tempFilePath, fileData.Take(actualDataSize).ToArray());
                }
                else
                {
                    // No recovery info, use the original file
                    File.Copy(encryptedFilePath, tempFilePath, true);
                }

                // Now decrypt the cleaned file
                using (FileStream fsInput = new FileStream(tempFilePath, FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        byte[] iv = new byte[aes.IV.Length];

                        // Read IV from the encrypted file
                        fsInput.Read(iv, 0, iv.Length);
                        aes.IV = iv;

                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (ZipArchive zipArchive = new ZipArchive(csDecrypt, ZipArchiveMode.Read))
                        {
                            zipArchive.ExtractToDirectory(destinationFolder);
                        }
                    }
                }

                // Delete the locked file after successful extraction if requested
                File.Delete(encryptedFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Decryption failed: " + ex.Message, ex);
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        private static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string lockedFilePath = lblFolderPath.Text;
            if (string.IsNullOrEmpty(lockedFilePath))
            {
                MessageBox.Show("Please select a locked file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Password change failed: No file selected.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            string oldPassword = txtPassword.Text;
            if (string.IsNullOrEmpty(oldPassword))
            {
                MessageBox.Show("Please enter the current password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Password change failed: No current password entered.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            progressBarLoading.Visible = true;
            bool hadNTFSProtection = CheckIfHasNTFSProtection(lockedFilePath);

            if (hadNTFSProtection)
            {
                RemoveNTFSProtection(lockedFilePath);
            }

            try
            {
                byte[] key = GenerateKey(oldPassword);
                if (IsPasswordValid(lockedFilePath, key))
                {
                    using (ChangePasswordForm changePasswordForm = new ChangePasswordForm())
                    {
                        if (changePasswordForm.ShowDialog() == DialogResult.OK)
                        {
                            string newPassword = changePasswordForm.NewPassword;

                            // Ensure password is at least 3 characters long
                            if (newPassword.Length < 3)
                            {
                                MessageBox.Show("New password must be at least 3 characters long!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                lblStatus.Text = "Password change failed: Password too short.";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                lblStatus.Visible = true;

                                // Reapply NTFS protection if it was originally there
                                if (hadNTFSProtection)
                                {
                                    ApplyNTFSProtection(lockedFilePath);
                                }
                                return;
                            }

                            try
                            {
                                ChangePassword(lockedFilePath, oldPassword, newPassword);
                                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                lblStatus.Text = "Password changed successfully.";
                                lblStatus.ForeColor = System.Drawing.Color.Green;
                                lblStatus.Visible = true;

                                txtPassword.Clear();
                                lblFolderPath.Clear();

                                // Reapply NTFS protection after successful password change
                                if (hadNTFSProtection)
                                {
                                    ApplyNTFSProtection(lockedFilePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (hadNTFSProtection)
                                {
                                    ApplyNTFSProtection(lockedFilePath);
                                }
                                MessageBox.Show("Error changing password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                lblStatus.Text = "Error changing password.";
                                lblStatus.ForeColor = System.Drawing.Color.Red;
                                lblStatus.Visible = true;
                            }
                        }
                        else
                        {
                            if (hadNTFSProtection)
                            {
                                ApplyNTFSProtection(lockedFilePath);
                            }
                            lblStatus.Text = "Password change canceled.";
                            lblStatus.ForeColor = System.Drawing.Color.Orange;
                            lblStatus.Visible = true;
                        }
                    }
                }
                else
                {
                    if (hadNTFSProtection)
                    {
                        ApplyNTFSProtection(lockedFilePath);
                    }
                    lblStatus.Text = "Incorrect password!";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Visible = true;
                    MessageBox.Show("Incorrect password! Please enter the correct password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(lockedFilePath);
                }
                lblStatus.Text = "Error validating password.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                MessageBox.Show("Error validating password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarLoading.Visible = false;
            }
        }


        private bool IsPasswordValid(string encryptedFilePath, byte[] key)
        {
            // Create a temporary file to work with
            string tempFilePath = Path.GetTempFileName();

            try
            {
                // Read the original encrypted file, excluding the recovery information
                byte[] fileData = File.ReadAllBytes(encryptedFilePath);

                // The recovery information is 32 bytes at the end
                int recoveryInfoSize = 32; // Adjust this if you changed the size

                // Check if we might have recovery info appended
                if (fileData.Length > recoveryInfoSize)
                {
                    // Remove potential recovery info - write only the encrypted folder data
                    int actualDataSize = fileData.Length - recoveryInfoSize;
                    File.WriteAllBytes(tempFilePath, fileData.Take(actualDataSize).ToArray());
                }
                else
                {
                    // No recovery info, use the original file
                    File.Copy(encryptedFilePath, tempFilePath, true);
                }

                using (FileStream fsInput = new FileStream(tempFilePath, FileMode.Open))
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        byte[] iv = new byte[aes.IV.Length];

                        // Read IV from the encrypted file
                        if (fsInput.Length < iv.Length)
                        {
                            return false; // File is too small to be valid
                        }

                        fsInput.Read(iv, 0, iv.Length);
                        aes.IV = iv;

                        // Try to read the zip header to verify if decryption works correctly
                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            try
                            {
                                // Zip files start with PK header (0x504B)
                                byte[] header = new byte[4];
                                int bytesRead = csDecrypt.Read(header, 0, 4);

                                // Check if we read the expected number of bytes and if they start with PK
                                return bytesRead == 4 && header[0] == 0x50 && header[1] == 0x4B;
                            }
                            catch
                            {
                                return false; // If we can't decrypt, password is invalid
                            }
                        }
                    }
                }
            }
            catch
            {
                return false; // Any exception means validation failed
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
        private void ChangePassword(string lockedFilePath, string oldPassword, string newPassword)
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(lockedFilePath));
            string tempFile = Path.GetTempFileName();

            try
            {
                // Clear any existing temp folder
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder, true);

                Directory.CreateDirectory(tempFolder);

                // Get recovery email if it exists
                string recoveryEmail = "";
                string recoveryEmailFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "UCUFolderLocker",
                    "recovery_email.txt");

                if (File.Exists(recoveryEmailFilePath))
                {
                    recoveryEmail = File.ReadAllText(recoveryEmailFilePath);
                }

                // Decrypt with old password
                byte[] oldKey = GenerateKey(oldPassword);
                DecryptAndExtractFolder(lockedFilePath, tempFolder, oldKey);

                // Create a new lock file (don't overwrite the original yet)
                byte[] newKey = GenerateKey(newPassword);
                EncryptFolder(tempFolder, tempFile, newKey);

                // If we have recovery info, add it back
                if (!string.IsNullOrEmpty(recoveryEmail))
                {
                    AppendRecoveryInfo(tempFile, newPassword, recoveryEmail);
                }

                // Replace the original file with our new one
                if (File.Exists(lockedFilePath))
                    File.Delete(lockedFilePath);

                File.Move(tempFile, lockedFilePath);

                // Cleanup
                Directory.Delete(tempFolder, true);
            }
            catch (Exception ex)
            {
                // Clean up any temp files
                if (File.Exists(tempFile))
                    File.Delete(tempFile);

                throw new Exception("Failed to change password: " + ex.Message);
            }
        }

        // Add this method to append recovery information
        private void AppendRecoveryInfo(string lockFilePath, string password, string recoveryEmail)
        {
            try
            {
                // Encrypt the password using the recovery email as the key
                byte[] emailKey = GenerateKey(recoveryEmail);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = emailKey;
                    aes.GenerateIV();

                    byte[] iv = aes.IV;
                    byte[] encryptedPassword;

                    // Encrypt the password
                    using (MemoryStream msEncrypt = new MemoryStream())
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(password);
                        swEncrypt.Flush();
                        csEncrypt.FlushFinalBlock();
                        encryptedPassword = msEncrypt.ToArray();
                    }

                    // Write IV + encrypted password to the end of the file
                    using (FileStream fs = new FileStream(lockFilePath, FileMode.Append))
                    {
                        fs.Write(iv, 0, iv.Length);
                        fs.Write(encryptedPassword, 0, encryptedPassword.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add recovery information: " + ex.Message);
            }
        }

        private void EncryptFolder(string sourceFolder, string destinationFile, byte[] key)
        {
            using (FileStream fsOutput = new FileStream(destinationFile, FileMode.Create))
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                fsOutput.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream csEncrypt = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (ZipArchive zipArchive = new ZipArchive(csEncrypt, ZipArchiveMode.Create))
                {
                    foreach (string filePath in Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories))
                    {
                        string relativePath = Path.GetRelativePath(sourceFolder, filePath);
                        zipArchive.CreateEntryFromFile(filePath, relativePath);
                    }
                }
            }
        }

        private void RemoveNTFSProtection(string filePath)
        {
            try
            {
                FileInfo dInfo = new FileInfo(filePath);
                FileSecurity fileSecurity = dInfo.GetAccessControl();

                // Check if the file has NTFS protection
                if (!fileSecurity.AreAccessRulesProtected)
                {
                    return;
                }

                // Restore inherited permissions
                fileSecurity.SetAccessRuleProtection(false, true);

                // Remove explicit deny rules for everyone
                AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, false, typeof(SecurityIdentifier));
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.AccessControlType == AccessControlType.Deny)
                    {
                        fileSecurity.RemoveAccessRule(rule);
                    }
                }

                dInfo.SetAccessControl(fileSecurity);
                //MessageBox.Show("NTFS protection removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error removing NTFS protection: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyNTFSProtection(string filePath)
        {
            try
            {
                FileInfo dInfo = new FileInfo(filePath);

                FileSecurity fileSecurity = dInfo.GetAccessControl();

                // Remove existing permissions
                fileSecurity.SetAccessRuleProtection(true, false);

                // Deny delete, move, and copy for everyone
                fileSecurity.AddAccessRule(new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    FileSystemRights.Delete | FileSystemRights.WriteData | FileSystemRights.FullControl,
                    AccessControlType.Deny
                ));

                dInfo.SetAccessControl(fileSecurity);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error applying NTFS protection: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckIfHasNTFSProtection(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileSecurity fileSecurity = fileInfo.GetAccessControl();

                // Check if access rule protection is enabled (inheritance disconnected)
                bool isProtected = fileSecurity.AreAccessRulesProtected;
                if (!isProtected)
                    return false;

                // Get the access rules
                AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

                // Check for the specific deny rule for everyone (WorldSid)
                foreach (FileSystemAccessRule rule in rules)
                {
                    SecurityIdentifier sid = rule.IdentityReference as SecurityIdentifier;
                    if (sid != null && sid.IsWellKnown(WellKnownSidType.WorldSid))
                    {
                        // Check if this is the deny rule for Delete, WriteData, and FullControl
                        if (rule.AccessControlType == AccessControlType.Deny &&
                            (rule.FileSystemRights & (FileSystemRights.Delete | FileSystemRights.WriteData | FileSystemRights.FullControl)) != 0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                // If there's an error checking the permissions, assume it's not protected
                return false;
            }
        }
    }
}
