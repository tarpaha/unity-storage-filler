using System;
using System.Runtime.InteropServices;

namespace Platform
{
    public class PlatformWindowsEditor : IPlatform
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
            out ulong lpFreeBytesAvailable,
            out ulong lpTotalNumberOfBytes,
            out ulong lpTotalNumberOfFreeBytes);

        private readonly char _drive;

        public PlatformWindowsEditor(string path)
        {
            _drive = path[0];
        }

        public long FreeSpace
        {
            get
            {
                ulong freeBytesAvail;
                ulong totalNumOfBytes;
                ulong totalNumOfFreeBytes;
                if(!GetDiskFreeSpaceEx(_drive + ":\\", out freeBytesAvail, out totalNumOfBytes, out totalNumOfFreeBytes))
                    throw new Exception("GetDiskFreeSpaceEx() failed");
                return (long)freeBytesAvail;
            }
        }
    }
}