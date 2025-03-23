namespace UCUFolderLocker
{
    partial class UnlockForm
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

        private void InitializeComponent()
        {
            buttonUnlock = new Button();
            labelPassword = new Label();
            txtPassword = new TextBox();
            panel1 = new Panel();
            label1 = new Label();
            labelPath = new Label();
            lblFolderPath = new TextBox();
            buttonBrowse = new Button();
            progressBarLoading = new ProgressBar();
            lblStatus = new Label();
            btnTogglePassword = new PictureBox();
            button1 = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            SuspendLayout();
            // 
            // buttonUnlock
            // 
            buttonUnlock.BackColor = Color.FromArgb(62, 123, 39);
            buttonUnlock.FlatStyle = FlatStyle.Flat;
            buttonUnlock.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonUnlock.ForeColor = Color.White;
            buttonUnlock.Location = new Point(123, 153);
            buttonUnlock.Margin = new Padding(3, 2, 3, 2);
            buttonUnlock.Name = "buttonUnlock";
            buttonUnlock.Size = new Size(324, 38);
            buttonUnlock.TabIndex = 0;
            buttonUnlock.Text = "Unlock Folder";
            buttonUnlock.UseVisualStyleBackColor = false;
            buttonUnlock.Click += btnUnlock_Click;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 12F);
            labelPassword.Location = new Point(18, 98);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(79, 21);
            labelPassword.TabIndex = 1;
            labelPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(123, 95);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(324, 29);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(669, 47);
            panel1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Popup;
            label1.Font = new Font("Segoe UI", 19.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 6);
            label1.Name = "label1";
            label1.Size = new Size(260, 37);
            label1.TabIndex = 0;
            label1.Text = "UNLOCK A FOLDER";
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(18, 62);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(91, 21);
            labelPath.TabIndex = 10;
            labelPath.Text = "Folder Path:";
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(123, 59);
            lblFolderPath.Margin = new Padding(3, 2, 3, 2);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(324, 29);
            lblFolderPath.TabIndex = 11;
            // 
            // buttonBrowse
            // 
            buttonBrowse.BackColor = Color.FromArgb(200, 76, 5);
            buttonBrowse.FlatStyle = FlatStyle.Flat;
            buttonBrowse.Font = new Font("Segoe UI", 11F);
            buttonBrowse.ForeColor = Color.White;
            buttonBrowse.Location = new Point(480, 58);
            buttonBrowse.Margin = new Padding(3, 2, 3, 2);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(94, 27);
            buttonBrowse.TabIndex = 12;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(123, 200);
            progressBarLoading.Margin = new Padding(3, 2, 3, 2);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(324, 22);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 14;
            progressBarLoading.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F);
            lblStatus.Location = new Point(18, 128);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(198, 20);
            lblStatus.TabIndex = 13;
            lblStatus.Text = "Status: Recvoery Successfully";
            lblStatus.Visible = false;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(452, 100);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 24);
            btnTogglePassword.TabIndex = 15;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(217, 131, 36);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(480, 94);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(150, 28);
            button1.TabIndex = 16;
            button1.Text = "ChangePassword";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnChangePassword_Click;
            // 
            // UnlockForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(669, 348);
            Controls.Add(button1);
            Controls.Add(btnTogglePassword);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            Controls.Add(panel1);
            Controls.Add(txtPassword);
            Controls.Add(labelPassword);
            Controls.Add(buttonUnlock);
            Margin = new Padding(3, 2, 3, 2);
            Name = "UnlockForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Unlock Folder";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button buttonUnlock;
        private Label labelPassword;
        private TextBox txtPassword;
        private Panel panel1;
        private Label label1;
        private Label labelPath;
        private TextBox lblFolderPath;
        private Button buttonBrowse;
        private ProgressBar progressBarLoading;
        private Label lblStatus;
        private PictureBox btnTogglePassword;
        private Button button1;
    }
}
