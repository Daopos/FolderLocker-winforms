namespace UCUFolderLocker
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            labelPath = new Label();
            txtEmail = new TextBox();
            btnSubmit = new Button();
            SuspendLayout();
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Segoe UI", 12F);
            labelPath.Location = new Point(25, 45);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(51, 21);
            labelPath.TabIndex = 144;
            labelPath.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 12F);
            txtEmail.Location = new Point(82, 42);
            txtEmail.Margin = new Padding(3, 2, 3, 2);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(162, 29);
            txtEmail.TabIndex = 1;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.FromArgb(44, 62, 80);
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(93, 96);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(133, 33);
            btnSubmit.TabIndex = 23;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSend_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(273, 159);
            Controls.Add(btnSubmit);
            Controls.Add(labelPath);
            Controls.Add(txtEmail);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelPath;
        private TextBox txtEmail;
        private Button btnSubmit;
    }
}