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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class UnlockForm : Form
    {
        private string selectedFolderPath = "";
        private Label lblProgressDetails;
        private Button btnCancel;
        private CancellationTokenSource cancellationTokenSource;
        private string recoveryCodesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "recovery_codes");

        public UnlockForm()
        {
            InitializeComponent();

            // Initialize progress details label
            lblProgressDetails = new Label
            {
                AutoSize = true,
                Location = new Point(progressBarLoading.Left, progressBarLoading.Bottom + 5),
                Visible = false
            };
            this.Controls.Add(lblProgressDetails);

            // Initialize cancel button
            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(progressBarLoading.Right - 80, progressBarLoading.Bottom + 20),
                Size = new Size(80, 25),
                Visible = false
            };
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                lblStatus.Text = "Cancelling operation...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                lblStatus.Visible = true;
            }
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

        private async void btnUnlock_Click(object sender, EventArgs e)
        {
            string lockedFilePath = lblFolderPath.Text;
            string input = txtPassword.Text;

            if (string.IsNullOrEmpty(lockedFilePath))
            {
                MessageBox.Show("Please select a locked file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Unlock failed: No file selected.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter the password or recovery code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Unlock failed: No password or recovery code entered.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            // Determine if input is likely a recovery code (contains dashes or is long)
            string password = input;
            bool isRecoveryCode = IsLikelyRecoveryCode(input);

            if (isRecoveryCode)
            {
                try
                {
                    // Try to get the original password using the recovery code
                    lblStatus.Text = "Processing recovery code...";
                    lblStatus.ForeColor = System.Drawing.Color.Blue;
                    lblStatus.Visible = true;

                    password = GetPasswordFromRecoveryCode(input, lockedFilePath);

                    if (string.IsNullOrEmpty(password))
                    {
                        MessageBox.Show("Invalid recovery code or recovery information not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Unlock failed: Invalid recovery code.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Visible = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error processing recovery code: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Unlock failed: Recovery code error.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Visible = true;
                    return;
                }
            }

            // Setup the progress UI
            progressBarLoading.Visible = true;
            lblProgressDetails.Visible = true;
            btnCancel.Visible = true;
            lblStatus.Text = "Unlocking folder...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;
            lblStatus.Visible = true;

            // Initialize cancellation token
            cancellationTokenSource = new CancellationTokenSource();

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
                byte[] key = GenerateKey(password);

                // Run the decrypt operation asynchronously
                await Task.Run(() => OptimizedDecryptAndExtractFolder(lockedFilePath, destinationFolder, key, cancellationTokenSource.Token));

                // Delete the locked file after successful extraction if not cancelled
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    File.Delete(lockedFilePath);

                    lblStatus.Text = "Folder unlocked successfully.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Visible = true;

                    txtPassword.Clear();
                    lblFolderPath.Clear();

                    MessageBox.Show("Folder unlocked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                // Only reapply protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(lockedFilePath);
                }
                lblStatus.Text = "Operation cancelled.";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                lblStatus.Visible = true;
            }
            catch (CryptographicException)
            {
                // Only reapply protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(lockedFilePath);
                }

                string errorMessage = isRecoveryCode ?
                    "Recovery code did not produce a valid password." :
                    "Wrong password entered.";

                lblStatus.Text = errorMessage;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;

                MessageBox.Show(errorMessage + " Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Only reapply protection if it was there originally
                if (hadNTFSProtection)
                {
                    ApplyNTFSProtection(lockedFilePath);
                }
                lblStatus.Text = "Error unlocking folder.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                MessageBox.Show("Error unlocking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarLoading.Visible = false;
                lblProgressDetails.Visible = false;
                btnCancel.Visible = false;

                // Dispose of cancellation token
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
            }
        }

        private bool IsLikelyRecoveryCode(string input)
        {
            // Recovery codes typically contain dashes and/or are longer than normal passwords
            return input.Contains("-") || input.Length >= 20;
        }

        private string GetPasswordFromRecoveryCode(string recoveryCode, string lockFilePath)
        {
            string folderName = Path.GetFileNameWithoutExtension(lockFilePath);
            string recoveryFilePath = Path.Combine(recoveryCodesDirectory, $"{folderName}_recovery.dat");

            if (!File.Exists(recoveryFilePath))
            {
                throw new Exception($"Recovery file not found for {folderName}. Please make sure you're using the correct recovery code for this file.");
            }

            try
            {
                using (FileStream fs = new FileStream(recoveryFilePath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    // Read recovery code from file
                    int recoveryCodeLength = reader.ReadInt32();
                    byte[] savedRecoveryCodeBytes = reader.ReadBytes(recoveryCodeLength);
                    string savedRecoveryCode = Encoding.UTF8.GetString(savedRecoveryCodeBytes);

                    // Verify recovery code (case insensitive and ignore formatting)
                    string cleanInputCode = recoveryCode.Replace("-", "").Trim().ToUpper();
                    string cleanSavedCode = savedRecoveryCode.Replace("-", "").Trim().ToUpper();

                    if (cleanSavedCode != cleanInputCode)
                    {
                        throw new Exception("Invalid recovery code. Please check and try again.");
                    }

                    // Read recovery key
                    int keyLength = reader.ReadInt32();
                    byte[] recoveryKey = reader.ReadBytes(keyLength);

                    // Read encrypted password
                    int encryptedPasswordLength = reader.ReadInt32();
                    byte[] encryptedPassword = reader.ReadBytes(encryptedPasswordLength);

                    // Decrypt the password
                    return DecryptStringFromBytesTwo(encryptedPassword, recoveryKey);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing recovery code: " + ex.Message);
            }
        }

        private string DecryptStringFromBytesTwo(byte[] cipherText, byte[] key)
        {
            // Decrypt using the provided key
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                // Get the first 16 bytes as the IV
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(cipherText, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                // Create a decryptor
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption
                using (MemoryStream msDecrypt = new MemoryStream(cipherText, iv.Length, cipherText.Length - iv.Length))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    // Read the decrypted bytes from the decrypting stream
                    plaintext = srDecrypt.ReadToEnd();
                }
            }

            return plaintext;
        }

        private void OptimizedDecryptAndExtractFolder(string encryptedFilePath, string destinationFolder, byte[] key, CancellationToken cancellationToken)
        {
            // Create a temporary file to work with
            string tempFilePath = Path.GetTempFileName();

            try
            {
                // Read the original encrypted file, excluding the recovery information
                byte[] fileData = File.ReadAllBytes(encryptedFilePath);

                // Update progress UI after reading file
                UpdateProgressUI(0, fileData.Length);
                UpdateProgressDetailsUI(0, 0, 0, fileData.Length);

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

                    // Update progress after writing temp file
                    UpdateProgressUI(actualDataSize / 4, actualDataSize);
                }
                else
                {
                    // No recovery info, use the original file
                    File.Copy(encryptedFilePath, tempFilePath, true);
                }

                // Now decrypt the cleaned file
                using (FileStream fsInput = new FileStream(tempFilePath, FileMode.Open))
                {
                    // Check for cancellation
                    cancellationToken.ThrowIfCancellationRequested();

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        byte[] iv = new byte[aes.IV.Length];

                        // Read IV from the encrypted file
                        fsInput.Read(iv, 0, iv.Length);
                        aes.IV = iv;

                        // Update progress after reading IV
                        long processedBytes = iv.Length;
                        UpdateProgressUI(actualDataSize / 3, actualDataSize);

                        // Create directory if it doesn't exist
                        Directory.CreateDirectory(destinationFolder);

                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (ZipArchive zipArchive = new ZipArchive(csDecrypt, ZipArchiveMode.Read))
                        {
                            // Check for cancellation
                            cancellationToken.ThrowIfCancellationRequested();

                            int totalEntries = zipArchive.Entries.Count;
                            int processedEntries = 0;

                            // Update progress with file count
                            UpdateProgressUI(actualDataSize / 2, actualDataSize);
                            UpdateProgressDetailsUI(processedEntries, totalEntries, actualDataSize / 2, actualDataSize);

                            foreach (ZipArchiveEntry entry in zipArchive.Entries)
                            {
                                // Check for cancellation
                                cancellationToken.ThrowIfCancellationRequested();

                                string fullPath = Path.Combine(destinationFolder, entry.FullName);
                                string directory = Path.GetDirectoryName(fullPath);

                                // Create directory if needed
                                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }

                                // Skip directories (they are created above)
                                if (string.IsNullOrEmpty(entry.Name))
                                    continue;

                                using (Stream entryStream = entry.Open())
                                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                                {
                                    int bufferSize = DetermineOptimalBufferSize(entry.Length);
                                    byte[] buffer = new byte[bufferSize];
                                    int bytesRead;

                                    while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        // Check for cancellation
                                        cancellationToken.ThrowIfCancellationRequested();

                                        fileStream.Write(buffer, 0, bytesRead);
                                    }
                                }

                                // File completed
                                processedEntries++;

                                // Calculate progress as a combination of processed entries and total file progress
                                double entryProgress = (double)processedEntries / totalEntries;
                                long calculatedProgress = actualDataSize / 2 + (long)(actualDataSize / 2 * entryProgress);

                                UpdateProgressDetailsUI(processedEntries, totalEntries, calculatedProgress, actualDataSize);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // If extraction failed but directory was created, clean it up
                if (Directory.Exists(destinationFolder))
                {
                    try
                    {
                        Directory.Delete(destinationFolder, true);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
                throw; // Re-throw the original exception
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempFilePath))
                {
                    try
                    {
                        File.Delete(tempFilePath);
                    }
                    catch
                    {
                        // Ignore temp file cleanup errors
                    }
                }
            }
        }
        // Determine optimal buffer size based on file size and system memory
        private int DetermineOptimalBufferSize(long totalBytes)
        {
            // Default buffer sizes based on total data size
            if (totalBytes < 1024 * 1024) // < 1MB
                return 4096; // 4KB buffer for very small files
            else if (totalBytes < 10 * 1024 * 1024) // < 10MB
                return 16384; // 16KB buffer
            else if (totalBytes < 100 * 1024 * 1024) // < 100MB
                return 65536; // 64KB buffer
            else
                return 131072; // 128KB buffer for large files
        }

        // Update progress bar UI
        private void UpdateProgressUI(long processedBytes, long totalBytes)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgressUI(processedBytes, totalBytes)));
                return;
            }

            // Update progress bar
            progressBarLoading.Maximum = 100;
            int percentComplete = totalBytes > 0 ? (int)((processedBytes * 100) / totalBytes) : 0;
            progressBarLoading.Value = Math.Min(percentComplete, 100);
        }

        // Update detailed progress information
        private void UpdateProgressDetailsUI(int current, int total, long processedBytes, long totalBytes)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgressDetailsUI(current, total, processedBytes, totalBytes)));
                return;
            }

            // Update progress bar
            UpdateProgressUI(processedBytes, totalBytes);

            // Update progress text
            double mbProcessed = Math.Round(processedBytes / (1024.0 * 1024.0), 2);
            double mbTotal = Math.Round(totalBytes / (1024.0 * 1024.0), 2);

            if (total > 0)
                lblProgressDetails.Text = $"Extracting: {current}/{total} files ({mbProcessed} MB / {mbTotal} MB)";
            else
                lblProgressDetails.Text = $"Decrypting: {mbProcessed} MB / {mbTotal} MB";
        }

        private static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
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
            }
            catch (Exception)
            {
                // Ignore NTFS protection removal errors
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
            catch (Exception)
            {
                // Ignore NTFS protection application errors
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

        private void UnlockForm_Load(object sender, EventArgs e)
        {

        }
    }
}