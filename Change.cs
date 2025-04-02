using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class Change : Form
    {
        private string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public Change()
        {
            InitializeComponent();
            LoadCurrentCredentials();
        }
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !password.UseSystemPasswordChar;
            btnTogglePassword.Image = password.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }
        private void btnToggleNewPassword_Click(object sender, EventArgs e)
        {
            newPassword.UseSystemPasswordChar = !newPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = newPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }
        private void LoadCurrentCredentials()
        {
            if (!File.Exists(configFilePath))
            {
                MessageBox.Show("Configuration file not found. Resetting to default credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                File.WriteAllText(configFilePath, "username=admin\npassword=admin123\nemail=");
            }

            string[] lines = File.ReadAllLines(configFilePath);


            // Check if email exists
            string storedEmail = lines.Length > 2 && lines[2].StartsWith("email=") ? lines[2].Split('=')[1] : "";


            email.Text = storedEmail; // Display email if available
        }

        private bool ValidateCurrentCredentials(string currentUsername, string currentPassword)
        {
            string[] lines = File.ReadAllLines(configFilePath);
            string storedUsername = lines[0].Split('=')[1];
            string storedPassword = lines[1].Split('=')[1];

            return currentUsername == storedUsername && currentPassword == storedPassword;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string currentUsername = username.Text;
            string currentPassword = password.Text;
            string newsUsername = newUsername.Text;
            string newsPassword = newPassword.Text;
            string newEmail = email.Text;

            if (!ValidateCurrentCredentials(currentUsername, currentPassword))
            {
                MessageBox.Show("Incorrect current username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(newsUsername) || string.IsNullOrWhiteSpace(newsPassword))
            {
                MessageBox.Show("New username and password cannot be empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (newsUsername.Length < 4)
            {
                MessageBox.Show("New username must be at least 4 characters long.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newsPassword.Length < 4)
            {
                MessageBox.Show("New password must be at least 4 characters long.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show("Email is required for recovery. Changes were not saved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdateCredentials(newsUsername, newsPassword, newEmail);

            username.Clear();
            password.Clear();
            newUsername.Clear();
            newPassword.Clear();


            MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateCredentials(string newUsername, string newPassword, string newEmail)
        {
            string[] lines = File.ReadAllLines(configFilePath);

            lines[0] = $"username={newUsername}";
            lines[1] = $"password={newPassword}";

            bool emailExists = lines.Any(line => line.StartsWith("email="));

            if (!string.IsNullOrWhiteSpace(newEmail))
            {
                if (emailExists)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].StartsWith("email="))
                        {
                            lines[i] = $"email={newEmail}";
                            break;
                        }
                    }
                }
                else
                {
                    lines = lines.Append($"email={newEmail}").ToArray();
                }
            }
            else if (emailExists) // If email exists but user cleared it, remove it.
            {
                lines = lines.Where(line => !line.StartsWith("email=")).ToArray();
            }

            File.WriteAllLines(configFilePath, lines);
        }

    }
}
