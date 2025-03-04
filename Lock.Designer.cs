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
            panel1 = new Panel();
            label1 = new Label();
            lblStatus = new Label();
            progressBarLoading = new ProgressBar();
            btnTogglePassword = new PictureBox();
            btnToggleConfirmPassword = new PictureBox();
            panel1.SuspendLayout();
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
            buttonBrowse.Location = new Point(583, 86);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(108, 38);
            buttonBrowse.TabIndex = 9;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(196, 90);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(370, 34);
            lblFolderPath.TabIndex = 8;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(18, 90);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(115, 28);
            labelPath.TabIndex = 7;
            labelPath.Text = "Folder Path:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 12F);
            labelPassword.Location = new Point(18, 139);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(97, 28);
            labelPassword.TabIndex = 6;
            labelPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(196, 139);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(370, 34);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // labelConfirmPassword
            // 
            labelConfirmPassword.AutoSize = true;
            labelConfirmPassword.Font = new Font("Segoe UI", 12F);
            labelConfirmPassword.Location = new Point(18, 186);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(172, 28);
            labelConfirmPassword.TabIndex = 4;
            labelConfirmPassword.Text = "Confirm Password:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 12F);
            txtConfirmPassword.Location = new Point(196, 186);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(370, 34);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.TextChanged += txtConfirmPassword_TextChanged;
            // 
            // labelRecovery
            // 
            labelRecovery.AutoSize = true;
            labelRecovery.Font = new Font("Segoe UI", 12F);
            labelRecovery.Location = new Point(18, 238);
            labelRecovery.Name = "labelRecovery";
            labelRecovery.Size = new Size(147, 28);
            labelRecovery.TabIndex = 2;
            labelRecovery.Text = "Recovery Email:";
            // 
            // txtRecoveryEmail
            // 
            txtRecoveryEmail.Font = new Font("Segoe UI", 12F);
            txtRecoveryEmail.Location = new Point(196, 238);
            txtRecoveryEmail.Name = "txtRecoveryEmail";
            txtRecoveryEmail.Size = new Size(370, 34);
            txtRecoveryEmail.TabIndex = 1;
            // 
            // buttonLock
            // 
            buttonLock.BackColor = Color.FromArgb(153, 0, 0);
            buttonLock.FlatStyle = FlatStyle.Flat;
            buttonLock.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonLock.ForeColor = Color.White;
            buttonLock.Location = new Point(196, 321);
            buttonLock.Name = "buttonLock";
            buttonLock.Size = new Size(370, 57);
            buttonLock.TabIndex = 0;
            buttonLock.Text = "Lock Folder";
            buttonLock.UseVisualStyleBackColor = false;
            buttonLock.Click += btnLock_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(765, 63);
            panel1.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Popup;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 8);
            label1.Name = "label1";
            label1.Size = new Size(276, 46);
            label1.TabIndex = 0;
            label1.Text = "LOCK A FOLDER";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F);
            lblStatus.Location = new Point(18, 283);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(257, 25);
            lblStatus.TabIndex = 11;
            lblStatus.Text = "Status: Recovery Successfully!";
            lblStatus.Visible = false;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(196, 384);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(370, 29);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 12;
            progressBarLoading.Visible = false;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.eye;
            btnTogglePassword.Location = new Point(572, 144);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(26, 23);
            btnTogglePassword.TabIndex = 13;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // btnToggleConfirmPassword
            // 
            btnToggleConfirmPassword.Image = Properties.Resources.eye;
            btnToggleConfirmPassword.Location = new Point(572, 191);
            btnToggleConfirmPassword.Name = "btnToggleConfirmPassword";
            btnToggleConfirmPassword.Size = new Size(26, 23);
            btnToggleConfirmPassword.TabIndex = 14;
            btnToggleConfirmPassword.TabStop = false;
            btnToggleConfirmPassword.Click += btnToggleConfirmPassword_Click;
            // 
            // Lock
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(765, 464);
            Controls.Add(btnToggleConfirmPassword);
            Controls.Add(btnTogglePassword);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(panel1);
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
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Lock";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Folder Lock";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Panel panel1;
        private Label label1;
        private Label lblStatus;
        private ProgressBar progressBarLoading;
        private PictureBox btnTogglePassword;
        private PictureBox btnToggleConfirmPassword;
    }
}