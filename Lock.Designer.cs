namespace UCUFolderLocker
{
    partial class Lock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            buttonBrowse = new Button();
            lblFolderPath = new TextBox();
            labelPath = new Label();
            labelPassword = new Label();
            txtPassword = new TextBox();
            labelConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            labelRecovery = new Label();
            txtRecoveryEmail = new TextBox();
            buttonLock = new Button();
            lblStatus = new Label();
            progressBarLoading = new ProgressBar();
            btnTogglePassword = new PictureBox();
            btnToggleConfirmPassword = new PictureBox();
            chkNTFSProtection = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnToggleConfirmPassword).BeginInit();
            SuspendLayout();
            // 
            // buttonBrowse
            // 
            buttonBrowse.BackColor = Color.FromArgb(200, 76, 5);
            buttonBrowse.FlatStyle = FlatStyle.Flat;
            buttonBrowse.Font = new Font("Segoe UI", 11F);
            buttonBrowse.ForeColor = Color.White;
            buttonBrowse.Location = new Point(506, 7);
            buttonBrowse.Margin = new Padding(3, 2, 3, 2);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(94, 28);
            buttonBrowse.TabIndex = 9;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(168, 11);
            lblFolderPath.Margin = new Padding(3, 2, 3, 2);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(324, 29);
            lblFolderPath.TabIndex = 1;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(12, 11);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(91, 21);
            labelPath.TabIndex = 7;
            labelPath.Text = "Folder Path:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 12F);
            labelPassword.Location = new Point(12, 47);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(79, 21);
            labelPassword.TabIndex = 6;
            labelPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(168, 47);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(324, 29);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // labelConfirmPassword
            // 
            labelConfirmPassword.AutoSize = true;
            labelConfirmPassword.Font = new Font("Segoe UI", 12F);
            labelConfirmPassword.Location = new Point(12, 83);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(140, 21);
            labelConfirmPassword.TabIndex = 4;
            labelConfirmPassword.Text = "Confirm Password:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 12F);
            txtConfirmPassword.Location = new Point(168, 83);
            txtConfirmPassword.Margin = new Padding(3, 2, 3, 2);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(324, 29);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // labelRecovery
            // 
            labelRecovery.AutoSize = true;
            labelRecovery.Font = new Font("Segoe UI", 12F);
            labelRecovery.Location = new Point(12, 121);
            labelRecovery.Name = "labelRecovery";
            labelRecovery.Size = new Size(119, 21);
            labelRecovery.TabIndex = 2;
            labelRecovery.Text = "Recovery Email:";
            // 
            // txtRecoveryEmail
            // 
            txtRecoveryEmail.Font = new Font("Segoe UI", 12F);
            txtRecoveryEmail.Location = new Point(168, 121);
            txtRecoveryEmail.Margin = new Padding(3, 2, 3, 2);
            txtRecoveryEmail.Name = "txtRecoveryEmail";
            txtRecoveryEmail.Size = new Size(324, 29);
            txtRecoveryEmail.TabIndex = 4;
            // 
            // buttonLock
            // 
            buttonLock.BackColor = Color.FromArgb(153, 0, 0);
            buttonLock.FlatStyle = FlatStyle.Flat;
            buttonLock.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonLock.ForeColor = Color.White;
            buttonLock.Location = new Point(168, 210);
            buttonLock.Margin = new Padding(3, 2, 3, 2);
            buttonLock.Name = "buttonLock";
            buttonLock.Size = new Size(324, 43);
            buttonLock.TabIndex = 6;
            buttonLock.Text = "Lock Folder";
            buttonLock.UseVisualStyleBackColor = false;
            buttonLock.Click += btnLock_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F);
            lblStatus.Location = new Point(12, 181);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(202, 20);
            lblStatus.TabIndex = 11;
            lblStatus.Text = "Status: Recovery Successfully!";
            lblStatus.Visible = false;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(168, 257);
            progressBarLoading.Margin = new Padding(3, 2, 3, 2);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(324, 22);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 12;
            progressBarLoading.Visible = false;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(496, 51);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 25);
            btnTogglePassword.TabIndex = 13;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // btnToggleConfirmPassword
            // 
            btnToggleConfirmPassword.Image = Properties.Resources.hidden;
            btnToggleConfirmPassword.Location = new Point(496, 86);
            btnToggleConfirmPassword.Margin = new Padding(3, 2, 3, 2);
            btnToggleConfirmPassword.Name = "btnToggleConfirmPassword";
            btnToggleConfirmPassword.Size = new Size(23, 26);
            btnToggleConfirmPassword.TabIndex = 14;
            btnToggleConfirmPassword.TabStop = false;
            btnToggleConfirmPassword.Click += btnToggleConfirmPassword_Click;
            // 
            // chkNTFSProtection
            // 
            chkNTFSProtection.AutoSize = true;
            chkNTFSProtection.Location = new Point(168, 155);
            chkNTFSProtection.Margin = new Padding(3, 2, 3, 2);
            chkNTFSProtection.Name = "chkNTFSProtection";
            chkNTFSProtection.Size = new Size(111, 19);
            chkNTFSProtection.TabIndex = 5;
            chkNTFSProtection.Text = "NTFS protection";
            chkNTFSProtection.UseVisualStyleBackColor = true;
            // 
            // Lock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(669, 336);
            Controls.Add(chkNTFSProtection);
            Controls.Add(btnToggleConfirmPassword);
            Controls.Add(btnTogglePassword);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(buttonLock);
            Controls.Add(txtRecoveryEmail);
            Controls.Add(labelRecovery);
            Controls.Add(txtConfirmPassword);
            Controls.Add(labelConfirmPassword);
            Controls.Add(txtPassword);
            Controls.Add(labelPassword);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Lock";
            StartPosition = FormStartPosition.CenterScreen;
            Text = " ";
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnToggleConfirmPassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button buttonBrowse;
        private TextBox lblFolderPath;
        private Label labelPath;
        private Label labelPassword;
        private TextBox txtPassword;
        private Label labelConfirmPassword;
        private TextBox txtConfirmPassword;
        private Label labelRecovery;
        private TextBox txtRecoveryEmail;
        private Button buttonLock;
        private Label lblStatus;
        private ProgressBar progressBarLoading;
        private PictureBox btnTogglePassword;
        private PictureBox btnToggleConfirmPassword;
        private CheckBox chkNTFSProtection;
    }
}