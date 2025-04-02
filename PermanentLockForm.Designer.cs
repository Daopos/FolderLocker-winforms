namespace UCUFolderLocker
{
    partial class PermanentLockForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnTogglePassword = new PictureBox();
            progressBarLoading = new ProgressBar();
            lblStatus = new Label();
            btnToggleConfirmPassword = new PictureBox();
            buttonLock = new Button();
            txtEmail = new TextBox();
            labelRecovery = new Label();
            txtConfirmPassword = new TextBox();
            labelConfirmPassword = new Label();
            txtPassword = new TextBox();
            labelPassword = new Label();
            labelPath = new Label();
            lblFolderPath = new TextBox();
            buttonBrowse = new Button();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnToggleConfirmPassword).BeginInit();
            SuspendLayout();
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(496, 49);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 25);
            btnTogglePassword.TabIndex = 29;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(168, 255);
            progressBarLoading.Margin = new Padding(3, 2, 3, 2);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(324, 22);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 28;
            progressBarLoading.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F);
            lblStatus.Location = new Point(12, 179);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(202, 20);
            lblStatus.TabIndex = 27;
            lblStatus.Text = "Status: Recovery Successfully!";
            lblStatus.Visible = false;
            // 
            // btnToggleConfirmPassword
            // 
            btnToggleConfirmPassword.Image = Properties.Resources.hidden;
            btnToggleConfirmPassword.Location = new Point(496, 84);
            btnToggleConfirmPassword.Margin = new Padding(3, 2, 3, 2);
            btnToggleConfirmPassword.Name = "btnToggleConfirmPassword";
            btnToggleConfirmPassword.Size = new Size(23, 26);
            btnToggleConfirmPassword.TabIndex = 30;
            btnToggleConfirmPassword.TabStop = false;
            btnToggleConfirmPassword.Click += btnToggleConfirmPassword_Click;
            // 
            // buttonLock
            // 
            buttonLock.BackColor = Color.FromArgb(153, 0, 0);
            buttonLock.FlatStyle = FlatStyle.Flat;
            buttonLock.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            buttonLock.ForeColor = Color.White;
            buttonLock.Location = new Point(168, 208);
            buttonLock.Margin = new Padding(3, 2, 3, 2);
            buttonLock.Name = "buttonLock";
            buttonLock.Size = new Size(324, 43);
            buttonLock.TabIndex = 16;
            buttonLock.Text = "Lock Folder";
            buttonLock.UseVisualStyleBackColor = false;
            buttonLock.Click += btnLockFolder_Click;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 12F);
            txtEmail.Location = new Point(168, 119);
            txtEmail.Margin = new Padding(3, 2, 3, 2);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(324, 29);
            txtEmail.TabIndex = 17;
            // 
            // labelRecovery
            // 
            labelRecovery.AutoSize = true;
            labelRecovery.Font = new Font("Segoe UI", 12F);
            labelRecovery.Location = new Point(12, 119);
            labelRecovery.Name = "labelRecovery";
            labelRecovery.Size = new Size(119, 21);
            labelRecovery.TabIndex = 18;
            labelRecovery.Text = "Recovery Email:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 12F);
            txtConfirmPassword.Location = new Point(168, 81);
            txtConfirmPassword.Margin = new Padding(3, 2, 3, 2);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(324, 29);
            txtConfirmPassword.TabIndex = 19;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // labelConfirmPassword
            // 
            labelConfirmPassword.AutoSize = true;
            labelConfirmPassword.Font = new Font("Segoe UI", 12F);
            labelConfirmPassword.Location = new Point(12, 81);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(140, 21);
            labelConfirmPassword.TabIndex = 20;
            labelConfirmPassword.Text = "Confirm Password:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(168, 45);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(324, 29);
            txtPassword.TabIndex = 21;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 12F);
            labelPassword.Location = new Point(12, 45);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(79, 21);
            labelPassword.TabIndex = 22;
            labelPassword.Text = "Password:";
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(12, 9);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(91, 21);
            labelPath.TabIndex = 23;
            labelPath.Text = "Folder Path:";
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(168, 9);
            lblFolderPath.Margin = new Padding(3, 2, 3, 2);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(324, 29);
            lblFolderPath.TabIndex = 24;
            // 
            // buttonBrowse
            // 
            buttonBrowse.BackColor = Color.FromArgb(200, 76, 5);
            buttonBrowse.FlatStyle = FlatStyle.Flat;
            buttonBrowse.Font = new Font("Segoe UI", 11F);
            buttonBrowse.ForeColor = Color.White;
            buttonBrowse.Location = new Point(506, 5);
            buttonBrowse.Margin = new Padding(3, 2, 3, 2);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(94, 28);
            buttonBrowse.TabIndex = 25;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnSelectFolder_Click;
            // 
            // PermanentLockForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnTogglePassword);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(btnToggleConfirmPassword);
            Controls.Add(buttonLock);
            Controls.Add(txtEmail);
            Controls.Add(labelRecovery);
            Controls.Add(txtConfirmPassword);
            Controls.Add(labelConfirmPassword);
            Controls.Add(txtPassword);
            Controls.Add(labelPassword);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            Name = "PermanentLockForm";
            Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnToggleConfirmPassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox btnTogglePassword;
        private ProgressBar progressBarLoading;
        private Label lblStatus;
        private PictureBox btnToggleConfirmPassword;
        private Button buttonLock;
        private TextBox txtEmail;
        private Label labelRecovery;
        private TextBox txtConfirmPassword;
        private Label labelConfirmPassword;
        private TextBox txtPassword;
        private Label labelPassword;
        private Label labelPath;
        private TextBox lblFolderPath;
        private Button buttonBrowse;
    }
}