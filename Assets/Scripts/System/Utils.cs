namespace System
{
    public static class Utils
    {
        public static string HumanReadable(long size)
        {
            string[] sizes = {"B", "KB", "MB" };
            var order = 0;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }
            return string.Format("{0:0.##} {1}", size, sizes[order]);
        }
    }
}