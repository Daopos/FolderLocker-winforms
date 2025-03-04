using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
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
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = folderDialog.SelectedPath;
                    lblFolderPath.Text = selectedFolderPath;
                }
            }
        }
        private async void btnSend_Click(object sender, EventArgs e)
        {
            selectedFolderPath = lblFolderPath.Text;

            if (string.IsNullOrEmpty(selectedFolderPath) || !Directory.Exists(selectedFolderPath))
            {
                MessageBox.Show("Invalid folder path! Please select a locked folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string keyFilePath = Path.Combine(selectedFolderPath, "lock.key");
            string recoveryFilePath = Path.Combine(selectedFolderPath, "recovery.dat");


            if (!File.Exists(keyFilePath) || !File.Exists(recoveryFilePath))
            {
                MessageBox.Show("This folder is not locked or missing recovery data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSend.Enabled = false;
                lblStatus.Visible = false;
                progressBarLoading.Visible = true; // Show progress bar

                string[] recoveryData = File.ReadAllLines(recoveryFilePath);
                string recoveryEmail = recoveryData[0];
                string encryptedPassword = recoveryData[1];

                string decryptedPassword = DecryptData(encryptedPassword, "your_secret_key"); // Decrypt password

                await SendRecoveryEmail(recoveryEmail, decryptedPassword);

                lblStatus.Visible = true;
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = "Status: Recovery sent successfully!";
                MessageBox.Show("Password has been sent to your recovery email!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Visible = true;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "Status: Error occurred!";
                MessageBox.Show("Failed to send email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBarLoading.Visible = false; // Hide progress bar
                btnSend.Enabled = true;
            }
        }

        private static string DecryptData(string encryptedText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                aes.IV = new byte[16];

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
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
                        Body = $"Dear User,\n\nWe received a request to recover your folder password. Please find your password below:\n\nPassword: {password}\n\nFor security reasons, we recommend changing your password after logging in.\n\nIf you did not request this recovery, please ignore this email.\n\nBest regards,\nIvan",
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

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
