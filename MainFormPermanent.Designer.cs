namespace UCUFolderLocker
{
    partial class MainFormPermanent
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
            panelContent = new Panel();
            panel2 = new Panel();
            label2 = new Label();
            panel1 = new Panel();
            lblTitle = new Label();
            btnForgotForm = new Button();
            btnUnlockForm = new Button();
            btnLockForm = new Button();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 113);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(700, 337);
            panelContent.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(240, 240, 240);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(btnForgotForm);
            panel2.Controls.Add(btnUnlockForm);
            panel2.Controls.Add(btnLockForm);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(700, 113);
            panel2.TabIndex = 1;
            // 
            // label2
            // 
            label2.BorderStyle = BorderStyle.Fixed3D;
            label2.Location = new Point(9, 103);
            label2.Name = "label2";
            label2.Size = new Size(656, 2);
            label2.TabIndex = 18;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(lblTitle);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(700, 47);
            panel1.TabIndex = 12;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.FlatStyle = FlatStyle.Popup;
            lblTitle.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(4, 6);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(218, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "LOCK A FOLDER";
            // 
            // btnForgotForm
            // 
            btnForgotForm.BackColor = Color.FromArgb(52, 73, 94);
            btnForgotForm.FlatStyle = FlatStyle.Flat;
            btnForgotForm.ForeColor = Color.White;
            btnForgotForm.Location = new Point(382, 61);
            btnForgotForm.Name = "btnForgotForm";
            btnForgotForm.Size = new Size(175, 34);
            btnForgotForm.TabIndex = 2;
            btnForgotForm.Text = "Forgot Password";
            btnForgotForm.UseVisualStyleBackColor = false;
            btnForgotForm.Click += btnForgotForm_Click;
            // 
            // btnUnlockForm
            // 
            btnUnlockForm.BackColor = Color.FromArgb(52, 73, 94);
            btnUnlockForm.FlatStyle = FlatStyle.Flat;
            btnUnlockForm.ForeColor = Color.White;
            btnUnlockForm.Location = new Point(196, 61);
            btnUnlockForm.Name = "btnUnlockForm";
            btnUnlockForm.Size = new Size(175, 34);
            btnUnlockForm.TabIndex = 1;
            btnUnlockForm.Text = "Unlock";
            btnUnlockForm.UseVisualStyleBackColor = false;
            btnUnlockForm.Click += btnUnlockForm_Click;
            // 
            // btnLockForm
            // 
            btnLockForm.BackColor = Color.FromArgb(52, 73, 94);
            btnLockForm.FlatStyle = FlatStyle.Flat;
            btnLockForm.ForeColor = Color.White;
            btnLockForm.Location = new Point(10, 61);
            btnLockForm.Name = "btnLockForm";
            btnLockForm.Size = new Size(175, 34);
            btnLockForm.TabIndex = 0;
            btnLockForm.Text = "Lock";
            btnLockForm.UseVisualStyleBackColor = false;
            btnLockForm.Click += btnLockForm_Click;
            // 
            // MainFormPermanent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 450);
            Controls.Add(panelContent);
            Controls.Add(panel2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainFormPermanent";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Folder Locker";
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelContent;
        private Panel panel2;
        private Button btnForgotForm;
        private Button btnUnlockForm;
        private Button btnLockForm;
        private Panel panel1;
        private Label lblTitle;
        private Label label2;
    }
}