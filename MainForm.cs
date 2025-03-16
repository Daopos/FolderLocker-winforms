using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace UCUFolderLocker
{
    public partial class MainForm : Form
    {
        private Color defaultColor = Color.FromArgb(52, 73, 94);
        private Button activeButton;

        // Store form instances
        private Form allForm;
        private Form lockForm;
        private Form unlockForm;
        private Form forgotForm;
        private Form helpForm;
            

           
        public MainForm(string lockFilePath = null)
        {

            InitializeComponent();

            if (!string.IsNullOrEmpty(lockFilePath))
            {
                // Automatically handle the .lock file
                HandleLockFile(lockFilePath);
            }
            // Initialize forms once
            allForm = new All();
            lockForm = new Lock();
            unlockForm = new UnlockForm();
            forgotForm = new ForgotForm();
            helpForm = new HelpForm();


            LoadForm(allForm, btnAllForm); // Default load
        }

        private void HandleLockFile(string lockFilePath)
        {
            // Switch to the unlock tab if you have tab control
            // tabControl.SelectedTab = unlockTab;

            // Set the file path in your UI
            // txtUnlockFilePath.Text = lockFilePath;

            // You could even show the password prompt automatically
            // MessageBox.Show("Please enter your password to unlock this file", "Unlock File");
        }

        // Your existing form code...

        private void LoadForm(Form form, Button clickedButton)
        {
            panelContent.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            panelContent.Controls.Add(form);
            form.Show();

            // Change button color
            SetActiveButton(clickedButton);
        }

        private void SetActiveButton(Button clickedButton)
        {
            if (activeButton != null)
            {
                activeButton.BackColor = defaultColor;
                activeButton.ForeColor = Color.White;
            }

            clickedButton.BackColor = Color.LightBlue;
            clickedButton.ForeColor = Color.Black;
            activeButton = clickedButton;
        }

        private void btnAllForm_Click(object sender, EventArgs e)
        {
            LoadForm(allForm, btnAllForm); // Use stored instance
        }

        private void btnLockForm_Click(object sender, EventArgs e)
        {
            LoadForm(lockForm, btnLockForm); // Use stored instance
        }

        private void btnUnlockForm_Click(object sender, EventArgs e)
        {
            LoadForm(unlockForm, btnUnlockForm); // Use stored instance
        }

        private void btnForgotForm_Click(object sender, EventArgs e)
        {
            LoadForm(forgotForm, btnForgotForm); // Use stored instance
        }

        private void btnHelpForm_Click(object sender, EventArgs e)
        {
            LoadForm(helpForm, btnHelpForm); // Use stored instance
        }


    }
}
