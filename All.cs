﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UCUFolderLocker
{
    public partial class All : Form
    {
        private ContextMenuStrip contextMenu;

        public All()
        {
            InitializeComponent();
            LoadLockedFolders(@"C:\"); // Default scan on C drive
            InitializeContextMenu();
        }
        private int clickedColumnIndex = 0; // Store the column index

        private void lstLockedFolders_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hitTest = lstLockedFolders.HitTest(e.Location);
                if (hitTest.Item != null)
                {
                    clickedColumnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem); // Get clicked column index
                    lstLockedFolders.ContextMenuStrip = contextMenu;
                }
            }
        }

        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy");
            copyItem.Click += CopySelectedItem;
            contextMenu.Items.Add(copyItem);
            lstLockedFolders.MouseDown += lstLockedFolders_MouseDown; // Attach mouse event
        }

        private void CopySelectedItem(object sender, EventArgs e)
        {
            if (lstLockedFolders.SelectedItems.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ListViewItem item in lstLockedFolders.SelectedItems)
                {
                    if (clickedColumnIndex < item.SubItems.Count)
                    {
                        sb.AppendLine(item.SubItems[clickedColumnIndex].Text); // Copy only the clicked column
                    }
                }
                Clipboard.SetText(sb.ToString());
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    txtFolderPath.Text = selectedPath;
                    LoadLockedFolders(selectedPath);
                }
            }
        }

        private void LoadLockedFolders(string rootPath)
        {
            lstLockedFolders.Items.Clear();
            int count = 0;

            try
            {
                // First search for .lock files in the current directory
                foreach (var lockFile in Directory.GetFiles(rootPath, "*.lock"))
                {
                    try
                    {
                        string folderName = Path.GetFileNameWithoutExtension(lockFile);

                        count++;
                        ListViewItem item = new ListViewItem(count.ToString()); // Row number
                        item.SubItems.Add(folderName); // Original folder name (without .lock extension)
                        item.SubItems.Add(rootPath); // Directory path
                        lstLockedFolders.Items.Add(item);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Skip files without permission
                        continue;
                    }
                }

                // Then search subdirectories one by one to handle permission errors
                foreach (var dir in Directory.GetDirectories(rootPath))
                {
                    try
                    {
                        // Look for .lock files in each subdirectory
                        foreach (var lockFile in Directory.GetFiles(dir, "*.lock"))
                        {
                            string folderName = Path.GetFileNameWithoutExtension(lockFile);

                            count++;
                            ListViewItem item = new ListViewItem(count.ToString()); // Row number
                            item.SubItems.Add(folderName); // Original folder name (without .lock extension)
                            item.SubItems.Add(dir); // Directory path
                            lstLockedFolders.Items.Add(item);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Skip directories without permission
                        continue;
                    }
                    catch (Exception)
                    {
                        // Skip other errors for individual directories
                        continue;
                    }
                }

                lblStatus.Text = $"Found {count} locked folders.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scanning folders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstLockedFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstLockedFolders.SelectedItems.Count > 0)
            {
                string folderPath = lstLockedFolders.SelectedItems[0].SubItems[2].Text;
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
