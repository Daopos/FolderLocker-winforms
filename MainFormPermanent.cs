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
    public partial class MainFormPermanent : Form
    {
        private Color defaultColor = Color.FromArgb(52, 73, 94);
        private Button activeButton;

        // Store form instances
        private Form lockForm;
        private Form UnlockForm;
        private Form ForgotForm;



        public MainFormPermanent()
        {
            InitializeComponent();

            // Initialize forms once
            lockForm = new PermanentLockForm();
            UnlockForm = new PermanentUnlockForm();
            ForgotForm = new PermanenForgotForm();


            LoadForm(lockForm, btnLockForm); // Default load

            lblTitle.Text = "LOCK A FOLDER";

        }



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

       
        private void btnLockForm_Click(object sender, EventArgs e)
        {
            LoadForm(lockForm, btnLockForm); // Use stored instance
            lblTitle.Text = "LOCK A FOLDER";

        }
        private void btnUnlockForm_Click(object sender, EventArgs e)
        {
            LoadForm(UnlockForm, btnUnlockForm); // Use stored instance
            lblTitle.Text = "UNLOCK A FOLDER";

        }
        private void btnForgotForm_Click(object sender, EventArgs e)
        {
            LoadForm(ForgotForm, btnForgotForm); // Use stored instance
            lblTitle.Text = "FOLDER RECOVERY";

        }




    }
}