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
        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadForm(Form form)
        {
            panelContent.Controls.Clear(); // Clear previous content
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None; // Remove borders
            panelContent.Controls.Add(form);
            form.Show();
        }

      


        private void btnLockForm_Click(object sender, EventArgs e)
        {
            LoadForm(new Lock()); // Load Form1 inside panelContent

        }

        private void btnAllForm_Click(object sender, EventArgs e)
        {
            LoadForm(new All()); // Load Form1 inside panelContent

        }
    }
}
