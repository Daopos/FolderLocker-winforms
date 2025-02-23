namespace UCUFolderLocker
{
    partial class MainForm
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
            panelSidebar = new Panel();
            btnAllForm = new Button();
            btnLockForm = new Button();
            panelContent = new Panel();
            panelSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.DarkSlateGray;
            panelSidebar.Controls.Add(btnAllForm);
            panelSidebar.Controls.Add(btnLockForm);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(156, 466);
            panelSidebar.TabIndex = 0;
            // 
            // btnAllForm
            // 
            btnAllForm.BackColor = Color.Teal;
            btnAllForm.FlatStyle = FlatStyle.Flat;
            btnAllForm.ForeColor = Color.White;
            btnAllForm.Location = new Point(22, 79);
            btnAllForm.Name = "btnAllForm";
            btnAllForm.Size = new Size(94, 29);
            btnAllForm.TabIndex = 1;
            btnAllForm.Text = "View";
            btnAllForm.UseVisualStyleBackColor = false;
            btnAllForm.Click += btnAllForm_Click;
            // 
            // btnLockForm
            // 
            btnLockForm.BackColor = Color.Teal;
            btnLockForm.FlatStyle = FlatStyle.Flat;
            btnLockForm.ForeColor = Color.White;
            btnLockForm.Location = new Point(22, 24);
            btnLockForm.Name = "btnLockForm";
            btnLockForm.Size = new Size(94, 29);
            btnLockForm.TabIndex = 0;
            btnLockForm.Text = "Lock";
            btnLockForm.UseVisualStyleBackColor = false;
            btnLockForm.Click += btnLockForm_Click;
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(156, 0);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(817, 466);
            panelContent.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(973, 466);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Name = "MainForm";
            Text = "Form2";
            panelSidebar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Panel panelContent;
        private Button btnAllForm;
        private Button btnLockForm;
    }
}