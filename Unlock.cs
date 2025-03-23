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
using System.Collections.Generic;
using Microsoft.Win32; // Add this for SystemEvents

namespace UCUFolderLocker
{
    public partial class Unlock : Form
    {
        private string lockFilePath;
        private string unlockedFolderPath;
        private string originalFolderName; // Store original folder name
        private string password;
        private bool folderWasUnlocked = false;
        private byte[] originalFileData; // Store the original file data for exact recreation
        private System.Threading.Timer relockTimer; // Add timer for periodic checks
        private bool hasNTFSProtection = false; // Track if the original file had NTFS protection

        public Unlock(string filePath)
        {
            InitializeComponent();
            lockFilePath = filePath;
            lblPath.Text = $"Lock File Path: {lockFilePath}";

            // Add event handler for form closing
            this.FormClosing += Unlock_FormClosing;

            // Register for system shutdown events
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;

            // Create a timer to periodically save the state to allow recovery
            relockTimer = new System.Threading.Timer(SaveRelockState, null, 30000, 30000); // Check every 30 seconds
        }

        // Handle system shutdown or logoff
        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            // Perform emergency relock
            if (folderWasUnlocked && originalFileData != null)
            {
                try
                {
                    EmergencyRelock();
                }
                catch
                {
                    // Cannot show message boxes during shutdown, just try our best
                }
            }
        }

