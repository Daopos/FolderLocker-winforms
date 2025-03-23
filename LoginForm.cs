using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UCUFolderLocker
{
    public partial class LoginForm : Form
    {
        private string configFilePath = "config.ini"; // File to store credentials
        public LoginForm()
        {
            InitializeComponent();
            LoadCredentials();
        }
        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtPassword.UseSystemPasswordChar ? Properties.Resources.hidden : Properties.Resources.eye;
        }

        private void LoadCredentials()
        {
            if (!File.Exists(configFilePath))
            {
                // Create file with default credentials
                File.WriteAllText(configFilePath, "username=admin\npassword=admin123");
            }
        }
        private bool ValidateLogin(string username, string password)
        {
            string[] lines = File.ReadAllLines(configFilePath);
            string storedUsername = lines[0].Split('=')[1];
            string storedPassword = lines[1].Split('=')[1];

            return username == storedUsername && password == storedPassword;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (ValidateLogin(username, password))
            {
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close(); // Close login form after MainForm closes
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Creating ToolTip instance
            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();

            // Setting tooltip messages for controls
            toolTip.SetToolTip(usernameInfo, "default: admin");
            toolTip.SetToolTip(passwordInfo, "default: admin123");
        }

        private void lblForgot_Click(object sender, EventArgs e)
        {
            Form2 forgotPasswordForm = new Form2();
            forgotPasswordForm.ShowDialog(); // Show modal
        }

    }
}
