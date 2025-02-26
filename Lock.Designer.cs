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
            textBoxRecovery = new TextBox();
            buttonLock = new Button();
            panel1 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(498, 85);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(90, 30);
            buttonBrowse.TabIndex = 9;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // lblFolderPath
            // 
            lblFolderPath.Location = new Point(118, 87);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(370, 27);
            lblFolderPath.TabIndex = 8;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Location = new Point(18, 90);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(86, 20);
            labelPath.TabIndex = 7;
            labelPath.Text = "Folder Path:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(18, 130);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(73, 20);
            labelPassword.TabIndex = 6;
            labelPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(118, 127);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(370, 27);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // labelConfirmPassword
            // 
            labelConfirmPassword.AutoSize = true;
            labelConfirmPassword.Location = new Point(18, 170);
            labelConfirmPassword.Name = "labelConfirmPassword";
            labelConfirmPassword.Size = new Size(130, 20);
            labelConfirmPassword.TabIndex = 4;
            labelConfirmPassword.Text = "Confirm Password:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(158, 167);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(330, 27);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // labelRecovery
            // 
            labelRecovery.AutoSize = true;
            labelRecovery.Location = new Point(18, 210);
            labelRecovery.Name = "labelRecovery";
            labelRecovery.Size = new Size(160, 20);
            labelRecovery.TabIndex = 2;
            labelRecovery.Text = "Recovery Email/Phone:";
            // 
            // textBoxRecovery
            // 
            textBoxRecovery.Location = new Point(178, 207);
            textBoxRecovery.Name = "textBoxRecovery";
            textBoxRecovery.Size = new Size(310, 27);
            textBoxRecovery.TabIndex = 1;
            // 
            // buttonLock
            // 
            buttonLock.Location = new Point(228, 255);
            buttonLock.Name = "buttonLock";
            buttonLock.Size = new Size(120, 35);
            buttonLock.TabIndex = 0;
            buttonLock.Text = "Lock Folder";
            buttonLock.UseVisualStyleBackColor = true;
            buttonLock.Click += btnLock_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(783, 63);
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
            // Lock
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(783, 437);
            Controls.Add(panel1);
            Controls.Add(buttonLock);
            Controls.Add(textBoxRecovery);
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
        private TextBox textBoxRecovery;
        private Button buttonLock;
        private Panel panel1;
        private Label label1;
    }
}