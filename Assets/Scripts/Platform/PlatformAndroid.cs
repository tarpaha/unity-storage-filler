#if UNITY_ANDROID
using UnityEngine;

namespace Platform
{
    public class PlatformAndroid : IPlatform
    {
        public long FreeSpace
        {
            get
            {
                var jc = new AndroidJavaClass("android.os.Environment");
                var file = jc.CallStatic<AndroidJavaObject>("getDataDirectory");
                var path = file.Call<string>("getAbsolutePath");

                var stat = new AndroidJavaObject("android.os.StatFs", path);
                var blocks = stat.Call<long>("getAvailableBlocksLong");
                var blockSize = stat.Call<long>("getBlockSizeLong");

                return blocks * blockSize;
            }
        }
    }
}
#endif