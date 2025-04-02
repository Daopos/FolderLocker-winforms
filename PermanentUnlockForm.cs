using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace UCUFolderLocker
{
    public partial class PermanentUnlockForm : Form
    {
        private string selectedFolderPath;
        private string savedHashedPassword;
        private string savedRecoveryCode;
        private string savedEmail;
        private Button btnRelockFolder;

        public PermanentUnlockForm()
        {
            InitializeComponent();

            // Create the Relock button but don't add it to controls yet
            btnRelockFolder = new Button
            {
                Text = "Relock Folder",
                Size = new Size(btnUnlockFolder.Size.Width, btnUnlockFolder.Size.Height),
                Location = new Point(btnUnlockFolder.Location.X, btnUnlockFolder.Location.Y + btnUnlockFolder.Size.Height + 10),
                Visible = false
            };
            btnRelockFolder.Click += new EventHandler(btnRelockFolder_Click);
            this.Controls.Add(btnRelockFolder);
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }

        // Select Folder Button
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = dialog.SelectedPath;
                    lblFolderPath.Text = selectedFolderPath;

                    // Hide relock button when browsing for a new folder
                    btnRelockFolder.Visible = false;
                }
            }
        }

        // Unlock Folder Button
        private void btnUnlockFolder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                MessageBox.Show("Please select the locked folder.");
                return;
            }

            if (!IsFolderLocked(selectedFolderPath))
            {
                MessageBox.Show("The folder is not locked. No unlocking is necessary.");
                return;
            }

            string enteredPassword = txtPassword.Text;

            // Check for the hidden lock file in the folder itself
            string lockDataPath = Path.Combine(selectedFolderPath, ".folderlock");

            if (!File.Exists(lockDataPath))
            {
                // Check for the old lockdata.txt for backward compatibility
                if (File.Exists("lockdata.txt"))
                {
                    lockDataPath = "lockdata.txt";
                }
                else
                {
                    MessageBox.Show("No lock data found for this folder!");
                    return;
                }
            }

            string[] lockData = File.ReadAllLines(lockDataPath);
            if (lockData.Length < 3)
            {
                MessageBox.Show("Lock data is corrupted!");
                return;
            }

            savedHashedPassword = lockData[0];
            savedRecoveryCode = lockData[1];
            savedEmail = lockData.Length > 2 ? lockData[2] : "";

            if (HashPassword(enteredPassword) == savedHashedPassword || enteredPassword == savedRecoveryCode)
            {
                UnlockFolder(selectedFolderPath);

                // Don't delete the lock file, we'll need it if we relock

                // Show the relock button
                btnRelockFolder.Visible = true;

                //txtPassword.Clear();
                //selectedFolderPath = null;
                //lblFolderPath.Clear();
            }
            else
            {
                MessageBox.Show("Incorrect password or recovery code.");
            }
        }

        // Relock Folder Button
        private void btnRelockFolder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                MessageBox.Show("Folder path is not valid.");
                return;
            }

            // Check if folder exists
            if (!Directory.Exists(selectedFolderPath))
            {
                MessageBox.Show("The folder no longer exists.");
                return;
            }

            //txtPassword.Clear();
            //selectedFolderPath = null;
            //lblFolderPath.Clear();
            // Relock the folder using the same credentials
            RelockFolder(selectedFolderPath);

            // Hide the relock button after relocking
            btnRelockFolder.Visible = false;
        }

        private void RelockFolder(string folderPath)
        {
            try
            {
                // Check if the folder is already locked
                if (IsFolderLocked(folderPath))
                {
                    MessageBox.Show("This folder is already locked.");
                    return;
                }

                // Change folder icon first
                ChangeFolderIcon(folderPath);

                // Apply access restrictions
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

                MessageBox.Show("Folder relocked successfully!");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied. Please run the application as an administrator.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error relocking folder: " + ex.Message);
            }
        }

        private bool IsFolderLocked(string folderPath)
        {
            // First try the original ACL check
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
            }
            catch (Exception)
            {
                // If we can't check permissions, continue to the file creation test
            }

            // Additional check: Try to create a test file
            string testFilePath = Path.Combine(folderPath, ".lock_test_" + Guid.NewGuid().ToString() + ".tmp");
            try
            {
                // Try to create a temporary file
                using (FileStream fs = File.Create(testFilePath, 1, FileOptions.DeleteOnClose))
                {
                    // If we can create the file, the folder is not locked
                }
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                // Access denied means the folder is probably locked
                return true;
            }
            catch (Exception)
            {
                // For any other exception, try to delete the file if it exists
                try
                {
                    if (File.Exists(testFilePath))
                    {
                        File.Delete(testFilePath);
                    }
                }
                catch { }

                // If we get here, we're not sure, so return false
                return false;
            }
        }

        private void ChangeFolderIcon(string folderPath)
        {
            try
            {
                // Path to the lock icon
                string lockIconPath = Path.Combine(Application.StartupPath, "lock2.ico");

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

        private void UnlockFolder(string folderPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                DirectorySecurity security = directoryInfo.GetAccessControl();
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

                // Remove the Deny rule for "Everyone" that was added during locking
                FileSystemAccessRule denyRule = new FileSystemAccessRule(
                    everyone,
                    FileSystemRights.FullControl | FileSystemRights.Delete | FileSystemRights.Write | FileSystemRights.Modify,
                    AccessControlType.Deny
                );
                security.RemoveAccessRule(denyRule);

                // Optional: Add an Allow rule to ensure full access (with inheritance)
                FileSystemAccessRule allowRule = new FileSystemAccessRule(
                    everyone,
                    FileSystemRights.FullControl,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, // Apply to folder and contents
                    PropagationFlags.None,
                    AccessControlType.Allow
                );
                security.AddAccessRule(allowRule);

                // Apply the updated security settings
                directoryInfo.SetAccessControl(security);

                // Remove the custom icon by deleting Desktop.ini and resetting folder attributes
                string desktopIniPath = Path.Combine(folderPath, "Desktop.ini");
                if (File.Exists(desktopIniPath))
                {
                    // Remove the system and hidden attributes from Desktop.ini
                    File.SetAttributes(desktopIniPath, FileAttributes.Normal);
                    // Delete the Desktop.ini file
                    File.Delete(desktopIniPath);
                }

                // Remove the system attribute from the folder to revert to default icon
                directoryInfo.Attributes &= ~FileAttributes.System;

                MessageBox.Show("Folder unlocked successfully!");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access denied. Please run the application as an administrator.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unlocking folder: " + ex.Message);
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
    }
}