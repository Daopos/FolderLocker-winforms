using System.IO.Compression;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace UCUFolderLocker
{
    public partial class Lock : Form
    {
        private string recoveryEmailFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "recovery_email.txt");
        private string recoveryCodesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "recovery_codes");
        private string tempDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "temp");
        private string selectedFolderPath = "";

        // Added for progress tracking
        private Label lblProgressDetails;
        private CancellationTokenSource cancellationTokenSource;

        // Added for tracking operation state
        private bool operationInProgress = false;
        private string currentOperationFolder = "";
        private string currentOperationBackupPath = "";
        private Button btnCancel;

        public Lock()
        {
            InitializeComponent();
            LoadRecoveryEmail();

            // Ensure directories exist
            Directory.CreateDirectory(recoveryCodesDirectory);
            Directory.CreateDirectory(tempDirectory);

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

            // Add application exit handler to prevent data loss
            Application.ApplicationExit += Application_ApplicationExit;

            // Create operation state file on startup to detect crash recovery
            CheckForCrashRecovery();
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

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            // If operation is in progress, save state for recovery
            if (operationInProgress)
            {
                SaveOperationState();
            }

            // Clean up resources
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Dispose();
            }
        }

        // Save current operation state to allow recovery after crash
        private void SaveOperationState()
        {
            try
            {
                string stateFilePath = Path.Combine(tempDirectory, "operation_state.dat");
                File.WriteAllText(stateFilePath, $"{currentOperationFolder}|{currentOperationBackupPath}|{DateTime.Now}");
            }
            catch (Exception)
            {
                // Ignore exceptions during exit
            }
        }

        // Check for crash recovery on startup
        private void CheckForCrashRecovery()
        {
            string stateFilePath = Path.Combine(tempDirectory, "operation_state.dat");

            if (File.Exists(stateFilePath))
            {
                try
                {
                    string[] state = File.ReadAllText(stateFilePath).Split('|');
                    if (state.Length >= 2)
                    {
                        string folder = state[0];
                        string backupPath = state[1];

                        if (Directory.Exists(backupPath))
                        {
                            DialogResult result = MessageBox.Show(
                                "The application was closed unexpectedly during a previous operation. Would you like to restore your files?",
                                "Recovery",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                RecoverFromCrash(folder, backupPath);
                            }
                        }
                    }

                    // Delete the state file regardless of recovery
                    File.Delete(stateFilePath);
                }
                catch (Exception)
                {
                    // If recovery fails, delete the state file
                    try { File.Delete(stateFilePath); } catch { }
                }
            }
        }

        // Recover files after a crash
        private void RecoverFromCrash(string folder, string backupPath)
        {
            try
            {
                // Restore original folder if it doesn't exist
                if (!Directory.Exists(folder) && Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(folder);

                    foreach (string file in Directory.GetFiles(backupPath))
                    {
                        string destFile = Path.Combine(folder, Path.GetFileName(file));
                        File.Copy(file, destFile, true);
                    }

                    MessageBox.Show("Your files have been successfully restored.", "Recovery Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Clean up backup directory
                try { Directory.Delete(backupPath, true); } catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during recovery: " + ex.Message, "Recovery Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }

        private void btnToggleConfirmPassword_Click(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !txtConfirmPassword.UseSystemPasswordChar;
            btnToggleConfirmPassword.Image = txtConfirmPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }

        private void LoadRecoveryEmail()
        {
            if (File.Exists(recoveryEmailFilePath))
            {
                txtRecoveryEmail.Text = File.ReadAllText(recoveryEmailFilePath);
            }
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

        private async void btnLock_Click(object sender, EventArgs e)
        {
            selectedFolderPath = lblFolderPath.Text;
            string recoveryEmail = txtRecoveryEmail.Text;

            if (!Directory.Exists(selectedFolderPath) ||
                Directory.GetFiles(selectedFolderPath).Length == 0 ||
                Directory.GetDirectories(selectedFolderPath).Length > 0)
            {
                MessageBox.Show("The folder must contain at least one file and no subfolders!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Locking failed: Folder must have files and no subfolders.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(selectedFolderPath) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please select a folder and enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Locking failed: Folder or password missing.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            if (txtPassword.Text.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters long!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Locking failed: Password too short.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Locking failed: Passwords do not match.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                return;
            }

            // Check if recovery email is provided
            if (string.IsNullOrEmpty(recoveryEmail))
            {
                DialogResult result = MessageBox.Show(
                    "No recovery email provided. You won't be able to recover your password if forgotten. Continue?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;
            }
            else
            {
                // Save recovery email
                Directory.CreateDirectory(Path.GetDirectoryName(recoveryEmailFilePath));
                File.WriteAllText(recoveryEmailFilePath, recoveryEmail);
            }

            // Generate recovery code - this won't modify the password
            string recoveryCode = GenerateRecoveryCode();

            // Compress and encrypt the folder into a single file
            string encryptedFilePath = Path.Combine(Path.GetDirectoryName(selectedFolderPath), Path.GetFileName(selectedFolderPath) + ".lock");

            try
            {
                // Disable controls during operation
                SetControlsEnabled(false);

                // Create backup of the folder before processing
                string backupFolderPath = CreateBackup(selectedFolderPath);

                // Set operation in progress
                operationInProgress = true;
                currentOperationFolder = selectedFolderPath;
                currentOperationBackupPath = backupFolderPath;
                SaveOperationState();

                // Set up and show progress controls
                progressBarLoading.Value = 0;
                progressBarLoading.Visible = true;
                lblProgressDetails.Visible = true;
                btnCancel.Visible = true;

                // Create cancellation token
                cancellationTokenSource = new CancellationTokenSource();

                byte[] key = GenerateKey(txtPassword.Text);

                // Compress and encrypt the folder asynchronously with optimizations
                await Task.Run(() => OptimizedCompressAndEncrypt(selectedFolderPath, encryptedFilePath, key, cancellationTokenSource.Token));

                // Save the recovery code mapping
                SaveRecoveryCodeMapping(encryptedFilePath, recoveryCode, txtPassword.Text);

                // Add recovery info if email is provided
                if (!string.IsNullOrEmpty(recoveryEmail))
                {
                    AppendRecoveryInfo(encryptedFilePath, txtPassword.Text, recoveryEmail);
                }

              /// Ensure all file handles are released before deletion
        GC.Collect();
                GC.WaitForPendingFinalizers();

                // Attempt to delete the original folder with permission adjustment
                try
                {
                    // Grant delete permission to current user
                    DirectoryInfo dirInfo = new DirectoryInfo(selectedFolderPath);
                    DirectorySecurity dirSecurity = dirInfo.GetAccessControl();
                    string currentUser = WindowsIdentity.GetCurrent().Name;
                    dirSecurity.AddAccessRule(new FileSystemAccessRule(currentUser, FileSystemRights.Delete, AccessControlType.Allow));
                    dirInfo.SetAccessControl(dirSecurity);

                    // Delete the folder
                    Directory.Delete(selectedFolderPath, true);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Log the error and inform the user, but proceed since encryption succeeded
                    MessageBox.Show($"Could not delete original folder due to permissions: {ex.Message}. The encrypted file is still created.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (chkNTFSProtection.Checked)
                {
                    ApplyNTFSProtection(encryptedFilePath);
                }

                // Operation completed successfully
                operationInProgress = false;
                DeleteBackup(backupFolderPath);

                lblStatus.Text = "Folder locked successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;

                // Show recovery code to user
                ShowRecoveryCode(recoveryCode, Path.GetFileName(selectedFolderPath));

                chkNTFSProtection.Checked = false;
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                lblFolderPath.Text = "";

                MessageBox.Show("Folder locked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                try
                {
                    if (File.Exists(encryptedFilePath))
                    {
                        File.Delete(encryptedFilePath);
                    }
                }
                catch (Exception) { /* Ignore any errors during cleanup */ }
                // Restore from backup if operation was canceled
                RestoreFromBackup(currentOperationBackupPath, selectedFolderPath);

                MessageBox.Show("Operation was canceled. Your files are unchanged.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Locking operation canceled.";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                lblStatus.Visible = true;
            }
            catch (Exception ex)
            {
                // Restore from backup if operation failed
                RestoreFromBackup(currentOperationBackupPath, selectedFolderPath);

                MessageBox.Show("Error locking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: Folder could not be locked.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
            }
            finally
            {
                // Mark operation as complete
                operationInProgress = false;
                currentOperationFolder = "";
                currentOperationBackupPath = "";

                // Delete operation state file
                try
                {
                    string stateFilePath = Path.Combine(tempDirectory, "operation_state.dat");
                    if (File.Exists(stateFilePath))
                    {
                        File.Delete(stateFilePath);
                    }
                }
                catch { }

                // Re-enable controls
                SetControlsEnabled(true);

                // Hide progress controls
                progressBarLoading.Visible = false;
                lblProgressDetails.Visible = false;
                btnCancel.Visible = false;

                // Dispose cancellation token
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Dispose();
                    cancellationTokenSource = null;
                }
            }
        }

        // Create backup of folder before processing
        private string CreateBackup(string folderPath)
        {
            string backupPath = Path.Combine(tempDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(backupPath);

            foreach (string file in Directory.GetFiles(folderPath))
            {
                string destFile = Path.Combine(backupPath, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            return backupPath;
        }

        // Restore from backup if operation fails or is canceled
        private void RestoreFromBackup(string backupPath, string originalPath)
        {
            if (Directory.Exists(backupPath) && Directory.Exists(originalPath))
            {
                // Clean original folder
                foreach (string file in Directory.GetFiles(originalPath))
                {
                    try { File.Delete(file); } catch { }
                }

                // Copy backup files
                foreach (string file in Directory.GetFiles(backupPath))
                {
                    string destFile = Path.Combine(originalPath, Path.GetFileName(file));
                    File.Copy(file, destFile, true);
                }

                // Delete backup
                DeleteBackup(backupPath);
            }
        }

        // Delete backup folder
        private void DeleteBackup(string backupPath)
        {
            if (Directory.Exists(backupPath))
            {
                try
                {
                    Directory.Delete(backupPath, true);
                }
                catch { }
            }
        }

        // Helper method to enable/disable controls during processing
        private void SetControlsEnabled(bool enabled)
        {
            buttonBrowse.Enabled = enabled;
            buttonLock.Enabled = enabled;
            txtPassword.Enabled = enabled;
            txtConfirmPassword.Enabled = enabled;
            txtRecoveryEmail.Enabled = enabled;
            chkNTFSProtection.Enabled = enabled;
            btnTogglePassword.Enabled = enabled;
            btnToggleConfirmPassword.Enabled = enabled;
        }

        // Generate a random recovery code
        private string GenerateRecoveryCode()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] chars = new char[12];

            for (int i = 0; i < 12; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
                // Add hyphen after every 4 characters except at the end
                if ((i + 1) % 4 == 0 && i < 11)
                {
                    i++;
                    chars[i] = '-';
                }
            }

            return new string(chars);
        }

        // Save recovery code with encrypted password
        private void SaveRecoveryCodeMapping(string lockFilePath, string recoveryCode, string password)
        {
            string folderName = Path.GetFileNameWithoutExtension(lockFilePath);
            string recoveryFilePath = Path.Combine(recoveryCodesDirectory, $"{folderName}_recovery.dat");

            try
            {
                // Create a random key for recovery code encryption
                byte[] recoveryKey = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(recoveryKey);
                }

                // Encrypt the original password using the recovery key
                byte[] encryptedPassword = EncryptStringToBytes(password, recoveryKey);

                // Save the recovery code mapping (recoveryCode:encryptedPassword:recoveryKey)
                using (FileStream fs = new FileStream(recoveryFilePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    byte[] recoveryCodeBytes = Encoding.UTF8.GetBytes(recoveryCode);

                    // Write recovery code length and data
                    writer.Write(recoveryCodeBytes.Length);
                    writer.Write(recoveryCodeBytes);

                    // Write recovery key
                    writer.Write(recoveryKey.Length);
                    writer.Write(recoveryKey);

                    // Write encrypted password
                    writer.Write(encryptedPassword.Length);
                    writer.Write(encryptedPassword);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save recovery information: " + ex.Message);
            }
        }

        // Show recovery code to user
        private void ShowRecoveryCode(string recoveryCode, string folderName)
        {
            using (Form recoveryForm = new Form())
            {
                recoveryForm.Text = "Recovery Code";
                recoveryForm.Size = new Size(450, 250);
                recoveryForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                recoveryForm.StartPosition = FormStartPosition.CenterParent;
                recoveryForm.MaximizeBox = false;
                recoveryForm.MinimizeBox = false;

                Label lblInfo = new Label
                {
                    Text = $"IMPORTANT: Keep this recovery code safe. You'll need it if you forget your password for '{folderName}'.",
                    Location = new Point(20, 20),
                    Size = new Size(400, 40),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                TextBox txtRecoveryCode = new TextBox
                {
                    Text = recoveryCode,
                    Location = new Point(75, 80),
                    Size = new Size(300, 30),
                    ReadOnly = true,
                    Font = new Font("Consolas", 12, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Center
                };

                Button btnCopy = new Button
                {
                    Text = "Copy to Clipboard",
                    Location = new Point(100, 130),
                    Size = new Size(250, 30)
                };
                btnCopy.Click += (s, e) =>
                {
                    Clipboard.SetText(recoveryCode);
                    MessageBox.Show("Recovery code copied to clipboard.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                Button btnClose = new Button
                {
                    Text = "Close",
                    Location = new Point(175, 170),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.OK
                };

                recoveryForm.Controls.Add(lblInfo);
                recoveryForm.Controls.Add(txtRecoveryCode);
                recoveryForm.Controls.Add(btnCopy);
                recoveryForm.Controls.Add(btnClose);

                recoveryForm.ShowDialog();
            }
        }

        // Add this method to append recovery information to the lock file
        private void AppendRecoveryInfo(string lockFilePath, string password, string recoveryEmail)
        {
            try
            {
                // Encrypt the password using the recovery email as the key
                byte[] emailKey = GenerateKey(recoveryEmail);
                byte[] encryptedPassword = EncryptStringToBytes(password, emailKey);

                // Append the encrypted password to the lock file
                using (FileStream fs = new FileStream(lockFilePath, FileMode.Append))
                {
                    fs.Write(encryptedPassword, 0, encryptedPassword.Length);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add recovery information: " + ex.Message);
            }
        }

        // Add this method to encrypt strings
        private byte[] EncryptStringToBytes(string plainText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Combine IV and encrypted data
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Write IV to the beginning of the stream
                    msEncrypt.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        // Optimized compression and encryption method for better performance on low-end devices
        private void OptimizedCompressAndEncrypt(string folderPath, string encryptedFilePath, byte[] key, CancellationToken cancellationToken)
        {
            // Get all files in the folder
            string[] files = Directory.GetFiles(folderPath);
            int totalFiles = files.Length;
            int processedFiles = 0;

            // Sort files by size (process smaller files first for responsive UI)
            Array.Sort(files, (a, b) =>
            {
                FileInfo fileA = new FileInfo(a);
                FileInfo fileB = new FileInfo(b);
                return fileA.Length.CompareTo(fileB.Length);
            });

            // Calculate total bytes for accurate progress
            long totalBytes = 0;
            long processedBytes = 0;

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                totalBytes += fileInfo.Length;
            }

            // Update the UI with total file count
            UpdateProgressUI(0, totalFiles, 0, totalBytes);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // Generate IV for encryption

                // Use buffer size based on file sizes to optimize for low-end devices
                int bufferSize = DetermineOptimalBufferSize(totalBytes, totalFiles);

                using (FileStream fsOutput = new FileStream(encryptedFilePath, FileMode.Create))
                {
                    // Write IV to the encrypted file first
                    fsOutput.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (ZipArchive zipArchive = new ZipArchive(csEncrypt, ZipArchiveMode.Create))
                    {
                        foreach (string file in files)
                        {
                            // Check for cancellation
                            cancellationToken.ThrowIfCancellationRequested();

                            // Get file size
                            FileInfo fileInfo = new FileInfo(file);
                            long fileSize = fileInfo.Length;

                            // Create file entry in zip
                            ZipArchiveEntry entry = zipArchive.CreateEntry(Path.GetFileName(file), CompressionLevel.Fastest);

                            using (Stream entryStream = entry.Open())
                            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize))
                            {
                                byte[] buffer = new byte[bufferSize];
                                int bytesRead;
                                long fileBytesProcessed = 0;

                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    // Check for cancellation
                                    cancellationToken.ThrowIfCancellationRequested();

                                    entryStream.Write(buffer, 0, bytesRead);

                                    // Update progress
                                    fileBytesProcessed += bytesRead;
                                    processedBytes += bytesRead;

                                    // Update progress every 256KB to avoid UI freezing
                                    if (fileBytesProcessed % 262144 == 0 || fileBytesProcessed == fileSize)
                                    {
                                        UpdateProgressUI(processedFiles, totalFiles, processedBytes, totalBytes);
                                    }
                                }
                            }

                            // File completed
                            processedFiles++;

                            // Update UI with file completion
                            UpdateProgressUI(processedFiles, totalFiles, processedBytes, totalBytes);
                        }
                    }
                }
            }
        }

        // Determine optimal buffer size based on file sizes and system memory
        private int DetermineOptimalBufferSize(long totalBytes, int totalFiles)
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

        // Update UI elements with progress information
        private void UpdateProgressUI(int current, int total, long processedBytes, long totalBytes)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgressUI(current, total, processedBytes, totalBytes)));
                return;
            }

            // Update progress bar
            progressBarLoading.Maximum = 100;
            int percentComplete = totalBytes > 0 ? (int)((processedBytes * 100) / totalBytes) : 0;
            progressBarLoading.Value = Math.Min(percentComplete, 100);

            // Update progress text
            double mbProcessed = Math.Round(processedBytes / (1024.0 * 1024.0), 2);
            double mbTotal = Math.Round(totalBytes / (1024.0 * 1024.0), 2);
            lblProgressDetails.Text = $"Processing: {current}/{total} files ({mbProcessed} MB / {mbTotal} MB)";
        }

        private static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
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
    }
}