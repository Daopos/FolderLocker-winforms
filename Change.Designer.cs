namespace UCUFolderLocker
{
    partial class Change
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
            label1 = new Label();
            panel1 = new Panel();
            labelPath = new Label();
            username = new TextBox();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            newUsername = new TextBox();
            label3 = new Label();
            newPassword = new TextBox();
            pictureBox2 = new PictureBox();
            label4 = new Label();
            password = new TextBox();
            btnSubmit = new Button();
            label5 = new Label();
            email = new TextBox();
            btnTogglePassword = new PictureBox();
            pictureBox3 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Popup;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 6);
            label1.Name = "label1";
            label1.Size = new Size(292, 37);
            label1.TabIndex = 0;
            label1.Text = "CHANGE LOGIN INFO";
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
            panel1.TabIndex = 11;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(13, 83);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(84, 21);
            labelPath.TabIndex = 12;
            labelPath.Text = "Username:";
            // 
            // username
            // 
            username.Font = new Font("Segoe UI", 12F);
            username.Location = new Point(103, 83);
            username.Margin = new Padding(3, 2, 3, 2);
            username.Name = "username";
            username.Size = new Size(162, 29);
            username.TabIndex = 13;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.arrow;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(280, 83);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(65, 29);
            pictureBox1.TabIndex = 14;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(358, 83);
            label2.Name = "label2";
            label2.Size = new Size(120, 21);
            label2.TabIndex = 15;
            label2.Text = "New Username:";
            // 
            // newUsername
            // 
            newUsername.Font = new Font("Segoe UI", 12F);
            newUsername.Location = new Point(484, 83);
            newUsername.Margin = new Padding(3, 2, 3, 2);
            newUsername.Name = "newUsername";
            newUsername.Size = new Size(162, 29);
            newUsername.TabIndex = 16;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(358, 150);
            label3.Name = "label3";
            label3.Size = new Size(115, 21);
            label3.TabIndex = 20;
            label3.Text = "New Password:";
            // 
            // newPassword
            // 
            newPassword.Font = new Font("Segoe UI", 12F);
            newPassword.Location = new Point(484, 150);
            newPassword.Margin = new Padding(3, 2, 3, 2);
            newPassword.Name = "newPassword";
            newPassword.Size = new Size(145, 29);
            newPassword.TabIndex = 21;
            newPassword.UseSystemPasswordChar = true;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = Properties.Resources.arrow;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(280, 150);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(65, 29);
            pictureBox2.TabIndex = 19;
            pictureBox2.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(13, 150);
            label4.Name = "label4";
            label4.Size = new Size(79, 21);
            label4.TabIndex = 17;
            label4.Text = "Password:";
            // 
            // password
            // 
            password.Font = new Font("Segoe UI", 12F);
            password.Location = new Point(103, 150);
            password.Margin = new Padding(3, 2, 3, 2);
            password.Name = "password";
            password.Size = new Size(142, 29);
            password.TabIndex = 18;
            password.UseSystemPasswordChar = true;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.FromArgb(44, 62, 80);
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(215, 273);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(258, 40);
            btnSubmit.TabIndex = 22;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSaveChanges_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(18, 216);
            label5.Name = "label5";
            label5.Size = new Size(105, 21);
            label5.TabIndex = 23;
            label5.Text = "Default Email:";
            // 
            // email
            // 
            email.Font = new Font("Segoe UI", 12F);
            email.Location = new Point(134, 216);
            email.Margin = new Padding(3, 2, 3, 2);
            email.Name = "email";
            email.Size = new Size(280, 29);
            email.TabIndex = 24;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(251, 153);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 25);
            btnTogglePassword.TabIndex = 25;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.hidden;
            pictureBox3.Location = new Point(634, 153);
            pictureBox3.Margin = new Padding(3, 2, 3, 2);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(23, 25);
            pictureBox3.TabIndex = 26;
            pictureBox3.TabStop = false;
            pictureBox3.Click += btnToggleNewPassword_Click;
            // 
            // Change
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(669, 348);
            Controls.Add(pictureBox3);
            Controls.Add(btnTogglePassword);
            Controls.Add(label5);
            Controls.Add(email);
            Controls.Add(btnSubmit);
            Controls.Add(label3);
            Controls.Add(newPassword);
            Controls.Add(pictureBox2);
            Controls.Add(label4);
            Controls.Add(password);
            Controls.Add(label2);
            Controls.Add(newUsername);
            Controls.Add(pictureBox1);
            Controls.Add(labelPath);
            Controls.Add(username);
            Controls.Add(panel1);
            Name = "Change";
            Text = "Change";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Label labelPath;
        private TextBox username;
        private PictureBox pictureBox1;
        private Label label2;
        private TextBox newUsername;
        private Label label3;
        private TextBox newPassword;
        private PictureBox pictureBox2;
        private Label label4;
        private TextBox password;
        private Button btnSubmit;
        private Label label5;
        private TextBox email;
        private PictureBox btnTogglePassword;
        private PictureBox pictureBox3;
    }
}