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
            btnToggleConfirmPassword = new PictureBox();
            btnTogglePassword = new PictureBox();
            label3 = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)btnToggleConfirmPassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtNewPassword
            // 
            txtNewPassword.Font = new Font("Segoe UI", 12F);
            txtNewPassword.Location = new Point(112, 81);
            txtNewPassword.Margin = new Padding(3, 2, 3, 2);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(163, 29);
            txtNewPassword.TabIndex = 0;
            txtNewPassword.UseSystemPasswordChar = true;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Segoe UI", 12F);
            txtConfirmPassword.Location = new Point(112, 136);
            txtConfirmPassword.Margin = new Padding(3, 2, 3, 2);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(163, 29);
            txtConfirmPassword.TabIndex = 1;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // btnChangePassword_Click
            // 
            btnChangePassword_Click.BackColor = Color.FromArgb(62, 123, 39);
            btnChangePassword_Click.FlatStyle = FlatStyle.Flat;
            btnChangePassword_Click.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnChangePassword_Click.ForeColor = Color.White;
            btnChangePassword_Click.Location = new Point(112, 171);
            btnChangePassword_Click.Margin = new Padding(3, 2, 3, 2);
            btnChangePassword_Click.Name = "btnChangePassword_Click";
            btnChangePassword_Click.Size = new Size(163, 32);
            btnChangePassword_Click.TabIndex = 2;
            btnChangePassword_Click.Text = "Confirm";
            btnChangePassword_Click.UseVisualStyleBackColor = false;
            btnChangePassword_Click.Click += btnChangePasswords_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(108, 58);
            label1.Name = "label1";
            label1.Size = new Size(115, 21);
            label1.TabIndex = 3;
            label1.Text = "New Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(108, 113);
            label2.Name = "label2";
            label2.Size = new Size(141, 21);
            label2.TabIndex = 4;
            label2.Text = "Confirm password:";
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ControlDark;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            button1.ForeColor = Color.Black;
            button1.Location = new Point(112, 216);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(163, 32);
            button1.TabIndex = 5;
            button1.Text = "Cancel";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnCancel_Click;
            // 
            // btnToggleConfirmPassword
            // 
            btnToggleConfirmPassword.Image = Properties.Resources.hidden;
            btnToggleConfirmPassword.Location = new Point(280, 139);
            btnToggleConfirmPassword.Margin = new Padding(3, 2, 3, 2);
            btnToggleConfirmPassword.Name = "btnToggleConfirmPassword";
            btnToggleConfirmPassword.Size = new Size(23, 25);
            btnToggleConfirmPassword.TabIndex = 16;
            btnToggleConfirmPassword.TabStop = false;
            btnToggleConfirmPassword.Click += btnToggleConfirmPassword_Click;
            // 
            // btnTogglePassword
            // 
            btnTogglePassword.Image = Properties.Resources.hidden;
            btnTogglePassword.Location = new Point(280, 86);
            btnTogglePassword.Margin = new Padding(3, 2, 3, 2);
            btnTogglePassword.Name = "btnTogglePassword";
            btnTogglePassword.Size = new Size(23, 24);
            btnTogglePassword.TabIndex = 17;
            btnTogglePassword.TabStop = false;
            btnTogglePassword.Click += btnTogglePassword_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.8F, FontStyle.Bold | FontStyle.Italic);
            label3.ForeColor = Color.White;
            label3.Location = new Point(10, 9);
            label3.Name = "label3";
            label3.Size = new Size(285, 28);
            label3.TabIndex = 18;
            label3.Text = "CHANGE PASSSWORD FORM";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(396, 45);
            panel1.TabIndex = 19;
            // 
            // ChangePasswordForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(396, 279);
            Controls.Add(panel1);
            Controls.Add(btnTogglePassword);
            Controls.Add(btnToggleConfirmPassword);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnChangePassword_Click);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtNewPassword);
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "ChangePasswordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChangePasswordForm";
            ((System.ComponentModel.ISupportInitialize)btnToggleConfirmPassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnTogglePassword).EndInit();
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
        private PictureBox btnToggleConfirmPassword;
        private PictureBox btnTogglePassword;
        private Label label3;
        private Panel panel1;
    }
}