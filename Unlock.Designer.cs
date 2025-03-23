namespace UCUFolderLocker
{
    partial class Unlock
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
            btnUnlock = new Button();
            lblStatus = new Label();
            lblFilePath = new Label();
            lblPasswordPrompt = new Label();
            btnTogglePassword = new PictureBox();
            progressBarLoading = new ProgressBar();
            lblPath = new Label();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            SuspendLayout();
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(92, 67);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(304, 23);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnUnlock
            // 
            btnUnlock.BackColor = Color.FromArgb(0, 122, 204);
            btnUnlock.FlatAppearance.BorderSize = 0;
            btnUnlock.FlatStyle = FlatStyle.Flat;
            btnUnlock.ForeColor = Color.White;
            btnUnlock.Location = new Point(92, 110);
            btnUnlock.Name = "btnUnlock";
            btnUnlock.Size = new Size(304, 30);
            btnUnlock.TabIndex = 4;
            btnUnlock.Text = "Unlock";
            btnUnlock.UseVisualStyleBackColor = false;
            btnUnlock.Click += btnUnlock_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.Location = new Point(92, 153);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status";
            lblStatus.Visible = false;
            // 
            // lblFilePath
            // 
            lblFilePath.AutoSize = true;
            lblFilePath.Font = new Font("Segoe UI", 9F);
            lblFilePath.Location = new Point(23, 30);
            lblFilePath.MaximumSize = new Size(410, 0);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(52, 15);
            lblFilePath.TabIndex = 2;
            lblFilePath.Text = "File path";
            // 
            // lblPasswordPrompt
            // 
            lblPasswordPrompt.AutoSize = true;
            lblPasswordPrompt.Location = new Point(23, 70);
            lblPasswordPrompt.Name = "lblPasswordPrompt";
            lblPasswordPrompt.Size = new Size(60, 15);
            lblPasswordPrompt.TabIndex = 6;
            lblPasswordPrompt.Text = "Password:";
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Cursor = Cursors.Hand;
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(402, 67);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(25, 23);
            btnTogglePassword.SizeMode = PictureBoxSizeMode.CenterImage;
            btnTogglePassword.TabIndex = 3;
            btnTogglePassword.TabStop = false;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(92, 180);
            progressBarLoading.MarqueeAnimationSpeed = 30;
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(304, 10);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 0;
            progressBarLoading.Visible = false;
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Font = new Font("Segoe UI", 9F);
            lblPath.Location = new Point(92, 30);
            lblPath.MaximumSize = new Size(410, 0);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(0, 15);
            lblPath.TabIndex = 7;
            // 
            // Unlock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(601, 211);
            Controls.Add(lblPath);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(lblFilePath);
            Controls.Add(btnTogglePassword);
            Controls.Add(btnUnlock);
            Controls.Add(txtPassword);
            Controls.Add(lblPasswordPrompt);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Unlock";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Unlock Folder";
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private TextBox txtPassword;
        private Button btnUnlock;
        private Label lblStatus;
        private Label lblFilePath;
        private Label lblPasswordPrompt;
        private PictureBox btnTogglePassword;
        private ProgressBar progressBarLoading;
        private Label lblPath;
    }
}
