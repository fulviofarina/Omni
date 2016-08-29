using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Rsx
{
    internal class NetUtil
    {
        [DllImport("netapi32.dll", CharSet = CharSet.Auto)]
        private static extern int NetWkstaGetInfo(string server,
            int level,
            out IntPtr info);

        [DllImport("netapi32.dll")]
        private static extern int NetApiBufferFree(IntPtr pBuf);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class WKSTA_INFO_100
        {
            public int wki100_platform_id;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string wki100_computername;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string wki100_langroup;

            public int wki100_ver_major;
            public int wki100_ver_minor;
        }

        public static string GetMachineNetBiosDomain()
        {
            IntPtr pBuffer = IntPtr.Zero;

            WKSTA_INFO_100 info;
            int retval = NetWkstaGetInfo(null, 100, out pBuffer);
            if (retval != 0)
                throw new Win32Exception(retval);

            info = (WKSTA_INFO_100)Marshal.PtrToStructure(pBuffer, typeof(WKSTA_INFO_100));
            string domainName = info.wki100_langroup;
            NetApiBufferFree(pBuffer);
            return domainName;
        }
    }
}