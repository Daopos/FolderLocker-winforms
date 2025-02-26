using System.Windows.Forms;

namespace UCUFolderLocker
{
    partial class All
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
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            btnBrowse = new Button();
            txtFolderPath = new TextBox();
            lblStatus = new Label();
            lstLockedFolders = new ListView();
            No = new ColumnHeader();
            name = new ColumnHeader();
            path = new ColumnHeader();
            panel1 = new Panel();
            label1 = new Label();
            panelListView = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(32, 76);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(100, 30);
            btnBrowse.TabIndex = 1;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(142, 80);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.ReadOnly = true;
            txtFolderPath.Size = new Size(450, 27);
            txtFolderPath.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(32, 128);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(50, 20);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "label1";
            lblStatus.Click += lblStatus_Click;
            // 
            // lstLockedFolders
            // 
            lstLockedFolders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstLockedFolders.Columns.AddRange(new ColumnHeader[] { No, name, path });
            lstLockedFolders.FullRowSelect = true;
            lstLockedFolders.GridLines = true;
            lstLockedFolders.Location = new Point(32, 168);
            lstLockedFolders.MultiSelect = false;
            lstLockedFolders.Name = "lstLockedFolders";
            lstLockedFolders.Size = new Size(679, 288);
            lstLockedFolders.TabIndex = 0;
            lstLockedFolders.UseCompatibleStateImageBehavior = false;
            lstLockedFolders.View = View.Details;
            // 
            // No
            // 
            No.Text = "No.";
            // 
            // name
            // 
            name.Text = "Folder Name";
            name.Width = 150;
            // 
            // path
            // 
            path.Text = "Folder Path";
            path.Width = 400;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(44, 62, 80);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 63);
            panel1.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.FlatStyle = FlatStyle.Popup;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(4, 8);
            label1.Name = "label1";
            label1.Size = new Size(396, 46);
            label1.TabIndex = 0;
            label1.Text = "FIND LOCKED FOLDERS";
            // 
            // panelListView
            // 
            panelListView.AutoScroll = true;
            panelListView.Location = new Point(32, 168);
            panelListView.Name = "panelListView";
            panelListView.Size = new Size(680, 290);
            panelListView.TabIndex = 0;
            // 
            // All
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 487);
            Controls.Add(panel1);
            Controls.Add(lstLockedFolders);
            Controls.Add(lblStatus);
            Controls.Add(txtFolderPath);
            Controls.Add(btnBrowse);
            Controls.Add(panelListView);
            Name = "All";
            Text = "Locked Folders";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button btnBrowse;
        private TextBox txtFolderPath;
        private Label lblStatus;
        private ListView lstLockedFolders;
        private ColumnHeader No;
        private ColumnHeader name;
        private ColumnHeader path;
        private Panel panel1;
        private Label label1;
        private Panel panelListView;
    }
}
