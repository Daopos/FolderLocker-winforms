namespace UCUFolderLocker
{
    partial class ChangePasswordForm
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
            txtNewPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnChangePassword_Click = new Button();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            btnTogglePassword = new PictureBox();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 12F);
            txtNewPassword.Location = new Point(128, 108);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '●';
            txtNewPassword.Size = new Size(186, 34);
            txtNewPassword.TabIndex = 0;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 12F);
            txtConfirmPassword.Location = new Point(128, 176);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '●';
            txtConfirmPassword.Size = new Size(186, 34);
            txtConfirmPassword.TabIndex = 1;
            // 
            // btnChangePassword_Click
            // 
            btnChangePassword_Click.BackColor = Color.FromArgb(62, 123, 39);
            btnChangePassword_Click.FlatStyle = FlatStyle.Flat;
            btnChangePassword_Click.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnChangePassword_Click.ForeColor = Color.White;
            btnChangePassword_Click.Location = new Point(128, 223);
            btnChangePassword_Click.Name = "btnChangePassword_Click";
            btnChangePassword_Click.Size = new Size(186, 42);
            btnChangePassword_Click.TabIndex = 2;
            btnChangePassword_Click.Text = "Confirm";
            btnChangePassword_Click.UseVisualStyleBackColor = false;
            btnChangePassword_Click.Click += btnChangePasswords_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(124, 77);
            label1.Name = "label1";
            label1.Size = new Size(141, 28);
            label1.TabIndex = 3;
            label1.Text = "New Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(124, 145);
            label2.Name = "label2";
            label2.Size = new Size(174, 28);
            label2.TabIndex = 4;
            label2.Text = "Confirm password:";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ControlDark;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(128, 283);
            button1.Name = "button1";
            button1.Size = new Size(186, 42);
            button1.TabIndex = 5;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnCancel_Click;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.eye;
            btnTogglePassword.Location = new Point(320, 182);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(26, 23);
            btnTogglePassword.TabIndex = 16;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnToggleConfirmPassword_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.eye;
            pictureBox1.Location = new Point(320, 114);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(26, 23);
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            pictureBox1.Click += btnTogglePassword_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.8F, FontStyle.Bold | FontStyle.Italic);
            label3.ForeColor = Color.White;
            label3.Location = new Point(12, 12);
            label3.Name = "label3";
            label3.Size = new Size(355, 35);
            label3.TabIndex = 18;
            label3.Text = "CHANGE PASSSWORD FORM";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(452, 60);
            panel1.TabIndex = 19;
            // 
            // ChangePasswordForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 363);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Controls.Add(btnTogglePassword);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnChangePassword_Click);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtNewPassword);
            MaximizeBox = false;
            Name = "ChangePasswordForm";
            Text = "ChangePasswordForm";
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnChangePassword_Click;
        private Label label1;
        private Label label2;
        private Button button1;
        private PictureBox btnTogglePassword;
        private PictureBox pictureBox1;
        private Label label3;
        private Panel panel1;
    }
}