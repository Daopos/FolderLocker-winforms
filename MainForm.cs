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
        public MainForm()
        {
            InitializeComponent();
            LoadForm(new All(), btnAllForm); // Default load with Lock form
        }

        private void LoadForm(Form form, Button clickedButton)
        {
            panelContent.Controls.Clear(); // Clear previous content
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None; // Remove borders
            panelContent.Controls.Add(form);
            form.Show();

            // Change button color
            SetActiveButton(clickedButton);
        }


        private void SetActiveButton(Button clickedButton)
        {
            // Reset previous active button color
            if (activeButton != null)
            {
                activeButton.BackColor = defaultColor; // Default color
                activeButton.ForeColor = Color.White;
            }

            // Set new active button color
            clickedButton.BackColor = Color.LightBlue; // Change to your preferred color
            clickedButton.ForeColor = Color.Black;
            activeButton = clickedButton; // Update active button
        }
        private void btnAllForm_Click(object sender, EventArgs e)
        {
            LoadForm(new All(), btnAllForm);
        }
        private void btnLockForm_Click(object sender, EventArgs e)
        {
            LoadForm(new Lock(), btnLockForm);
        }


        private void btnUnlockForm_Click(object sender, EventArgs e)
        {
            LoadForm(new UnlockForm(), btnUnlockForm);
        }

 

        
    }
}
