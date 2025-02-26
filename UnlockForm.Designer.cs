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
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonUnlock
            // 
            buttonUnlock.Location = new Point(230, 174);
            buttonUnlock.Name = "buttonUnlock";
            buttonUnlock.Size = new Size(120, 35);
            buttonUnlock.TabIndex = 0;
            buttonUnlock.Text = "Unlock Folder";
            buttonUnlock.UseVisualStyleBackColor = true;
            buttonUnlock.Click += btnUnlock_Click;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(20, 127);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(73, 20);
            labelPassword.TabIndex = 1;
            labelPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 124);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(370, 27);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(731, 63);
            panel1.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Popup;
            label1.Font = new Font("Segoe UI", 19.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 8);
            label1.Name = "label1";
            label1.Size = new Size(317, 45);
            label1.TabIndex = 0;
            label1.Text = "UNLOCK A FOLDER";
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Location = new Point(20, 82);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(86, 20);
            labelPath.TabIndex = 10;
            labelPath.Text = "Folder Path:";
            // 
            // lblFolderPath
            // 
            lblFolderPath.Location = new Point(120, 79);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(370, 27);
            lblFolderPath.TabIndex = 11;
            // 
            // buttonBrowse
            // 
            buttonBrowse.ForeColor = Color.Black;
            buttonBrowse.Location = new Point(500, 77);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(90, 30);
            buttonBrowse.TabIndex = 12;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // UnlockForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(731, 300);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            Controls.Add(panel1);
            Controls.Add(txtPassword);
            Controls.Add(labelPassword);
            Controls.Add(buttonUnlock);
            Name = "UnlockForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Unlock Folder";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
    }
}
