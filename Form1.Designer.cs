namespace UCUFolderLocker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Select = new Button();
            button2 = new Button();
            lblFolderPath = new Label();
            txtPassword = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // Select
            // 
            Select.Location = new Point(126, 121);
            Select.Name = "Select";
            Select.Size = new Size(134, 34);
            Select.TabIndex = 1;
            Select.Text = "Select Folder";
            Select.UseVisualStyleBackColor = true;
            Select.Click += btnBrowse_Click;
            // 
            // button2
            // 
            button2.Location = new Point(85, 300);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 2;
            button2.Text = "Lock";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnLock_Click;
            // 
            // lblFolderPath
            // 
            lblFolderPath.AutoSize = true;
            lblFolderPath.Location = new Point(276, 128);
            lblFolderPath.Name = "lblFolderPath";
            lblFolderPath.Size = new Size(50, 20);
            lblFolderPath.TabIndex = 3;
            lblFolderPath.Text = "label1";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(135, 205);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(125, 27);
            txtPassword.TabIndex = 4;
            // 
            // button1
            // 
            button1.Location = new Point(226, 300);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 5;
            button1.Text = "UnLock";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnUnlock_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(txtPassword);
            Controls.Add(lblFolderPath);
            Controls.Add(button2);
            Controls.Add(Select);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button Select;
        private Button button2;
        private Label lblFolderPath;
        private TextBox txtPassword;
        private Button button1;
    }
}
