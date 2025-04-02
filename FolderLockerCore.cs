using System;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace UCUFolderLocker
{
    public class FolderLockerCore
    {
        // Directory paths
        private readonly string recoveryEmailFilePath;
        private readonly string recoveryCodesDirectory;
        private readonly string tempDirectory;

        // Progress reporting
        public delegate void ProgressCallback(int filesProcessed, int totalFiles, long bytesProcessed, long totalBytes);
        public event ProgressCallback OnProgressUpdated;

        public FolderLockerCore()
        {
            // Initialize important directories
            recoveryEmailFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "recovery_email.txt");
            recoveryCodesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "recovery_codes");
            tempDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "temp");

            // Ensure directories exist
            Directory.CreateDirectory(recoveryCodesDirectory);
            Directory.CreateDirectory(tempDirectory);
        }

        // Save recovery email
        public void SaveRecoveryEmail(string email)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(recoveryEmailFilePath));
            File.WriteAllText(recoveryEmailFilePath, email);
        }

        // Load recovery email
        public string LoadRecoveryEmail()
        {
            if (File.Exists(recoveryEmailFilePath))
            {
                return File.ReadAllText(recoveryEmailFilePath);
            }
            return string.Empty;
        }

        // Get state file path
        public string GetOperationStateFilePath()
        {
            return Path.Combine(tempDirectory, "operation_state.dat");
        }

        // Save current operation state
        public void SaveOperationState(string folderPath, string backupPath)
        {
            try
            {
                string stateFilePath = GetOperationStateFilePath();
                File.WriteAllText(stateFilePath, $"{folderPath}|{backupPath}|{DateTime.Now}");
            }
            catch (Exception)
            {
                // Ignore exceptions during exit
            }
        }

        // Clear operation state
        public void ClearOperationState()
        {
            try
            {
                string stateFilePath = GetOperationStateFilePath();
                if (File.Exists(stateFilePath))
                {
                    File.Delete(stateFilePath);
                }
            }
            catch { }
        }

        // Check if there's a pending operation
        public (bool hasPendingOperation, string folderPath, string backupPath) CheckForPendingOperation()
        {
            string stateFilePath = GetOperationStateFilePath();

            if (File.Exists(stateFilePath))
            {
                try
                {
                    string[] state = File.ReadAllText(stateFilePath).Split('|');
                    if (state.Length >= 2)
                    {
                        return (true, state[0], state[1]);
                    }
                }
                catch { }
            }

            return (false, string.Empty, string.Empty);
        }

        // Create backup of folder
        public string CreateBackup(string folderPath)
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

        // Restore from backup
        public void RestoreFromBackup(string backupPath, string originalPath)
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
        public void DeleteBackup(string backupPath)
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

        // Generate a random recovery code
        public string GenerateRecoveryCode()
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

        // Save recovery code mapping
        public void SaveRecoveryCodeMapping(string lockFilePath, string recoveryCode, string password)
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

        // Append recovery info to lock file
        public void AppendRecoveryInfo(string lockFilePath, string password, string recoveryEmail)
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

        // Apply NTFS protection to file
        public void ApplyNTFSProtection(string filePath)
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
                throw new Exception("Error applying NTFS protection: " + ex.Message);
            }
        }

        // Main method to compress and encrypt a folder
        public void CompressAndEncryptFolder(string folderPath, string password, bool applyNTFSProtection, string recoveryEmail, CancellationToken cancellationToken)
        {
            // Validate folder
            if (!Directory.Exists(folderPath) ||
                Directory.GetFiles(folderPath).Length == 0 ||
                Directory.GetDirectories(folderPath).Length > 0)
            {
                throw new Exception("The folder must contain at least one file and no subfolders!");
            }

            // Generate encryption key from password
            byte[] key = GenerateKey(password);

            // Generate recovery code
            string recoveryCode = GenerateRecoveryCode();

            // Create output path for encrypted file
            string encryptedFilePath = Path.Combine(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath) + ".lock");

            // Create backup of the folder before processing
            string backupFolderPath = CreateBackup(folderPath);

            try
            {
                // Save operation state
                SaveOperationState(folderPath, backupFolderPath);

                // Compress and encrypt the folder
                OptimizedCompressAndEncrypt(folderPath, encryptedFilePath, key, cancellationToken);

                // Save the recovery code mapping
                SaveRecoveryCodeMapping(encryptedFilePath, recoveryCode, password);

                // Add recovery info if email is provided
                if (!string.IsNullOrEmpty(recoveryEmail))
                {
                    AppendRecoveryInfo(encryptedFilePath, password, recoveryEmail);
                }

                // Delete the original folder after successful encryption
                Directory.Delete(folderPath, true);

                // Apply NTFS protection if requested
                if (applyNTFSProtection)
                {
                    ApplyNTFSProtection(encryptedFilePath);
                }

                // Clean up
                DeleteBackup(backupFolderPath);
                ClearOperationState();
            }
            catch (OperationCanceledException)
            {
                // Restore from backup if operation was canceled
                RestoreFromBackup(backupFolderPath, folderPath);
                throw;
            }
            catch (Exception ex)
            {
                // Restore from backup if operation failed
                RestoreFromBackup(backupFolderPath, folderPath);
                throw new Exception("Error during encryption: " + ex.Message);
            }
        }

        // Optimized compression and encryption
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

            // Report initial progress
            OnProgressUpdated?.Invoke(0, totalFiles, 0, totalBytes);

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
                                        OnProgressUpdated?.Invoke(processedFiles, totalFiles, processedBytes, totalBytes);
                                    }
                                }
                            }

                            // File completed
                            processedFiles++;

                            // Update UI with file completion
                            OnProgressUpdated?.Invoke(processedFiles, totalFiles, processedBytes, totalBytes);
                        }
                    }
                }
            }
        }

        // Encrypt string to bytes
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

        // Generate encryption key from password
        public static byte[] GenerateKey(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Determine optimal buffer size based on file sizes
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

        // Main method to decrypt and extract a locked folder
        public void DecryptAndExtractFolder(string lockFilePath, string password, CancellationToken cancellationToken)
        {
            // Validate lock file
            if (!File.Exists(lockFilePath) || !lockFilePath.EndsWith(".lock"))
            {
                throw new Exception("The selected file is not a valid locked folder!");
            }

            // Generate key from password
            byte[] key = GenerateKey(password);

            // Create output folder path automatically
            string outputFolderPath = Path.Combine(
                Path.GetDirectoryName(lockFilePath),
                Path.GetFileNameWithoutExtension(lockFilePath)
            );

            // Create the output directory if it doesn't exist
            Directory.CreateDirectory(outputFolderPath);

            try
            {
                // Check if file has NTFS protection and remove it if needed
                if (CheckIfHasNTFSProtection(lockFilePath))
                {
                    RemoveNTFSProtection(lockFilePath);
                }

                // Decrypt the file
                OptimizedDecryptAndExtract(lockFilePath, outputFolderPath, key, cancellationToken);

                // Operation completed successfully
                ClearOperationState();
            }
            catch (OperationCanceledException)
            {
                // Clean up partial extraction if operation was canceled
                if (Directory.Exists(outputFolderPath))
                {
                    try { Directory.Delete(outputFolderPath, true); } catch { }
                }
                throw;
            }
            catch (Exception ex)
            {
                // Clean up partial extraction if operation failed
                if (Directory.Exists(outputFolderPath) &&
                    Directory.GetFiles(outputFolderPath).Length == 0)
                {
                    try { Directory.Delete(outputFolderPath, true); } catch { }
                }
                throw new Exception("Error during decryption: " + ex.Message);
            }
        }

        // Optimized decryption and extraction
        private void OptimizedDecryptAndExtract(string lockFilePath, string outputFolderPath, byte[] key, CancellationToken cancellationToken)
        {
            // Get file size for progress tracking
            FileInfo lockFileInfo = new FileInfo(lockFilePath);
            long totalBytes = lockFileInfo.Length;
            long processedBytes = 0;

            // Determine optimal buffer size
            int bufferSize = DetermineOptimalBufferSize(totalBytes, 1);

            using (FileStream fsInput = new FileStream(lockFilePath, FileMode.Open))
            {
                // Read IV (first 16 bytes)
                byte[] iv = new byte[16]; // AES IV size is always 16 bytes
                fsInput.Read(iv, 0, iv.Length);

                // Update processed bytes
                processedBytes += iv.Length;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    try
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (ZipArchive zipArchive = new ZipArchive(csDecrypt, ZipArchiveMode.Read))
                        {
                            // Get total files for progress reporting
                            int totalFiles = zipArchive.Entries.Count;
                            int processedFiles = 0;

                            // Report initial progress
                            OnProgressUpdated?.Invoke(0, totalFiles, processedBytes, totalBytes);

                            // Extract each file
                            foreach (ZipArchiveEntry entry in zipArchive.Entries)
                            {
                                // Check for cancellation
                                cancellationToken.ThrowIfCancellationRequested();

                                // Create output file path
                                string outputFilePath = Path.Combine(outputFolderPath, entry.Name);

                                // Extract the file
                                using (Stream entryStream = entry.Open())
                                using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                                {
                                    byte[] buffer = new byte[bufferSize];
                                    int bytesRead;
                                    long entryBytesProcessed = 0;

                                    while ((bytesRead = entryStream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        // Check for cancellation
                                        cancellationToken.ThrowIfCancellationRequested();

                                        outputFileStream.Write(buffer, 0, bytesRead);

                                        // Update progress (approximation since we can't get exact compressed bytes)
                                        entryBytesProcessed += bytesRead;
                                        processedBytes = iv.Length + (long)((double)fsInput.Length * (processedFiles + (double)entryBytesProcessed / entry.Length) / totalFiles);

                                        // Update progress periodically to avoid UI freezing
                                        if (entryBytesProcessed % 262144 == 0)
                                        {
                                            OnProgressUpdated?.Invoke(processedFiles, totalFiles, processedBytes, totalBytes);
                                        }
                                    }
                                }

                                // File completed
                                processedFiles++;

                                // Update UI with file completion
                                OnProgressUpdated?.Invoke(processedFiles, totalFiles, processedBytes, totalBytes);
                            }
                        }
                    }
                    catch (CryptographicException)
                    {
                        throw new Exception("Incorrect password or corrupted file!");
                    }
                }
            }
        }

        // Method to verify password using recovery information (if available)
        public bool VerifyPassword(string lockFilePath, string password)
        {
            try
            {
                // Generate key from password
                byte[] key = GenerateKey(password);

                using (FileStream fsInput = new FileStream(lockFilePath, FileMode.Open))
                {
                    // Read IV (first 16 bytes)
                    byte[] iv = new byte[16];
                    fsInput.Read(iv, 0, iv.Length);

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;

                        // Try to decrypt the first few bytes to verify password
                        using (CryptoStream csDecrypt = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (BinaryReader reader = new BinaryReader(csDecrypt))
                        {
                            try
                            {
                                // Try to read the first few bytes of the decrypted stream
                                // This will throw an exception if the password is incorrect
                                reader.ReadBytes(4);
                                return true;
                            }
                            catch
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Check if recovery is possible for a locked folder
        public bool CanRecoverPassword(string lockFilePath)
        {
            string folderName = Path.GetFileNameWithoutExtension(lockFilePath);
            string recoveryFilePath = Path.Combine(recoveryCodesDirectory, $"{folderName}_recovery.dat");

            return File.Exists(recoveryFilePath);
        }

        // Try to recover password using recovery code
        public string TryRecoverPassword(string lockFilePath, string recoveryCode)
        {
            string folderName = Path.GetFileNameWithoutExtension(lockFilePath);
            string recoveryFilePath = Path.Combine(recoveryCodesDirectory, $"{folderName}_recovery.dat");

            if (!File.Exists(recoveryFilePath))
            {
                throw new Exception("No recovery information available for this folder!");
            }

            try
            {
                using (FileStream fs = new FileStream(recoveryFilePath, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    // Read stored recovery code
                    int recoveryCodeLength = reader.ReadInt32();
                    byte[] storedRecoveryCodeBytes = reader.ReadBytes(recoveryCodeLength);
                    string storedRecoveryCode = Encoding.UTF8.GetString(storedRecoveryCodeBytes);

                    // Check if recovery code matches
                    if (recoveryCode != storedRecoveryCode)
                    {
                        throw new Exception("Invalid recovery code!");
                    }

                    // Read recovery key
                    int keyLength = reader.ReadInt32();
                    byte[] recoveryKey = reader.ReadBytes(keyLength);

                    // Read encrypted password
                    int encryptedPasswordLength = reader.ReadInt32();
                    byte[] encryptedPassword = reader.ReadBytes(encryptedPasswordLength);

                    // Decrypt the password
                    return DecryptPassword(encryptedPassword, recoveryKey);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to recover password: {ex.Message}");
            }
        }

        // Decrypt password using recovery key
        private string DecryptPassword(byte[] encryptedData, byte[] key)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;

                    // First 16 bytes are the IV
                    byte[] iv = new byte[16];
                    Buffer.BlockCopy(encryptedData, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    // Remaining bytes are the encrypted password
                    byte[] cipherText = new byte[encryptedData.Length - iv.Length];
                    Buffer.BlockCopy(encryptedData, iv.Length, cipherText, 0, cipherText.Length);

                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch
            {
                throw new Exception("Failed to decrypt password with recovery key!");
            }
        }

        // NTFS protection methods provided by the user
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
                // Silently handle errors
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
    }

}