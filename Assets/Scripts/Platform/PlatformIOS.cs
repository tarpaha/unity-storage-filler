#if UNITY_IOS 
using System.Runtime.InteropServices;

namespace Platform
{
    public class PlatformIOS: IPlatform
    {
        [DllImport("__Internal")]
        private static extern double CheckFreeSpace();
        
        public long FreeSpace { get { return (long)CheckFreeSpace(); } }
    }
}
#endif