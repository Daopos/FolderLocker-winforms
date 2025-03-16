namespace UCUFolderLocker
{
    partial class ForgotForm
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
            panel1 = new Panel();
            label1 = new Label();
            btnSend = new Button();
            labelPath = new Label();
            lblFolderPath = new TextBox();
            buttonBrowse = new Button();
            lblStatus = new Label();
            progressBarLoading = new ProgressBar();
            label2 = new Label();
            txtRecoveryEmail = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(765, 63);
            panel1.TabIndex = 16;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Popup;
            label1.Font = new Font("Segoe UI", 19.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 8);
            label1.Name = "label1";
            label1.Size = new Size(474, 45);
            label1.TabIndex = 0;
            label1.Text = "FORGOT PASSWORD FOLDER";
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(62, 123, 39);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(148, 224);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(370, 50);
            btnSend.TabIndex = 13;
            btnSend.Text = "Send ";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(27, 89);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(115, 28);
            labelPath.TabIndex = 17;
            labelPath.Text = "Folder Path:";
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(148, 89);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(370, 34);
            lblFolderPath.TabIndex = 18;
            // 
            // buttonBrowse
            // 
            buttonBrowse.BackColor = Color.FromArgb(200, 76, 5);
            buttonBrowse.FlatStyle = FlatStyle.Flat;
            buttonBrowse.Font = new Font("Segoe UI", 11F);
            buttonBrowse.ForeColor = Color.White;
            buttonBrowse.Location = new Point(535, 89);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(116, 34);
            buttonBrowse.TabIndex = 19;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F);
            lblStatus.Location = new Point(26, 187);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(267, 28);
            lblStatus.TabIndex = 20;
            lblStatus.Text = "Status: Recovery Successfully!";
            lblStatus.Visible = false;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(147, 282);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(370, 29);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 21;
            progressBarLoading.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(27, 140);
            label2.Name = "label2";
            label2.Size = new Size(59, 28);
            label2.TabIndex = 22;
            label2.Text = "Email";
            // 
            // txtRecoveryEmail
            // 
            txtRecoveryEmail.Font = new Font("Segoe UI", 12F);
            txtRecoveryEmail.Location = new Point(148, 140);
            txtRecoveryEmail.Name = "txtRecoveryEmail";
            txtRecoveryEmail.Size = new Size(370, 34);
            txtRecoveryEmail.TabIndex = 23;
            // 
            // ForgotForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(765, 464);
            Controls.Add(label2);
            Controls.Add(txtRecoveryEmail);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(panel1);
            Controls.Add(btnSend);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            Name = "ForgotForm";
            Text = "ForgotForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button btnSend;
        private Label labelPath;
        private TextBox lblFolderPath;
        private Button buttonBrowse;
        private Label lblStatus;
        private ProgressBar progressBarLoading;
        private Label label2;
        private TextBox txtRecoveryEmail;
    }
}