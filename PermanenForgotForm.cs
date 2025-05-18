using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class PermanenForgotForm : Form
    {
        private string selectedFolderPath;
        private string storedRecoveryCode;
        private string storedHashedPassword;
        private string storedEmail;

        public PermanenForgotForm()
        {
            InitializeComponent();

            progressBarLoading.Visible = false;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = dialog.SelectedPath;
                    lblFolderPath.Text = selectedFolderPath;

                    // Try to load the lock data
                    LoadLockData();
                }
            }
        }

        // Load the lock data from file
        private void LoadLockData()
        {
            try
            {
                string lockDataPath = Path.Combine(selectedFolderPath, ".folderlock");

                if (File.Exists(lockDataPath))
                {
                    // Try to ensure we have access to the file
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
                        return;
                    }

                    string[] lines = File.ReadAllLines(lockDataPath);
                    if (lines.Length >= 3)
                    {
                        storedHashedPassword = lines[0];
                        storedRecoveryCode = lines[1];
                        storedEmail = lines[2];  

                        // Remove this line - don't prefill the email
                        // txtEmail.Text = storedEmail;

                        //MessageBox.Show("Lock data found. Please enter your recovery email to continue.",
                        //               "Lock Data Found",
                        //               MessageBoxButtons.OK,
                        //               MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Lock data file is corrupted or incomplete.");
                    }
                }
                else
                {
                    MessageBox.Show("Lock data file not found in this folder. Please make sure you're trying to recover the correct folder.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading lock data: " + ex.Message);
            }
        }

        // Email Recovery Button
        private void btnSendRecoveryEmail_Click(object sender, EventArgs e)
        {
            string userEmail = txtEmail.Text;
            if (string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("Please enter your recovery email.");
                return;
            }

            // Verify if email matches stored email
            if (string.IsNullOrEmpty(storedEmail) || storedEmail != userEmail)
            {
                MessageBox.Show("The email provided does not match the recovery email associated with this folder.");
                return;
            }

            if (string.IsNullOrEmpty(storedRecoveryCode))
            {
                MessageBox.Show("Recovery code not found. Please select the correct locked folder first.");
                return;
            }

            try
            {
                btnSend.Enabled = false;
                // Show progress bar and disable the button before sending
                progressBarLoading.Visible = true;
                progressBarLoading.Style = ProgressBarStyle.Marquee;
                progressBarLoading.MarqueeAnimationSpeed = 30;

                SendRecoveryEmail(userEmail, storedRecoveryCode);

            }
            catch (Exception ex)
            {
                // Hide progress bar if error occurs
                progressBarLoading.Visible = false;
                btnSend.Enabled = true;
                MessageBox.Show("Failed to send recovery email: " + ex.Message);
            }
        
        }

        private async void SendRecoveryEmail(string email, string recoveryCode)
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress("qma.mis.edu@gmail.com"),
                            Subject = "Folder Recovery",
                            Body = $"Dear User,\n\n" +
                                  $"We received a request to recover your folder access credentials. Please find your recovery information below:\n\n" +
                                  $"Recovery Code: {recoveryCode}\n\n" +
                                  $"For security reasons, we recommend changing your password after unlocking your folder.\n\n" +
                                  $"If you did not request this recovery, please ignore this email.\n\n" +
                                  $"Best regards,\nIvan",
                            IsBodyHtml = false
                        };
                        mail.To.Add(email);

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("qma.mis.edu@gmail.com", "gdnqtehnexoblvxi");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }

                        MessageBox.Show("Recovery code sent to your email.");

                        selectedFolderPath = null;
                        lblFolderPath.Clear();
                        txtEmail.Clear();

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to send recovery email. Please try again later. Error: " + ex.Message);
                    }finally
                    {

                        btnSend.Enabled = true;
                        // Show progress bar and disable the button before sending
                        progressBarLoading.Visible = false;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }
    }
}