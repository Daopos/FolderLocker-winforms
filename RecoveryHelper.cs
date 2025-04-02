using System;
using System.Drawing;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public class RecoveryHelper
    {
        // Show recovery code dialog
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
                    Font = new Font("Arial", 12, FontStyle.Bold),
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

        // Handle crash recovery
        public static bool HandleCrashRecovery(string folder, string backupPath)
        {
            try
            {
                // Check if backup exists
                if (!Directory.Exists(backupPath))
                {
                    return false;
                }

                // Ask user if they want to recover
                DialogResult result = MessageBox.Show(
                    "The application was closed unexpectedly during a previous operation. Would you like to restore your files?",
                    "Recovery",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Restore original folder if it doesn't exist
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    foreach (string file in Directory.GetFiles(backupPath))
                    {
                        string destFile = Path.Combine(folder, Path.GetFileName(file));
                        File.Copy(file, destFile, true);
                    }

                    MessageBox.Show("Your files have been successfully restored.", "Recovery Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clean up backup directory
                    try { Directory.Delete(backupPath, true); } catch { }

                    return true;
                }

                // Clean up backup directory if user chose not to recover
                try { Directory.Delete(backupPath, true); } catch { }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during recovery: " + ex.Message, "Recovery Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}