namespace Platform
{
    public static class Platform
    {
        public static IPlatform Get()
        {
#if UNITY_EDITOR_WIN
            return new PlatformWindowsEditor();
#elif UNITY_ANDROID
			return new PlatformAndroid();
#elif UNITY_IOS
            return new PlatformIOS();
#endif
        }
    }
}