namespace Tracks
{
    // Inspired by SFML Clock class
    internal static class Clock
    {
        private static DateTime LastTime { get; set; } = DateTime.Now;

        public static TimeSpan Restart()
        {
            DateTime currentTime = DateTime.Now;

            TimeSpan elapsedTime = LastTime - currentTime;
            LastTime = currentTime;

            return elapsedTime;
        }
    }
}
