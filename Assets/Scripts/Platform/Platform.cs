﻿using System.IO;
using UnityEngine;

namespace Platform
{
    public static class Platform
    {
        public static IPlatform Get()
        {
#if UNITY_EDITOR_WIN
            return new PlatformWindowsEditor(Path.GetPathRoot(Application.persistentDataPath));
#elif UNITY_ANDROID
			return new PlatformAndroid();
#endif
        }
    }
}