        // Save relock information periodically
        private void SaveRelockState(object state)
        {
            if (folderWasUnlocked && password != null && unlockedFolderPath != null)
            {
                try
                {
                    // Create a recovery file with the information needed to relock
                    string recoveryPath = Path.Combine(Path.GetTempPath(), "UCUFolderLockerRecovery.dat");

                    // Encrypt the recovery data
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = GenerateKey("UCUFolderLockerRecoveryKey"); // Use a fixed key for recovery
                        aes.GenerateIV();

                        using (FileStream fs = new FileStream(recoveryPath, FileMode.Create))
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
                                sw.WriteLine(Convert.ToBase64String(originalFileData)); // Store original file data
                                sw.WriteLine(hasNTFSProtection); // Store NTFS protection status
                            }
                        }
                    }
                }
                catch
                {
                    // Silent fail for background operation
                }
            }
        }


        // Emergency relock method for shutdown events
        private void EmergencyRelock()
        {
            // Get the original lock file path (without .temp)
            string originalLockPath = lockFilePath.EndsWith(".temp")
                ? lockFilePath.Substring(0, lockFilePath.Length - 5)
                : lockFilePath;

            // Just restore the original file and protect it
            File.WriteAllBytes(originalLockPath, originalFileData);

            if (CheckIfHasNTFSProtection(lockFilePath))
            {
                ApplyNTFSProtection(originalLockPath);
            }

            // We won't try to delete the unlocked folder during emergency shutdown
            // as it might cause issues. The cleanup will be handled on next startup.
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            string lockedFilePath = lockFilePath;
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

            // Store password for relock
            password = txtPassword.Text;
            progressBarLoading.Visible = true;

            // Check if NTFS protection exists before removing it
            bool hadNTFSProtection = CheckIfHasNTFSProtection(lockedFilePath);

            // Store for later use when relocking
            hasNTFSProtection = hadNTFSProtection;

            // First remove NTFS protection if it exists, before attempting to read the file
            if (hadNTFSProtection)
            {
                try
                {
                    RemoveNTFSProtection(lockedFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing NTFS protection: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Error: Cannot remove NTFS protection.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Visible = true;
                    progressBarLoading.Visible = false;
                    return;
                }
            }

            // Now try to read the file after protection is removed
            try
            {
                originalFileData = File.ReadAllBytes(lockedFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading lock file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: Cannot read lock file.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                progressBarLoading.Visible = false;

                // If we had removed NTFS protection but still failed, try to reapply it
                if (hadNTFSProtection)
                {
                    try
                    {
                        ApplyNTFSProtection(lockedFilePath);
                    }
                    catch { /* Ignore errors when trying to restore protection */ }
                }
                return;
            }

            originalFolderName = Path.GetFileNameWithoutExtension(lockedFilePath);
            string destinationFolder = Path.Combine(Path.GetDirectoryName(lockedFilePath), originalFolderName);
            unlockedFolderPath = destinationFolder; // Store for relock

            try
            {
                byte[] key = GenerateKey(txtPassword.Text);
                DecryptAndExtractFolder(lockedFilePath, destinationFolder, key);

                // Make the folder visible but store that we should hide it when relocking
                DirectoryInfo dirInfo = new DirectoryInfo(destinationFolder);
                if ((dirInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    // Remove hidden attribute for user access
                    dirInfo.Attributes &= ~FileAttributes.Hidden;
                }

                // Instead, rename it to a temporary filename
                string tempLockFile = lockedFilePath + ".temp";
                if (File.Exists(tempLockFile))
                    File.Delete(tempLockFile);
                File.Move(lockedFilePath, tempLockFile);
                // Hide the temporary file so it can't be easily deleted
                File.SetAttributes(tempLockFile, File.GetAttributes(tempLockFile) | FileAttributes.Hidden);
                lockFilePath = tempLockFile; // Update the path

                folderWasUnlocked = true;
                lblStatus.Text = "Folder unlocked successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;

                // Disable the unlock button to prevent multiple clicks
                btnUnlock.Enabled = false;
                btnUnlock.Text = "Folder Unlocked";

                // Save state immediately after successful unlock
                SaveRelockState(null);

                MessageBox.Show("Folder unlocked successfully! It will be automatically locked when you close this window.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                lblStatus.Text = "Error unlocking folder.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
                MessageBox.Show("Error unlocking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarLoading.Visible = false;
            }
        }

        private void Unlock_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up system event handler
            SystemEvents.SessionEnding -= SystemEvents_SessionEnding;

            // Dispose timer
            relockTimer?.Dispose();
            relockTimer = null;

            // Delete recovery file if form is closing normally
            try
            {
                string recoveryPath = Path.Combine(Path.GetTempPath(), "UCUFolderLockerRecovery.dat");
                if (File.Exists(recoveryPath))
                {
                    File.Delete(recoveryPath);
                }
            }
            catch
            {
                // Ignore cleanup errors
            }

            // If the folder was unlocked, relock it
            if (folderWasUnlocked && originalFileData != null)
            {
                try
                {
                    // Find the folder by name, even if it moved
                    string actualFolderPath = FindFolderByName(originalFolderName);

                    if (string.IsNullOrEmpty(actualFolderPath))
                    {
                        // If can't find, try the original path
                        if (Directory.Exists(unlockedFolderPath))
                        {
                            actualFolderPath = unlockedFolderPath;
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show(
                                "Cannot find the unlocked folder. It may have been deleted or moved to an unknown location.\n\nDo you want to continue without locking?",
                                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                            if (result == DialogResult.No)
                            {
                                e.Cancel = true; // Prevent form from closing
                                return;
                            }
                            return; // Allow closing without locking
                        }
                    }

                    // Update the path to where we found the folder
                    unlockedFolderPath = actualFolderPath;

                    // Show a loading message
                    lblStatus.Text = "Checking for open files...";
                    lblStatus.ForeColor = System.Drawing.Color.Blue;
                    lblStatus.Visible = true;
                    progressBarLoading.Visible = true;
                    Application.DoEvents(); // Force UI update

                    // Check if any files in the folder are in use
                    List<string> openFiles = GetOpenFiles(unlockedFolderPath);
                    if (openFiles.Count > 0)
                    {
                        string fileList = string.Join("\n", openFiles.Take(5)); // Show first 5 files
                        if (openFiles.Count > 5)
                            fileList += $"\n... and {openFiles.Count - 5} more files";

                        DialogResult result = MessageBox.Show(
                            $"Some files are still open and cannot be locked. Please close these files first:\n\n{fileList}",
                            "Open Files Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        lblStatus.Text = "Cannot relock: Files are in use.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        progressBarLoading.Visible = false;
                        e.Cancel = true; // Prevent form from closing
                        return;
                    }

                    lblStatus.Text = "Re-locking folder...";
                    Application.DoEvents(); // Force UI update

                    // Get the original lock file path (without .temp)
                    string originalLockPath = lockFilePath.EndsWith(".temp")
                        ? lockFilePath.Substring(0, lockFilePath.Length - 5)
                        : lockFilePath;

                    // Check if folder still contains only files and no subfolders
                    if (Directory.GetDirectories(unlockedFolderPath).Length > 0)
                    {
                        MessageBox.Show(
                       "The folder contains subfolders, which cannot be locked. Please remove them before closing.",
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        e.Cancel = true; // Prevent form from closing
                        lblStatus.Text = "Re-locking canceled: Contains subfolders.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        progressBarLoading.Visible = false;
                        return;
                    }

                    // but first check if files have been modified
                    bool filesModified = CheckIfFilesModified();

                    if (filesModified)
                    {
                        // Get the original lock file path (without .temp)
                        string recoveryInfo = null;

                        if (originalFileData.Length >= 32)
                        {
                            recoveryInfo = Convert.ToBase64String(
                                originalFileData.Skip(originalFileData.Length - 32).Take(32).ToArray());
                        }

                        byte[] key = GenerateKey(password);

                        CompressAndEncryptFolder(unlockedFolderPath, originalLockPath, key);

                        if (recoveryInfo != null)
                        {
                            byte[] recoveryData = Convert.FromBase64String(recoveryInfo);
                            using (FileStream fs = new FileStream(originalLockPath, FileMode.Append))
                            {
                                fs.Write(recoveryData, 0, recoveryData.Length);
                            }
                        }
                    }
                    else
                    {
                        File.WriteAllBytes(originalLockPath, originalFileData);
                    }

                    // Only apply NTFS protection if the original file had it
                    if (hasNTFSProtection)
                    {
                        ApplyNTFSProtection(originalLockPath);
                    }

                    try
                    {
                        DeleteWithRetry(unlockedFolderPath, true);
                        if (File.Exists(lockFilePath) && lockFilePath.EndsWith(".temp"))
                            DeleteWithRetry(lockFilePath, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Warning: Could not clean up temporary files: " + ex.Message,
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error re-locking folder: " + ex.Message +
                        "\n\nYour folder remains unlocked at: " + unlockedFolderPath,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    progressBarLoading.Visible = false;
                }
            }
        }

        // Method to check if any files in the folder are currently in use
        private List<string> GetOpenFiles(string folderPath)
        {
            List<string> openFiles = new List<string>();

            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                try
                {
                    // Try to open the file with exclusive access
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        // File isn't locked if we get here
                        fs.Close();
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
        }

        // Helper method to find a folder by name in common locations
        private string FindFolderByName(string folderName)
        {
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

            // Search in all drives if not found in common locations
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
        }

        // Recursively search for folder in a directory (limited depth to avoid excessive searching)
        private string SearchFolderInDirectory(string directory, string folderName, int depth = 0)
        {
            if (depth > 3) // Limit search depth to avoid excessive recursion
                return null;

            try
            {
                // Check if the folder exists in this directory
                string potentialPath = Path.Combine(directory, folderName);
                if (Directory.Exists(potentialPath))
                {
                    return potentialPath;
                }

                // Check subdirectories (only common ones to avoid excessive searching)
                string[] commonSubdirs = { "Documents", "Downloads", "Desktop", "Users" };
                foreach (string dir in Directory.GetDirectories(directory))
                {
                    string dirName = Path.GetFileName(dir);
                    // Only search common directories or ones that might contain user files
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
            catch (Exception)
            {
                // Ignore access errors, etc.
            }

            return null; // Not found
        }

        private bool CheckIfFilesModified()
        {
            string[] files = Directory.GetFiles(unlockedFolderPath);

            if (files.Length == 0)
                return true;

            return true;
        }

        // Add this method to compress and encrypt folders for re-locking
        private void CompressAndEncryptFolder(string folderPath, string encryptedFilePath, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // Generate IV for encryption

                using (FileStream fsOutput = new FileStream(encryptedFilePath, FileMode.Create))
                {
                    // Write IV to the encrypted file first
                    fsOutput.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (ZipArchive zipArchive = new ZipArchive(csEncrypt, ZipArchiveMode.Create))
                    {
                        foreach (string file in Directory.GetFiles(folderPath))
                        {
                            zipArchive.CreateEntryFromFile(file, Path.GetFileName(file));
                        }
                    }
                }
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
                            // Create folder as hidden
                            if (!Directory.Exists(destinationFolder))
                            {
                                Directory.CreateDirectory(destinationFolder);
                                File.SetAttributes(destinationFolder, FileAttributes.Hidden);
                            }

                            zipArchive.ExtractToDirectory(destinationFolder);
                        }
                    }
                }
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
            catch (Exception ex)
            {
                //MessageBox.Show("Error removing NTFS protection: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ApplyNTFSProtection(string filePath)
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

        private static bool CheckIfHasNTFSProtection(string filePath)
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
                return false;
            }
        }

        private void DeleteWithRetry(string path, bool isDirectory, int maxRetries = 3)
        {
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
        }
    }
}