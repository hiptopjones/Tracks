using NLog;
using OpenTK.Mathematics;

namespace Tracks
{
    internal static class Debug
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Log(string message)
        {
            Logger.Info(message);
        }

        public static void LogWarning(string message)
        {
            Logger.Warn(message);
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
        }
    }
}
