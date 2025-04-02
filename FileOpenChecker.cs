using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public class FileLockerHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RM_UNIQUE_PROCESS
    {
        public int dwProcessId;
        public FILETIME ProcessStartTime;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct RM_PROCESS_INFO
    {
        public RM_UNIQUE_PROCESS Process;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string strAppName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string strServiceShortName;
        public uint ApplicationType;
        public uint AppStatus;
        public uint TSSessionId;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bRestartable;
    }

    // Windows API Functions
    [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
    private static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

    [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
    private static extern int RmEndSession(uint pSessionHandle);

    [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
    private static extern int RmRegisterResources(uint pSessionHandle, uint nFiles, string[] rgsFilenames, uint nApplications,
                                                   IntPtr rgApplications, uint nServices, IntPtr rgsServiceNames);

    [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
    private static extern int RmGetList(uint pSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo,
                                        [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons);

    public static List<Process> GetProcessesLockingFile(string filePath)
    {
        uint sessionHandle;
        string sessionKey = Guid.NewGuid().ToString();
        List<Process> lockingProcesses = new List<Process>();

        if (RmStartSession(out sessionHandle, 0, sessionKey) != 0)
            return lockingProcesses;

        try
        {
            string[] resources = new string[] { filePath };
            if (RmRegisterResources(sessionHandle, (uint)resources.Length, resources, 0, IntPtr.Zero, 0, IntPtr.Zero) != 0)
                return lockingProcesses;

            uint procInfoNeeded = 0, procInfo = 1, rebootReasons = 0;
            RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[1];

            int result = RmGetList(sessionHandle, out procInfoNeeded, ref procInfo, processInfo, ref rebootReasons);
            if (result == 234) // ERROR_MORE_DATA: More processes exist than we allocated for
            {
                processInfo = new RM_PROCESS_INFO[procInfoNeeded];
                procInfo = procInfoNeeded;
                RmGetList(sessionHandle, out procInfoNeeded, ref procInfo, processInfo, ref rebootReasons);
            }

            for (int i = 0; i < procInfo; i++)
            {
                try
                {
                    Process process = Process.GetProcessById(processInfo[i].Process.dwProcessId);
                    lockingProcesses.Add(process);
                }
                catch { }
            }
        }
        finally
        {
            RmEndSession(sessionHandle);
        }

        return lockingProcesses;
    }
}
