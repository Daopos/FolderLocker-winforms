namespace UCUFolderLocker
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panelSidebar = new Panel();
            btnPermanentLockForm = new Button();
            btnInfoForm = new Button();
            btnHelpForm = new Button();
            btnAllForm = new Button();
            btnLockForm = new Button();
            panelContent = new Panel();
            panelSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(44, 62, 80);
            panelSidebar.Controls.Add(btnPermanentLockForm);
            panelSidebar.Controls.Add(btnInfoForm);
            panelSidebar.Controls.Add(btnHelpForm);
            panelSidebar.Controls.Add(btnAllForm);
            panelSidebar.Controls.Add(btnLockForm);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Margin = new Padding(3, 2, 3, 2);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(201, 450);
            panelSidebar.TabIndex = 0;
            // 
            // btnPermanentLockForm
            // 
            btnPermanentLockForm.BackColor = Color.FromArgb(52, 73, 94);
            btnPermanentLockForm.FlatAppearance.BorderSize = 0;
            btnPermanentLockForm.FlatStyle = FlatStyle.Flat;
            btnPermanentLockForm.ForeColor = Color.White;
            btnPermanentLockForm.Image = Properties.Resources._lock;
            btnPermanentLockForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnPermanentLockForm.Location = new Point(9, 132);
            btnPermanentLockForm.Margin = new Padding(3, 2, 3, 2);
            btnPermanentLockForm.Name = "btnPermanentLockForm";
            btnPermanentLockForm.Size = new Size(184, 38);
            btnPermanentLockForm.TabIndex = 7;
            btnPermanentLockForm.Text = "  Lock Folder";
            btnPermanentLockForm.UseVisualStyleBackColor = false;
            btnPermanentLockForm.Click += btnPermanentLockForm_Click;
            // 
            // btnInfoForm
            // 
            btnInfoForm.BackColor = Color.FromArgb(52, 73, 94);
            btnInfoForm.FlatAppearance.BorderSize = 0;
            btnInfoForm.FlatStyle = FlatStyle.Flat;
            btnInfoForm.ForeColor = Color.White;
            btnInfoForm.Image = Properties.Resources.reset_password;
            btnInfoForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnInfoForm.Location = new Point(9, 368);
            btnInfoForm.Margin = new Padding(3, 2, 3, 2);
            btnInfoForm.Name = "btnInfoForm";
            btnInfoForm.Size = new Size(184, 38);
            btnInfoForm.TabIndex = 6;
            btnInfoForm.Text = "Change Login Info";
            btnInfoForm.UseVisualStyleBackColor = false;
            btnInfoForm.Click += btnInfo_Click;
            // 
            // btnHelpForm
            // 
            btnHelpForm.BackColor = Color.FromArgb(52, 73, 94);
            btnHelpForm.FlatAppearance.BorderSize = 0;
            btnHelpForm.FlatStyle = FlatStyle.Flat;
            btnHelpForm.ForeColor = Color.White;
            btnHelpForm.Image = Properties.Resources.help;
            btnHelpForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnHelpForm.Location = new Point(9, 320);
            btnHelpForm.Margin = new Padding(3, 2, 3, 2);
            btnHelpForm.Name = "btnHelpForm";
            btnHelpForm.Size = new Size(184, 38);
            btnHelpForm.TabIndex = 5;
            btnHelpForm.Text = "User Guide";
            btnHelpForm.UseVisualStyleBackColor = false;
            btnHelpForm.Click += btnHelpForm_Click;
            // 
            // btnAllForm
            // 
            btnAllForm.BackColor = Color.FromArgb(52, 73, 94);
            btnAllForm.FlatAppearance.BorderSize = 0;
            btnAllForm.FlatStyle = FlatStyle.Flat;
            btnAllForm.ForeColor = Color.White;
            btnAllForm.Image = (Image)resources.GetObject("btnAllForm.Image");
            btnAllForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnAllForm.Location = new Point(9, 22);
            btnAllForm.Margin = new Padding(3, 2, 3, 2);
            btnAllForm.Name = "btnAllForm";
            btnAllForm.Size = new Size(184, 38);
            btnAllForm.TabIndex = 1;
            btnAllForm.Text = "  View Locked Folders";
            btnAllForm.TextAlign = ContentAlignment.MiddleLeft;
            btnAllForm.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAllForm.UseVisualStyleBackColor = false;
            btnAllForm.Click += btnAllForm_Click;
            // 
            // btnLockForm
            // 
            btnLockForm.BackColor = Color.FromArgb(52, 73, 94);
            btnLockForm.FlatAppearance.BorderSize = 0;
            btnLockForm.FlatStyle = FlatStyle.Flat;
            btnLockForm.ForeColor = Color.White;
            btnLockForm.Image = Properties.Resources.encrypted_data;
            btnLockForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnLockForm.Location = new Point(9, 75);
            btnLockForm.Margin = new Padding(3, 2, 3, 2);
            btnLockForm.Name = "btnLockForm";
            btnLockForm.Size = new Size(184, 38);
            btnLockForm.TabIndex = 2;
            btnLockForm.Text = "  Encrypt Files";
            btnLockForm.UseVisualStyleBackColor = false;
            btnLockForm.Click += btnLockForm_Click;
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(201, 0);
            panelContent.Margin = new Padding(3, 2, 3, 2);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(685, 450);
            panelContent.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(886, 450);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UCU Folder Locker";
            panelSidebar.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Panel panelSidebar;
        private Panel panelContent;
        private Button btnAllForm;
        private Button btnLockForm;
        private Button btnHelpForm;
        private Button btnInfoForm;
        private Button btnPermanentLockForm;
    }
}
