using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DLLUnManager
{
    public class DLLManager
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", SetLastError=true)]
        public static extern int LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true)]
        public static extern IntPtr GetProcAddress(int hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
        public static extern bool FreeLibrary(int hModule);
    }
}
