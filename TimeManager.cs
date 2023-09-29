using NLog;
using SFML.System;

namespace Tracks
{
    internal class TimeManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public float TotalTime { get; private set; }
        public float DeltaTime { get; private set; }

        public float InstantFps { get; private set; }
        public float RollingFps { get; private set; }

        public float TimeScale { get; private set; } = 1;

        private Clock Clock { get; } = new Clock();

        private Queue<float> RecentDeltaTimes { get; } = new Queue<float>(GameSettings.StatsFpsSampleCount);

        public void OnFrameStarted()
        {
            Time time = Clock.Restart();
            DeltaTime = time.AsSeconds() * TimeScale;
            TotalTime += DeltaTime;

            // TODO: Avoid in production
            CalculateFps();
        }

        private void CalculateFps()
        {
            if (RecentDeltaTimes.Count == GameSettings.StatsFpsSampleCount)
            {
                RecentDeltaTimes.Dequeue();
            }

            RecentDeltaTimes.Enqueue(DeltaTime);

            RollingFps = 1 / RecentDeltaTimes.Average();
            InstantFps = 1 / DeltaTime;

            //Logger.Info($"FPS Rolling: {RollingFps:0.00} Instant: {InstantFps:0.00}");
        }
    }
}
