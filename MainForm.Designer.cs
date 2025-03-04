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
            btnForgotForm = new Button();
            btnAllForm = new Button();
            btnLockForm = new Button();
            btnUnlockForm = new Button();
            panelContent = new Panel();
            panelSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(44, 62, 80);
            panelSidebar.Controls.Add(btnForgotForm);
            panelSidebar.Controls.Add(btnAllForm);
            panelSidebar.Controls.Add(btnLockForm);
            panelSidebar.Controls.Add(btnUnlockForm);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(230, 511);
            panelSidebar.TabIndex = 0;
            // 
            // btnForgotForm
            // 
            btnForgotForm.BackColor = Color.FromArgb(52, 73, 94);
            btnForgotForm.FlatAppearance.BorderSize = 0;
            btnForgotForm.FlatStyle = FlatStyle.Flat;
            btnForgotForm.ForeColor = Color.White;
            btnForgotForm.Image = (Image)resources.GetObject("btnForgotForm.Image");
            btnForgotForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnForgotForm.Location = new Point(10, 238);
            btnForgotForm.Name = "btnForgotForm";
            btnForgotForm.Size = new Size(210, 50);
            btnForgotForm.TabIndex = 4;
            btnForgotForm.Text = "  Forgot Password";
            btnForgotForm.UseVisualStyleBackColor = false;
            btnForgotForm.Click += btnForgotForm_Click;
            // 
            // btnAllForm
            // 
            btnAllForm.BackColor = Color.FromArgb(52, 73, 94);
            btnAllForm.FlatAppearance.BorderSize = 0;
            btnAllForm.FlatStyle = FlatStyle.Flat;
            btnAllForm.ForeColor = Color.White;
            btnAllForm.Image = (Image)resources.GetObject("btnAllForm.Image");
            btnAllForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnAllForm.Location = new Point(10, 30);
            btnAllForm.Name = "btnAllForm";
            btnAllForm.Size = new Size(210, 50);
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
            btnLockForm.Image = Properties.Resources._lock;
            btnLockForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnLockForm.Location = new Point(10, 100);
            btnLockForm.Name = "btnLockForm";
            btnLockForm.Size = new Size(210, 50);
            btnLockForm.TabIndex = 2;
            btnLockForm.Text = "  Lock Folder";
            btnLockForm.UseVisualStyleBackColor = false;
            btnLockForm.Click += btnLockForm_Click;
            // 
            // btnUnlockForm
            // 
            btnUnlockForm.BackColor = Color.FromArgb(52, 73, 94);
            btnUnlockForm.FlatAppearance.BorderSize = 0;
            btnUnlockForm.FlatStyle = FlatStyle.Flat;
            btnUnlockForm.ForeColor = Color.White;
            btnUnlockForm.Image = Properties.Resources.unlocked;
            btnUnlockForm.ImageAlign = ContentAlignment.MiddleLeft;
            btnUnlockForm.Location = new Point(10, 170);
            btnUnlockForm.Name = "btnUnlockForm";
            btnUnlockForm.Size = new Size(210, 50);
            btnUnlockForm.TabIndex = 3;
            btnUnlockForm.Text = "  Unlock Folder";
            btnUnlockForm.UseVisualStyleBackColor = false;
            btnUnlockForm.Click += btnUnlockForm_Click;
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(230, 0);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(783, 511);
            panelContent.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1013, 511);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private Button btnUnlockForm;
        private Button btnForgotForm;
    }
}
