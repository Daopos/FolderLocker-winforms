using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class ForgotForm : Form
    {
        private string selectedFolderPath = "";

        public ForgotForm()
        {
            InitializeComponent();
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
        private async void btnSend_Click(object sender, EventArgs e)
        {
            selectedFolderPath = lblFolderPath.Text;
            if (!File.Exists(selectedFolderPath))
            {
                MessageBox.Show("No lock file found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string recoveryEmail = txtRecoveryEmail.Text;
            if (string.IsNullOrEmpty(recoveryEmail))
            {
                MessageBox.Show("Please enter your recovery email!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            progressBarLoading.Visible = true;
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            btnSend.Enabled = false;

            // Check if NTFS protection exists before removing it
            bool hadNTFSProtection = CheckIfHasNTFSProtection(selectedFolderPath);

            try
            {
                if (hadNTFSProtection)
                {
                    RemoveNTFSProtection(selectedFolderPath);
                }

                string decryptedPassword = RetrievePassword(recoveryEmail, selectedFolderPath);
                // Send password via email
                await SendRecoveryEmail(recoveryEmail, decryptedPassword);

                // Only reapply NTFS protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(selectedFolderPath);
                }

                lblStatus.Text = "Successfully recovered your password.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;
                MessageBox.Show("Password sent to your email!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Only reapply NTFS protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(selectedFolderPath);
                }

                MessageBox.Show("Error recovering password: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error recovering your password.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
            }
            finally
            {
                progressBarLoading.Visible = false;
                btnSend.Enabled = true;
            }
        }


        private string RetrievePassword(string email, string lockFilePath)
        {
            if (!File.Exists(lockFilePath))
            {
                throw new Exception("Lock file not found!");
            }

            try
            {
                byte[] fileBytes = File.ReadAllBytes(lockFilePath);

                // Extract the encrypted password from the end of the file
                // The encrypted password consists of IV (16 bytes) + encrypted text
                // Assume we can identify it from the end of the file
                int encryptedPasswordSize = 32; // IV (16 bytes) + typical encrypted password length (varies)

                // Adjust size if needed by examining the actual files
                byte[] encryptedPasswordBytes = new byte[encryptedPasswordSize];
                Array.Copy(fileBytes, fileBytes.Length - encryptedPasswordSize, encryptedPasswordBytes, 0, encryptedPasswordSize);

                // Decrypt using email as the key
                byte[] emailKey = GenerateKey(email);
                return DecryptStringFromBytes(encryptedPasswordBytes, emailKey);
            }
            catch (Exception ex)
            {
                //throw new Exception("Failed to recover password: " + ex.Message);
                throw new Exception("Invalid Email");
            }
        }


        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key)
        {
            try
            {
                // First, extract the IV from the beginning of the cipherText
                byte[] iv = new byte[16]; // AES IV size is 16 bytes
                if (cipherText.Length < 16)
                {
                    throw new Exception("Encrypted data is too short to contain an IV");
                }

                Array.Copy(cipherText, 0, iv, 0, 16);

                // The actual encrypted data starts after the IV
                byte[] actualCipherText = new byte[cipherText.Length - 16];
                Array.Copy(cipherText, 16, actualCipherText, 0, cipherText.Length - 16);

                // Now decrypt with the extracted IV
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (MemoryStream msDecrypt = new MemoryStream(actualCipherText))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException)
            {
                throw new CryptographicException("Decryption failed. Invalid key or corrupted data.");
            }
        }

        private static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        private async Task SendRecoveryEmail(string email, string password)
        {
            await Task.Run(() =>
            {
                try
                {
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress("qma.mis.edu@gmail.com"),
                        Subject = "Password Recovery - Folder Lock",
                        Body = $"Dear User,\n\nWe received a request to recover your folder password. Please find your password below:\n\nPassword: {password}\n\nFor security reasons, we recommend changing your password.\n\nIf you did not request this recovery, please ignore this email.\n\nBest regards,\nIvan",
                        IsBodyHtml = false
                    };

                    mail.To.Add(email);

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("qma.mis.edu@gmail.com", "gdnqtehnexoblvxi");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to send recovery email. Please try again later. Error: " + ex.Message);
                }
            });
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
