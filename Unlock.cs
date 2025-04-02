using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Win32;

namespace UCUFolderLocker
{
    public partial class Unlock : Form
    {
        private string lockFilePath;
        private string unlockedFolderPath;
        private string originalFolderName;
        private string password;
        private bool folderWasUnlocked = false;
        private byte[] originalFileData;
        private System.Threading.Timer relockTimer;
        private bool hasNTFSProtection = false;
        private readonly string recoveryFilePath;
        private readonly string backupFolderPath;
        private CancellationTokenSource cts = new CancellationTokenSource();

        // Progress reporting
        private int totalFiles = 0;
        private int processedFiles = 0;

        public Unlock(string filePath)
        {
            InitializeComponent();
            lockFilePath = filePath;
            lblPath.Text = $"Lock File Path: {lockFilePath}";
            recoveryFilePath = Path.Combine(Path.GetTempPath(), "UCUFolderLockerRecovery.dat");
            backupFolderPath = Path.Combine(Path.GetTempPath(), "UCUFolderLockerBackup");

            // Add event handlers
            this.FormClosing += Unlock_FormClosing;

            // Create a timer for periodic state saving (every 30 seconds)
            relockTimer = new System.Threading.Timer(SaveRelockState, null, 30000, 30000);
        }

        // Toggle password visibility
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }

