using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace UCUFolderLocker
{
    public partial class Lock : Form
    {
        private string recoveryEmailFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UCUFolderLocker", "recovery_email.txt");

        private string selectedFolderPath = "";
        public Lock()
        {
            InitializeComponent();
            LoadRecoveryEmail();
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

        private void btnLock_Click(object sender, EventArgs e)
        {
            selectedFolderPath = lblFolderPath.Text;
            string recoveryEmail = txtRecoveryEmail.Text.Trim();

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

            // Compress and encrypt the folder into a single file
            string encryptedFilePath = Path.Combine(Path.GetDirectoryName(selectedFolderPath), Path.GetFileName(selectedFolderPath) + ".lock");
            try
            {
                progressBarLoading.Visible = true;
                byte[] key = GenerateKey(txtPassword.Text);

                // Compress and encrypt the folder
                CompressAndEncryptFolder(selectedFolderPath, encryptedFilePath, key);

                // Add recovery info if email is provided
                if (!string.IsNullOrEmpty(recoveryEmail))
                {
                    AppendRecoveryInfo(encryptedFilePath, txtPassword.Text, recoveryEmail);
                }

                // Delete the original folder after successful encryption
                Directory.Delete(selectedFolderPath, true);

                if (chkNTFSProtection.Checked)
                {
                    ApplyNTFSProtection(encryptedFilePath);
                }

                lblStatus.Text = "Folder locked successfully.";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Visible = true;

                chkNTFSProtection.Checked = false;
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                lblFolderPath.Clear();

                MessageBox.Show("Folder locked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                progressBarLoading.Visible = false;
                MessageBox.Show("Error locking folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error: Folder could not be locked.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Visible = true;
            }
            finally
            {
                progressBarLoading.Visible = false;
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
