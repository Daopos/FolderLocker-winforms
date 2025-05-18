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
            btnSend = new Button();
            labelPath = new Label();
            lblFolderPath = new TextBox();
            buttonBrowse = new Button();
            lblStatus = new Label();
            progressBarLoading = new ProgressBar();
            label2 = new Label();
            txtRecoveryEmail = new TextBox();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(62, 123, 39);
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(217, 111);
            btnSend.Margin = new Padding(3, 2, 3, 2);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(160, 29);
            btnSend.TabIndex = 3;
            btnSend.Text = "Send ";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(6, 11);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(91, 21);
            labelPath.TabIndex = 17;
            labelPath.Text = "Folder Path:";
            // 
            // lblFolderPath
            // 
            lblFolderPath.Font = new Font("Segoe UI", 12F);
            lblFolderPath.Location = new Point(129, 11);
            lblFolderPath.Margin = new Padding(3, 2, 3, 2);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(324, 29);
            lblFolderPath.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            buttonBrowse.BackColor = Color.FromArgb(200, 76, 5);
            buttonBrowse.FlatStyle = FlatStyle.Flat;
            buttonBrowse.Font = new Font("Segoe UI", 11F);
            buttonBrowse.ForeColor = Color.White;
            buttonBrowse.Location = new Point(467, 11);
            buttonBrowse.Margin = new Padding(3, 2, 3, 2);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(102, 29);
            buttonBrowse.TabIndex = 19;
            buttonBrowse.Text = "Browse";
            buttonBrowse.UseVisualStyleBackColor = false;
            buttonBrowse.Click += btnBrowse_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F);
            lblStatus.Location = new Point(6, 87);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(216, 21);
            lblStatus.TabIndex = 20;
            lblStatus.Text = "Status: Recovery Successfully!";
            lblStatus.Visible = false;
            // 
            // progressBarLoading
            // 
            progressBarLoading.Location = new Point(129, 153);
            progressBarLoading.Margin = new Padding(3, 2, 3, 2);
            progressBarLoading.Name = "progressBarLoading";
            progressBarLoading.Size = new Size(324, 22);
            progressBarLoading.Style = ProgressBarStyle.Marquee;
            progressBarLoading.TabIndex = 21;
            progressBarLoading.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(6, 53);
            label2.Name = "label2";
            label2.Size = new Size(51, 21);
            label2.TabIndex = 22;
            label2.Text = "Email:";
            // 
            // txtRecoveryEmail
            // 
            txtRecoveryEmail.Font = new Font("Segoe UI", 12F);
            txtRecoveryEmail.Location = new Point(129, 53);
            txtRecoveryEmail.Margin = new Padding(3, 2, 3, 2);
            txtRecoveryEmail.Name = "txtRecoveryEmail";
            txtRecoveryEmail.Size = new Size(324, 29);
            txtRecoveryEmail.TabIndex = 2;
            // 
            // ForgotForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(669, 370);
            Controls.Add(label2);
            Controls.Add(txtRecoveryEmail);
            Controls.Add(progressBarLoading);
            Controls.Add(lblStatus);
            Controls.Add(btnSend);
            Controls.Add(labelPath);
            Controls.Add(lblFolderPath);
            Controls.Add(buttonBrowse);
            Margin = new Padding(3, 2, 3, 2);
            Name = "ForgotForm";
            Text = "ForgotForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
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