        // Save relock information periodically for recovery
        private async void SaveRelockState(object state)
        {
            if (!folderWasUnlocked || password == null || unlockedFolderPath == null) return;

            try
            {
                await Task.Run(() => {
                    try
                    {
                        // Encrypt the recovery data
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = GenerateKey("UCUFolderLockerRecoveryKey"); // Fixed key for recovery
                            aes.GenerateIV();

                            using (FileStream fs = new FileStream(recoveryFilePath, FileMode.Create))
                            {
                                // Write IV
                                fs.Write(aes.IV, 0, aes.IV.Length);

                                using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                                using (StreamWriter sw = new StreamWriter(cs))
                                {
                                    // Write recovery information
                                    sw.WriteLine(lockFilePath);
                                    sw.WriteLine(unlockedFolderPath);
                                    sw.WriteLine(originalFolderName);
                                    sw.WriteLine(password);
                                    sw.WriteLine(Convert.ToBase64String(originalFileData));
                                    sw.WriteLine(hasNTFSProtection);
                                }
                            }
                        }

                        // Create backup of unlocked files if they don't exist
                        if (Directory.Exists(unlockedFolderPath) && !Directory.Exists(backupFolderPath))
                        {
                            CreateBackupOfUnlockedFiles();
                        }
                    }
                    catch
                    {
                        // Silent fail for background operation
                    }
                });
            }
            catch
            {
                // Handle any Task-related exceptions
            }
        }

        private void CreateBackupOfUnlockedFiles()
        {
            try
            {
                // Create backup directory if it doesn't exist
                if (!Directory.Exists(backupFolderPath))
                {
                    Directory.CreateDirectory(backupFolderPath);
                }

                // Clear any existing files
                foreach (string file in Directory.GetFiles(backupFolderPath))
                {
                    File.Delete(file);
                }

                // Copy all files from unlocked folder to backup
                foreach (string file in Directory.GetFiles(unlockedFolderPath))
                {
                    File.Copy(file, Path.Combine(backupFolderPath, Path.GetFileName(file)), true);
                }
            }
            catch
            {
                // Silent fail for backup operation
            }
        }

        private async void btnUnlock_Click(object sender, EventArgs e)
        {

            // Reset cancellation token if needed
            if (cts.IsCancellationRequested)
            {
                cts.Dispose();
                cts = new CancellationTokenSource();
            }

            // Validate inputs
            if (string.IsNullOrEmpty(lockFilePath))
            {
                ShowError("Please select a locked file!");
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                ShowError("Please enter the password!");
                return;
            }

            // Store password for relock
            password = txtPassword.Text;

            // Update UI for loading state
            progressBarLoading.Value = 0;
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.Visible = true;
            btnUnlock.Enabled = false;
            lblStatus.Text = "Preparing to unlock...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;
            lblStatus.Visible = true;

            try
            {
                ControlBox = false; // Prevent closing during relock

                await Task.Run(async () => {
                    try
                    {
                        // Check if NTFS protection exists before removing it
                        hasNTFSProtection = CheckIfHasNTFSProtection(lockFilePath);

                        UpdateStatusSafe("Checking file protection...");

                        // First remove NTFS protection if it exists
                        if (hasNTFSProtection)
                        {
                            UpdateStatusSafe("Removing file protection...");
                            RemoveNTFSProtection(lockFilePath);
                        }

                        // Now try to read the file
                        UpdateStatusSafe("Reading lock file...");
                        originalFileData = File.ReadAllBytes(lockFilePath);
                        long fileSizeKB = originalFileData.Length / 1024;
                        UpdateStatusSafe($"Lock file size: {fileSizeKB} KB");

                        originalFolderName = Path.GetFileNameWithoutExtension(lockFilePath);
                        string destinationFolder = Path.Combine(Path.GetDirectoryName(lockFilePath), originalFolderName);
                        unlockedFolderPath = destinationFolder; // Store for relock

                        UpdateStatusSafe("Decrypting and extracting files...");

                        byte[] key = GenerateKey(txtPassword.Text);
                        int fileCount = await DecryptAndExtractFolderAsync(lockFilePath, destinationFolder, key, cts.Token);

                        // Make the folder visible
                        DirectoryInfo dirInfo = new DirectoryInfo(destinationFolder);
                        if ((dirInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                        {
                            dirInfo.Attributes &= ~FileAttributes.Hidden;
                        }

                        // Rename lock file to a temporary filename
                        string tempLockFile = lockFilePath + ".temp";
                        if (File.Exists(tempLockFile))
                            File.Delete(tempLockFile);

                        File.Move(lockFilePath, tempLockFile);
                        // Hide the temporary file
                        File.SetAttributes(tempLockFile, File.GetAttributes(tempLockFile) | FileAttributes.Hidden);
                        lockFilePath = tempLockFile; // Update the path

                        folderWasUnlocked = true;
                        UpdateStatusSafe($"Folder unlocked successfully: {fileCount} files extracted.", System.Drawing.Color.Green);

                        // Create backup immediately after successful unlock
                        CreateBackupOfUnlockedFiles();
                        SaveRelockState(null);

                        // Open the folder in Explorer
                        this.Invoke((MethodInvoker)delegate {
                            try
                            {
                                Process.Start("explorer.exe", destinationFolder);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Failed to open folder: " + ex.Message);
                            }

                            //// Update message to indicate that the folder will not be automatically locked
                            //MessageBox.Show("Folder unlocked successfully! It will remain unlocked even if the system shuts down. It will only be locked when you close this window.",
                            //    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        UpdateStatusSafe("Operation was canceled.", System.Drawing.Color.Red);
                    }
                    catch (CryptographicException)
                    {
                        // Restore protection if needed
                        if (hasNTFSProtection)
                        {
                            ApplyNTFSProtection(lockFilePath);
                        }
                        UpdateStatusSafe("Wrong password entered. Please try again.", System.Drawing.Color.Red);
                    }
                    catch (Exception ex)
                    {
                        // Restore protection if needed
                        if (hasNTFSProtection)
                        {
                            ApplyNTFSProtection(lockFilePath);
                        }
                        UpdateStatusSafe("Error unlocking folder: " + ex.Message, System.Drawing.Color.Red);
                    }
                }, cts.Token);
            }
            catch (Exception ex)
            {
                ShowError("Error in unlock operation: " + ex.Message);
            }
            finally
            {
                this.Invoke((MethodInvoker)delegate {
                    progressBarLoading.Style = ProgressBarStyle.Blocks;
                    progressBarLoading.Maximum = 100; // Add this line
                    progressBarLoading.Value = 100;
                    btnUnlock.Enabled = true;

                    if (folderWasUnlocked)
                    {
                        btnUnlock.Text = "Folder Unlocked";
                        btnUnlock.Enabled = false;
                    }
                });

                ControlBox = true; // Prevent closing during relock

            }
        }

        private async void Unlock_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up
            relockTimer?.Dispose();
            relockTimer = null;

            // If folder wasn't unlocked or no data to relock, just clean up and exit
            if (!folderWasUnlocked || originalFileData == null)
            {
                CleanupRecoveryFile();
                return;
            }

            e.Cancel = true; // Temporarily cancel closing to show progress

            // Update UI for relock process
            progressBarLoading.Value = 0;
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.Visible = true;
            lblStatus.Text = "Preparing to relock...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;
            lblStatus.Visible = true;

            bool relockSuccess = false;

            try
            {
                ControlBox = false; // Prevent closing during relock
                // Create a new CancellationTokenSource for the relock operation
                cts.Dispose();
                cts = new CancellationTokenSource();

                await Task.Run(async () => {
                    try
                    {
                        // Find the folder even if it was moved
                        string actualFolderPath = await FindFolderByNameAsync(originalFolderName);

                        if (string.IsNullOrEmpty(actualFolderPath))
                        {
                            // If can't find, try the original path
                            if (Directory.Exists(unlockedFolderPath))
                            {
                                actualFolderPath = unlockedFolderPath;
                            }
                            else
                            {
                                this.Invoke((MethodInvoker)delegate {
                                    DialogResult result = MessageBox.Show(
                                        "Cannot find the unlocked folder. It may have been deleted or moved to an unknown location.\n\n" +
                                        "Do you want to continue without locking? A recovery file will be kept in case you need to relock later.",
                                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                    if (result == DialogResult.Yes)
                                    {
                                        // Keep recovery file in this case and allow closing
                                        relockSuccess = true;
                                    }
                                });

                                if (relockSuccess)
                                    return;
                                else
                                    throw new Exception("Unable to find the folder to relock.");
                            }
                        }

                        // Update the path to where we found the folder
                        unlockedFolderPath = actualFolderPath;

                        UpdateStatusSafe("Checking for open files...");

                        // Check if any files in the folder are in use
                        List<string> openFiles = await GetOpenFilesAsync(unlockedFolderPath);
                        if (openFiles.Count > 0)
                        {
                            string fileList = string.Join("\n", openFiles.Take(5));
                            if (openFiles.Count > 5)
                                fileList += $"\n... and {openFiles.Count - 5} more files";

                            this.Invoke((MethodInvoker)delegate {
                                MessageBox.Show(
                                    $"Some files are still open and cannot be locked. Please close these files first:\n\n{fileList}",
                                    "Open Files Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            });

                            UpdateStatusSafe("Cannot relock: Files are in use.", System.Drawing.Color.Red);
                            return;
                        }

                        // Check for subfolders
                        if (Directory.GetDirectories(unlockedFolderPath).Length > 0)
                        {
                            this.Invoke((MethodInvoker)delegate {
                                MessageBox.Show(
                                   "The folder contains subfolders, which cannot be locked. Please remove them before closing.",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            });

                            UpdateStatusSafe("Re-locking canceled: Contains subfolders.", System.Drawing.Color.Red);
                            return;
                        }

                        // Get the original lock file path
                        string originalLockPath = lockFilePath.EndsWith(".temp")
                            ? lockFilePath.Substring(0, lockFilePath.Length - 5)
                            : lockFilePath;

                        // Check file stats for progress reporting
                        string[] files = Directory.GetFiles(unlockedFolderPath);
                        long totalSize = 0;
                        foreach (string file in files)
                        {
                            totalSize += new FileInfo(file).Length;
                        }

                        UpdateStatusSafe($"Re-locking folder ({files.Length} files, {totalSize / 1024} KB)...");

                        // Always create a fresh lock file when relocking
                        byte[] key = GenerateKey(password);
                        await CompressAndEncryptFolderAsync(unlockedFolderPath, originalLockPath, key, cts.Token);

                        // Store recovery info at the end (from original file)
                        if (originalFileData.Length >= 32)
                        {
                            byte[] recoveryData = originalFileData.Skip(originalFileData.Length - 32).Take(32).ToArray();
                            using (FileStream fs = new FileStream(originalLockPath, FileMode.Append))
                            {
                                fs.Write(recoveryData, 0, recoveryData.Length);
                            }
                        }

                        UpdateStatusSafe("Applying file protection...");

                        // Apply NTFS protection if the original file had it
                        if (hasNTFSProtection)
                        {
                            ApplyNTFSProtection(originalLockPath);
                        }

                        UpdateStatusSafe("Cleaning up...");

                        try
                        {
                            await DeleteWithRetryAsync(unlockedFolderPath, true);
                            if (File.Exists(lockFilePath) && lockFilePath.EndsWith(".temp"))
                                await DeleteWithRetryAsync(lockFilePath, false);

                            CleanupRecoveryFile();
                            relockSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            UpdateStatusSafe("Warning: Could not clean up temporary files: " + ex.Message, System.Drawing.Color.Orange);
                            // Still consider this a success, since the locked file was created
                            relockSuccess = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((MethodInvoker)delegate {
                            DialogResult result = MessageBox.Show(
                                "Error re-locking folder: " + ex.Message +
                                "\n\nYour folder remains unlocked at: " + unlockedFolderPath +
                                "\n\nA backup of your files has been saved at: " + backupFolderPath +
                                "\n\nDo you want to continue closing the application?",
                                "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                            if (result == DialogResult.Yes)
                            {
                                relockSuccess = true; // Allow closing despite error
                            }
                        });
                    }
                }, cts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical error during relock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                progressBarLoading.Style = ProgressBarStyle.Blocks;
                progressBarLoading.Maximum = 100; // Add this line
                progressBarLoading.Value = 100;
                ControlBox = true; // Prevent closing during relock

                if (relockSuccess)
                {
                    // Show message box confirming successful relock
                    MessageBox.Show(
                        "Folder has been successfully locked!",
                        "Folder Locked",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    // Actually close the form now
                    this.FormClosing -= Unlock_FormClosing;
                    this.Close();
                }
            }
        }

        // Helper method to safely update status from a background thread
        private void UpdateStatusSafe(string message, System.Drawing.Color? color = null)
        {
            if (this.IsDisposed) return;

            this.Invoke((MethodInvoker)delegate {
                lblStatus.Text = message;
                if (color.HasValue)
                    lblStatus.ForeColor = color.Value;

                // Update progress bar if we have file processing info
                if (totalFiles > 0 && processedFiles > 0)
                {
                    progressBarLoading.Style = ProgressBarStyle.Blocks;
                    progressBarLoading.Maximum = totalFiles;
                    progressBarLoading.Value = Math.Min(processedFiles, totalFiles);
                }
            });
        }

        // Helper method to show error messages consistently
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            lblStatus.Text = "Error: " + message;
            lblStatus.ForeColor = System.Drawing.Color.Red;
            lblStatus.Visible = true;
            progressBarLoading.Visible = false;
        }

        // Clean up recovery file
        private void CleanupRecoveryFile()
        {
            try
            {
                if (File.Exists(recoveryFilePath))
                {
                    File.Delete(recoveryFilePath);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        // Async method to check if any files in the folder are currently in use
        private async Task<List<string>> GetOpenFilesAsync(string folderPath)
        {
            return await Task.Run(() => {
                List<string> openFiles = new List<string>();

                foreach (string filePath in Directory.GetFiles(folderPath))
                {
                    try
                    {
                        // Try to open the file with exclusive access
                        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            // File isn't locked if we get here
                        }
                    }
                    catch (IOException)
                    {
                        // File is in use if an IOException occurs
                        openFiles.Add(Path.GetFileName(filePath));
                    }
                    catch
                    {
                        // Other exceptions (access denied, etc.) - assume file might be in use
                        openFiles.Add(Path.GetFileName(filePath));
                    }
                }

                return openFiles;
            });
        }

        // Async helper method to find a folder by name in common locations
        private async Task<string> FindFolderByNameAsync(string folderName)
        {
            return await Task.Run(() => {
                // First check if it's still in the original location
                if (Directory.Exists(unlockedFolderPath))
                {
                    return unlockedFolderPath;
                }

                // Check common locations
                string[] commonLocations = new string[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Path.GetDirectoryName(lockFilePath)
                };

                foreach (string location in commonLocations)
                {
                    string potentialPath = Path.Combine(location, folderName);
                    if (Directory.Exists(potentialPath))
                    {
                        return potentialPath;
                    }
                }

                // Search in all drives (limited search scope)
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        string potentialPath = SearchFolderInDirectory(drive.RootDirectory.FullName, folderName);
                        if (!string.IsNullOrEmpty(potentialPath))
                        {
                            return potentialPath;
                        }
                    }
                }

                return null; // Not found
            });
        }

        // Recursively search for folder in a directory (limited depth)
        private string SearchFolderInDirectory(string directory, string folderName, int depth = 0)
        {
            if (depth > 2) // Reduced search depth for better performance
                return null;

            try
            {
                // Check if the folder exists in this directory
                string potentialPath = Path.Combine(directory, folderName);
                if (Directory.Exists(potentialPath))
                {
                    return potentialPath;
                }

                // Check only common subdirectories
                string[] commonSubdirs = { "Documents", "Downloads", "Desktop", "Users" };
                foreach (string dir in Directory.GetDirectories(directory))
                {
                    string dirName = Path.GetFileName(dir);
                    if (commonSubdirs.Contains(dirName) ||
                        dirName.StartsWith("User") ||
                        dirName.Equals(Environment.UserName))
                    {
                        string result = SearchFolderInDirectory(dir, folderName, depth + 1);
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                }
            }
            catch
            {
                // Ignore access errors
            }

            return null;
        }

        // Async method to compress and encrypt folders with progress reporting
        private async Task CompressAndEncryptFolderAsync(string folderPath, string encryptedFilePath, byte[] key, CancellationToken token)
        {
            await Task.Run(() => {
                string[] files = Directory.GetFiles(folderPath);
                totalFiles = files.Length;
                processedFiles = 0;

                // Create the output directory if it doesn't exist
                string outputDir = Path.GetDirectoryName(encryptedFilePath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.GenerateIV();

                    using (FileStream fsOutput = new FileStream(encryptedFilePath, FileMode.Create))
                    {
                        // Write IV
                        fsOutput.Write(aes.IV, 0, aes.IV.Length);

                        using (CryptoStream csEncrypt = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        using (ZipArchive zipArchive = new ZipArchive(csEncrypt, ZipArchiveMode.Create))
                        {
                            foreach (string file in files)
                            {
                                token.ThrowIfCancellationRequested();

                                zipArchive.CreateEntryFromFile(file, Path.GetFileName(file));
                                processedFiles++;

                                // Update status every 10 files or for progress
                                if (processedFiles % 10 == 0 || processedFiles == totalFiles)
                                {
                                    UpdateStatusSafe($"Compressing files: {processedFiles}/{totalFiles}");
                                }
                            }
                        }
                    }
                }
            }, token);
        }

        // Improved async decrypt and extract method with progress reporting
        private async Task<int> DecryptAndExtractFolderAsync(string encryptedFilePath, string destinationFolder, byte[] key, CancellationToken token)
        {
            return await Task.Run(() => {
                int fileCount = 0;
                string tempFilePath = Path.GetTempFileName();

                try
                {
                    // Read the original encrypted file
                    byte[] fileData = File.ReadAllBytes(encryptedFilePath);

                    // The recovery information is 32 bytes at the end
                    int recoveryInfoSize = 32;
                    int actualDataSize = fileData.Length;

                    // Check if we have recovery info appended
                    if (fileData.Length > recoveryInfoSize)
                    {
                        actualDataSize = fileData.Length - recoveryInfoSize;
                        File.WriteAllBytes(tempFilePath, fileData.Take(actualDataSize).ToArray());
                    }
                    else
                    {
                        File.Copy(encryptedFilePath, tempFilePath, true);
                    }

                    // Now decrypt the cleaned file
                    using (FileStream fsInput = new FileStream(tempFilePath, FileMode.Open))
                    {
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = key;
                            byte[] iv = new byte[aes.IV.Length];

                            // Read IV
                            fsInput.Read(iv, 0, iv.Length);
                            aes.IV = iv;

                            using (CryptoStream csDecrypt = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                            using (ZipArchive zipArchive = new ZipArchive(csDecrypt, ZipArchiveMode.Read))
                            {
                                // Create folder as hidden
                                if (!Directory.Exists(destinationFolder))
                                {
                                    Directory.CreateDirectory(destinationFolder);
                                    File.SetAttributes(destinationFolder, FileAttributes.Hidden);
                                }

                                // Get file count for progress reporting
                                fileCount = zipArchive.Entries.Count;
                                totalFiles = fileCount;
                                processedFiles = 0;

                                // Extract each file, counting as we go
                                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                                {
                                    token.ThrowIfCancellationRequested();

                                    entry.ExtractToFile(Path.Combine(destinationFolder, entry.Name), true);
                                    processedFiles++;

                                    // Update status for progress
                                    if (processedFiles % 10 == 0 || processedFiles == totalFiles)
                                    {
                                        UpdateStatusSafe($"Extracting files: {processedFiles}/{totalFiles}");
                                    }
                                }
                            }
                        }
                    }

                    return fileCount;
                }
                catch (Exception ex)
                {
                    // Clean up temp file if it exists
                    if (File.Exists(tempFilePath))
                    {
                        try { File.Delete(tempFilePath); } catch { }
                    }

                    throw new Exception("Decryption failed: " + ex.Message, ex);
                }
                finally
                {
                    // Clean up temp file
                    if (File.Exists(tempFilePath))
                    {
                        try { File.Delete(tempFilePath); } catch { }
                    }
                }
            }, token);
        }

        // Generate encryption key from password
        private static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Remove NTFS protection
        private void RemoveNTFSProtection(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileSecurity fileSecurity = fileInfo.GetAccessControl();

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

                fileInfo.SetAccessControl(fileSecurity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing NTFS protection: " + ex.Message, ex);
            }
        }

        // Apply NTFS protection
        private static void ApplyNTFSProtection(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileSecurity fileSecurity = fileInfo.GetAccessControl();

                // Remove existing permissions
                fileSecurity.SetAccessRuleProtection(true, false);

                // Deny delete, move, and copy for everyone
                fileSecurity.AddAccessRule(new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    FileSystemRights.Delete | FileSystemRights.WriteData | FileSystemRights.FullControl,
                    AccessControlType.Deny
                ));

                fileInfo.SetAccessControl(fileSecurity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error applying NTFS protection: " + ex.Message, ex);
            }
        }

        // Check if a file has NTFS protection
        private static bool CheckIfHasNTFSProtection(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileSecurity fileSecurity = fileInfo.GetAccessControl();

                // Check if access rule protection is enabled
                bool isProtected = fileSecurity.AreAccessRulesProtected;
                if (!isProtected)
                    return false;

                // Get the access rules
                AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

                // Check for deny rule for everyone
                foreach (FileSystemAccessRule rule in rules)
                {
                    SecurityIdentifier sid = rule.IdentityReference as SecurityIdentifier;
                    if (sid != null && sid.IsWellKnown(WellKnownSidType.WorldSid))
                    {
                        if (rule.AccessControlType == AccessControlType.Deny &&
                            (rule.FileSystemRights & (FileSystemRights.Delete | FileSystemRights.WriteData | FileSystemRights.FullControl)) != 0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        // Delete file or directory with retry
        private async Task DeleteWithRetryAsync(string path, bool isDirectory, int maxRetries = 3)
        {
            await Task.Run(() => {
                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        if (isDirectory)
                            Directory.Delete(path, true);
                        else
                            File.Delete(path);

                        return; // Success
                    }
                    catch (IOException)
                    {
                        if (i < maxRetries - 1)
                            Thread.Sleep(500); // Wait before retry
                        else
                            throw; // Rethrow on last attempt
                    }
                }
            });
        }
    }
}