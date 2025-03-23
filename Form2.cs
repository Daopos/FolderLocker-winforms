using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class Form2: Form
    {

        private string configFilePath = "config.ini";
        private string storedEmail = "";
        private string storedUsername = "";
        private string storedPassword = "";

        public Form2()
        {
            InitializeComponent();
            LoadStoredCredentials();
        }

        private void LoadStoredCredentials()
        {
            if (File.Exists(configFilePath))
            {
                string[] lines = File.ReadAllLines(configFilePath);
                storedUsername = lines[0].Split('=')[1];
                storedPassword = lines[1].Split('=')[1];
                storedEmail = lines[2].Split('=')[1];
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (email != storedEmail)
            {
                MessageBox.Show("Email not found in system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Send email with credentials
            SendRecoveryEmail(email, storedUsername, storedPassword);
        }

        private void SendRecoveryEmail(string email, string username, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); // Use your SMTP server

                mail.From = new MailAddress("your_email@gmail.com"); // Sender's email
                mail.To.Add(email);
                mail.Subject = "Password Recovery - UCU Folder Locker";
                mail.Body = $"Dear User,\n\n" +
                    $"We received a request to recover your credentials in Folder Locker.\n\n" +
                    $"Your login credentials:\n" +
                    $"Username: {username}\n" +
                    $"Password: {password}\n\n" +
                    $"For security reasons, we recommend changing your password.\n\n" +
                    $"If you did not request this recovery, please ignore this email.\n\n" +
                    $"Best Regards,\nIvan";
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("qma.mis.edu@gmail.com", "gdnqtehnexoblvxi");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
                MessageBox.Show("Email sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
