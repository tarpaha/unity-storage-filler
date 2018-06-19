#if UNITY_EDITOR_WIN
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

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

        public PlatformWindowsEditor()
        {
            _drive = Path.GetPathRoot(Application.persistentDataPath)[0];
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
#endif