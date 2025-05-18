namespace UCUFolderLocker
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            lblTitle = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            usernameInfo = new PictureBox();
            passwordInfo = new PictureBox();
            btnTogglePassword = new PictureBox();
            lblForgot = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)usernameInfo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)passwordInfo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(118, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(318, 39);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "FOLDER LOCKER";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            lblUsername.Font = new Font("Segoe UI", 12F);
            lblUsername.Location = new Point(66, 88);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(98, 19);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            // 
            // lblPassword
            // 
            lblPassword.Font = new Font("Segoe UI", 12F);
            lblPassword.Location = new Point(74, 133);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(88, 19);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Password:";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 12F);
            txtUsername.Location = new Point(169, 88);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(258, 29);
            txtUsername.TabIndex = 1;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 12F);
            txtPassword.Location = new Point(169, 133);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(258, 29);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(44, 62, 80);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(169, 182);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(258, 40);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Font = new Font("Segoe UI", 12F);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(564, 67);
            panel1.TabIndex = 6;
            panel1.TabStop = true;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = Properties.Resources.folder;
            pictureBox1.Location = new Point(118, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 34);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // usernameInfo
            // 
            usernameInfo.Image = Properties.Resources.info;
            usernameInfo.Location = new Point(433, 93);
            usernameInfo.Name = "usernameInfo";
            usernameInfo.Size = new Size(24, 20);
            usernameInfo.SizeMode = PictureBoxSizeMode.StretchImage;
            usernameInfo.TabIndex = 7;
            usernameInfo.TabStop = false;
            // 
            // passwordInfo
            // 
            passwordInfo.Image = Properties.Resources.info;
            passwordInfo.Location = new Point(462, 137);
            passwordInfo.Name = "passwordInfo";
            passwordInfo.Size = new Size(24, 20);
            passwordInfo.SizeMode = PictureBoxSizeMode.StretchImage;
            passwordInfo.TabIndex = 8;
            passwordInfo.TabStop = false;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(433, 137);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 25);
            btnTogglePassword.TabIndex = 14;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // lblForgot
            // 
            lblForgot.AutoSize = true;
            lblForgot.ForeColor = SystemColors.Highlight;
            lblForgot.Location = new Point(327, 229);
            lblForgot.Name = "lblForgot";
            lblForgot.Size = new Size(100, 15);
            lblForgot.TabIndex = 15;
            lblForgot.Text = "Forgot Password?";
            lblForgot.Click += lblForgot_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(564, 267);
            Controls.Add(lblForgot);
            Controls.Add(btnTogglePassword);
            Controls.Add(passwordInfo);
            Controls.Add(usernameInfo);
            Controls.Add(lblUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UCU FOLDER LOCKER";
            Load += LoginForm_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)usernameInfo).EndInit();
            ((System.ComponentModel.ISupportInitialize)passwordInfo).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private Panel panel1;
        private PictureBox pictureBox1;
        private PictureBox usernameInfo;
        private PictureBox passwordInfo;
        private PictureBox btnTogglePassword;
        private Label lblForgot;
    }
}
