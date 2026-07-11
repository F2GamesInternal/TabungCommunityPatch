namespace OfflinePhoton.Helpers
{
    public static class Log
    {
        public static void Info(string msg) => Plugin.Log?.LogInfo("[OfflinePhoton] " + msg);
        public static void Warn(string msg) => Plugin.Log?.LogWarning("[OfflinePhoton] " + msg);
        public static void Error(string msg) => Plugin.Log?.LogError("[OfflinePhoton] " + msg);
    }
}
