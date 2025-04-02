namespace UCUFolderLocker
{
    partial class PermanentUnlockForm
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
            txtPassword = new TextBox();
            labelPassword = new Label();
            btnUnlockFolder = new Button();
            btnTogglePassword = new PictureBox();
            progressBarLoading = new ProgressBar();
            lblStatus = new Label();
            labelPath = new Label();
            lblFolderPath = new TextBox();
            buttonBrowse = new Button();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            SuspendLayout();
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Consolas", 12F);
            txtPassword.Location = new Point(119, 47);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(324, 26);
            txtPassword.TabIndex = 19;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 12F);
            labelPassword.Location = new Point(14, 50);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(79, 21);
            labelPassword.TabIndex = 18;
            labelPassword.Text = "Password:";
            // 
            // btnUnlockFolder
            // 
            btnUnlockFolder.BackColor = Color.FromArgb(62, 123, 39);
            btnUnlockFolder.FlatStyle = FlatStyle.Flat;
            btnUnlockFolder.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUnlockFolder.ForeColor = Color.White;
            btnUnlockFolder.Location = new Point(119, 105);
            btnUnlockFolder.Margin = new Padding(3, 2, 3, 2);
            btnUnlockFolder.Name = "btnUnlockFolder";
            btnUnlockFolder.Size = new Size(324, 38);
            btnUnlockFolder.TabIndex = 17;
            btnUnlockFolder.Text = "Unlock Folder";
            btnUnlockFolder.UseVisualStyleBackColor = false;
            btnUnlockFolder.Click += btnUnlockFolder_Click;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(448, 52);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 24);
            btnTogglePassword.TabIndex = 26;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(119, 152);
            progressBarLoading.Margin = new Padding(3, 2, 3, 2);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(324, 22);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 25;
            progressBarLoading.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F);
            lblStatus.Location = new Point(14, 80);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(198, 20);
            lblStatus.TabIndex = 24;
            lblStatus.Text = "Status: Recvoery Successfully";
            lblStatus.Visible = false;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(14, 14);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(91, 21);
            labelPath.TabIndex = 21;
            labelPath.Text = "Folder Path:";
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(119, 11);
            lblFolderPath.Margin = new Padding(3, 2, 3, 2);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(324, 29);
            lblFolderPath.TabIndex = 22;
            // 
            // buttonBrowse
            // 
            buttonBrowse.BackColor = Color.FromArgb(200, 76, 5);
            buttonBrowse.FlatStyle = FlatStyle.Flat;
            buttonBrowse.Font = new Font("Segoe UI", 11F);
            buttonBrowse.ForeColor = Color.White;
            buttonBrowse.Location = new Point(476, 10);
            buttonBrowse.Margin = new Padding(3, 2, 3, 2);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(94, 30);
            buttonBrowse.TabIndex = 23;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnSelectFolder_Click;
            // 
            // PermanentUnlockForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtPassword);
            Controls.Add(labelPassword);
            Controls.Add(btnUnlockFolder);
            Controls.Add(btnTogglePassword);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            Name = "PermanentUnlockForm";
            Text = "PermanentUnlockForm";
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtPassword;
        private Label labelPassword;
        private Button btnUnlockFolder;
        private PictureBox btnTogglePassword;
        private ProgressBar progressBarLoading;
        private Label lblStatus;
        private Label labelPath;
        private TextBox lblFolderPath;
        private Button buttonBrowse;
    }
}