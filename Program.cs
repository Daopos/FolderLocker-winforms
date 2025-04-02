using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace UCUFolderLocker
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            ApplicationConfiguration.Initialize();
            CheckAndRegisterFileAssociation();

            if (args.Length > 0 && File.Exists(args[0]) && Path.GetExtension(args[0]).ToLower() == ".lock")
            {
                string lockFilePath = args[0];
                string mutexName = "UCUFolderLocker_" + lockFilePath.Replace("\\", "_");

                using (Mutex mutex = new Mutex(true, mutexName, out bool isNewInstance))
                {
                    if (!isNewInstance)
                    {
                        MessageBox.Show($"The file '{Path.GetFileName(lockFilePath)}' is already open.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show("This folder is locked. You will need to provide the password to access it.",
                                    "Locked Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Application.Run(new Unlock(lockFilePath));
                }
            }
            else
            {
                // Check if LoginForm is already running
                bool isLoginFormOpen = false;
                foreach (Form openForm in Application.OpenForms)
                {
                    if (openForm is LoginForm)
                    {
                        isLoginFormOpen = true;
                        break;
                    }
                }

                if (!isLoginFormOpen)
                {
                    Application.Run(new LoginForm());
                }
                else
                {
                }
            }
        }

        private static void CheckAndRegisterFileAssociation()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\.lock"))
                {
                    if (key == null)
                    {
                        RegisterFileAssociation();
                    }
                }
            }
            catch (Exception)
            {
                // Handle silently
            }
        }

        private static void RegisterFileAssociation()
        {
            string fileExtension = ".lock";
            string progId = "YourApp.LockFile";
            string applicationPath = Application.ExecutablePath;
            string iconPath = Path.Combine(Application.StartupPath, "lock.ico");

            using (var userClasses = Registry.CurrentUser.CreateSubKey(@"Software\Classes"))
            {
                using (var ext = userClasses.CreateSubKey(fileExtension))
                {
                    ext.SetValue("", progId);
                }

                using (var progIdKey = userClasses.CreateSubKey(progId))
                {
                    progIdKey.SetValue("", "Locked Folder");

                    using (var iconKey = progIdKey.CreateSubKey("DefaultIcon"))
                    {
                        iconKey.SetValue("", iconPath);
                    }

                    using (var cmdKey = progIdKey.CreateSubKey(@"shell\open\command"))
                    {
                        cmdKey.SetValue("", $"\"{applicationPath}\" \"%1\"");
                    }
                }
            }

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
