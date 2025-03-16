using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace UCUFolderLocker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //Only register file association if needed
            CheckAndRegisterFileAssociation();

            //Check if a file path was passed(someone clicked a.lock file)
            if (args.Length > 0 && File.Exists(args[0]) && Path.GetExtension(args[0]).ToLower() == ".lock")
            {
                // Show dialog message before opening the main form
                MessageBox.Show($"This folder is locked. You will need to provide the password to access it.",
                               "Locked Folder", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Pass the file path to the main form
                //Application.Run(new MainForm(args[0]));
            }
            else
            {
                //Normal startup(no file clicked)
                Application.Run(new MainForm());
            }
        }

        // Check if association exists before registering
        private static void CheckAndRegisterFileAssociation()
        {
            try
            {
                // Check if the association already exists
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
                // Handle any errors silently - don't block application startup
            }
        }

        // Add this method to your class
        private static void RegisterFileAssociation()
        {
            string fileExtension = ".lock";
            string progId = "YourApp.LockFile";
            string applicationPath = Application.ExecutablePath;
            string iconPath = Path.Combine(Application.StartupPath, "lock.ico");

            // Use HKEY_CURRENT_USER instead of HKEY_CLASSES_ROOT
            using (var userClasses = Registry.CurrentUser.CreateSubKey(@"Software\Classes"))
            {
                // Create file extension association
                using (var ext = userClasses.CreateSubKey(fileExtension))
                {
                    ext.SetValue("", progId);
                }

                // Create program ID and default values
                using (var progIdKey = userClasses.CreateSubKey(progId))
                {
                    progIdKey.SetValue("", "Locked Folder");

                    // Set icon
                    using (var iconKey = progIdKey.CreateSubKey("DefaultIcon"))
                    {
                        iconKey.SetValue("", iconPath);
                    }

                    // Fix the command association
                    using (var cmdKey = progIdKey.CreateSubKey(@"shell\open\command"))
                    {
                        cmdKey.SetValue("", $"\"{applicationPath}\" \"%1\"");
                    }
                }
            }

            // Notify the system about the file association change
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        // Add this P/Invoke for SHChangeNotify
        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
