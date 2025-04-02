using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Net.Mail;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class PermanentLockForm : Form
    {
        private string recoveryEmailFilePath = Path.Combine(Application.StartupPath, "recovery_email.txt");

        private string storedHashedPassword;
        private string storedRecoveryCode;
        private string selectedFolderPath;
        private string storedEmail;

        // Path to the lock icon (should be in your project resources or a specific location)
        private string lockIconPath = Path.Combine(Application.StartupPath, "lock2.ico");

        public PermanentLockForm()
        {
            InitializeComponent();
            LoadRecoveryEmail();
        }

        // Select Folder Button
        // Modify the btnSelectFolder_Click method to call this new method
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = dialog.SelectedPath;
                    lblFolderPath.Text = selectedFolderPath;

                    // Check for existing lock data when folder is selected
                    CheckForExistingLockData(selectedFolderPath);
                }
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
                txtEmail.Text = File.ReadAllText(recoveryEmailFilePath);
            }
        }

        // Lock Folder Button
        private bool IsFolderLocked(string folderPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                DirectorySecurity security = directoryInfo.GetAccessControl();
                AuthorizationRuleCollection rules = security.GetAccessRules(true, true, typeof(SecurityIdentifier));
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference == everyone &&
                        rule.AccessControlType == AccessControlType.Deny)
                    {
                        FileSystemRights deniedRights = rule.FileSystemRights &
                            (FileSystemRights.FullControl | FileSystemRights.Delete | FileSystemRights.Write | FileSystemRights.Modify);
                        if (deniedRights != 0)
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

        private void ChangeFolderIcon(string folderPath)
        {
            try
            {
                // Create Desktop.ini content
                string desktopIniContent =
                    "[.ShellClassInfo]\r\n" +
                    $"IconFile={lockIconPath}\r\n" +
                    "IconIndex=0\r\n" +
                    "ConfirmFileOp=0\r\n";

                string desktopIniPath = Path.Combine(folderPath, "Desktop.ini");

                // Write Desktop.ini file
                File.WriteAllText(desktopIniPath, desktopIniContent);

                // Make Desktop.ini hidden and system file
                File.SetAttributes(desktopIniPath,
                    FileAttributes.Hidden | FileAttributes.System);

                // Set folder as system folder to apply custom icon
                DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
                dirInfo.Attributes |= FileAttributes.System;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error changing folder icon: " + ex.Message);
            }
        }

        private void LockFolder(string folderPath)
        {
            try
            {
                if (IsFolderLocked(folderPath))
                {
                    MessageBox.Show("This folder is already locked.");
                    return;
                }

                // Change folder icon FIRST
                ChangeFolderIcon(folderPath);

                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                DirectorySecurity security = directoryInfo.GetAccessControl();
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

                FileSystemAccessRule denyRule = new FileSystemAccessRule(
                    everyone,
                    FileSystemRights.FullControl | FileSystemRights.Delete | FileSystemRights.Write | FileSystemRights.Modify,
                    AccessControlType.Deny
                );

                security.AddAccessRule(denyRule);
                directoryInfo.SetAccessControl(security);

                MessageBox.Show("Folder locked successfully!");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied. Please run the application as an administrator.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error locking folder: " + ex.Message);
            }
        }

        // In the PermanentLockForm class, add this method to check for existing lock data
        private bool CheckForExistingLockData(string folderPath)
        {
            string lockDataPath = Path.Combine(folderPath, ".folderlock");

            if (File.Exists(lockDataPath))
            {
                try
                {
                    // First, try to ensure we have access to the file
                    try
                    {
                        File.SetAttributes(lockDataPath, FileAttributes.Normal);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //MessageBox.Show("Cannot access the lock file due to permission restrictions. Try running the application as an administrator.",
                        //                "Access Denied",
                        //                MessageBoxButtons.OK,
                        //                MessageBoxIcon.Warning);
                        return false;
                    }

                    string[] lockData = File.ReadAllLines(lockDataPath);
                    if (lockData.Length >= 3)
                    {
                        // Don't store the old values
                        // Instead, just notify the user that this folder was previously locked

                        // We can still pre-fill the email if desired
                        string storedEmail = lockData[2];
                        txtEmail.Text = storedEmail;

                        //MessageBox.Show("This folder was previously locked. A fresh lock will be created with your new password.",
                        //                "Previous Lock Data Found",
                        //                MessageBoxButtons.OK,
                        //                MessageBoxIcon.Information);

                        return true;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access to lock file is denied. Please run the application as an administrator.");
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading existing lock data: " + ex.Message);
                    return false;
                }
            }

            return false;
        }



        // Modify the btnLockFolder_Click method to handle previously locked folders
        private void btnLockFolder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                MessageBox.Show("Please select a folder first.");
                return;
            }

            // Check if folder is currently locked (not by checking for the file but by the actual lock status)
            if (IsFolderLocked(selectedFolderPath))
            {
                MessageBox.Show("This folder is already locked.");
                return;
            }

            // Always use new credentials when locking
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password.");
                return;
            }

            // Added password length validation
            if (password.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters long.");
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please confirm your password.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            if (string.IsNullOrEmpty(email))
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
                // Save recovery email to a portable location
                File.WriteAllText(recoveryEmailFilePath, email);
            }

            // Always generate fresh credentials
            string hashedPassword = HashPassword(password);
            string recoveryCode = GenerateRecoveryCode();

            try
            {
                // Store the lock data in a hidden file within the folder itself
                string lockDataPath = Path.Combine(selectedFolderPath, ".folderlock");

                // Check if the file exists and try to ensure it's accessible
                if (File.Exists(lockDataPath))
                {
                    try
                    {
                        // Try to remove any restricted attributes
                        File.SetAttributes(lockDataPath, FileAttributes.Normal);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Cannot access the existing lock file. Please run the application as an administrator.");
                        return;
                    }
                }

                // Save lock details
                File.WriteAllText(lockDataPath, $"{hashedPassword}\n{recoveryCode}\n{email}");



                // Make the lock file hidden
                File.SetAttributes(lockDataPath, FileAttributes.Hidden | FileAttributes.System);
                LockFolder(selectedFolderPath);

                if (IsFolderLocked(selectedFolderPath))
                {
                    // Always show the recovery code for the fresh lock
                    ShowRecoveryCode(recoveryCode, Path.GetFileName(selectedFolderPath));
                }

                txtConfirmPassword.Clear();
                txtPassword.Clear();
                selectedFolderPath = null;
                lblFolderPath.Clear();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied when writing lock file. Please run the application as an administrator.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing lock file: " + ex.Message);
                return;
            }
        }
        // Securely Hash Password
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Generate Recovery Code
        private string GenerateRecoveryCode()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] chars = new char[12];
            for (int i = 0; i < 12; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
                // Add hyphen after 4th and 8th characters
                if ((i + 1) % 4 == 0 && i < 11)
                {
                    i++;
                    chars[i] = '-';
                }
            }
            return new string(chars);
        }

        public static void ShowRecoveryCode(string recoveryCode, string folderName)
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
    }
}