using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class ChangePasswordForm : Form
    {
        public string NewPassword { get; private set; }

        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void btnTogglePassword_Click(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !txtNewPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtNewPassword.UseSystemPasswordChar ? Properties.Resources.eye : Properties.Resources.hidden;
        }

        private void btnToggleConfirmPassword_Click(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !txtConfirmPassword.UseSystemPasswordChar;
            btnTogglePassword.Image = txtConfirmPassword.UseSystemPasswordChar ? Properties.Resources.eye : Properties.Resources.hidden;
        }

        private void btnChangePasswords_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NewPassword = txtNewPassword.Text;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
