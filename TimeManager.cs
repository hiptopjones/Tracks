namespace Tracks
{
    internal class TimeManager
    {
        public float TotalTime { get; private set; }
        public float DeltaTime { get; private set; }

        public float InstantFps { get; private set; }
        public float RollingFps { get; private set; }

        public float TimeScale { get; private set; } = 1;

        private Queue<float> RecentDeltaTimes { get; } = new Queue<float>(GameSettings.DiagnosticsFpsSampleCount);

        public void OnFrameStarted()
        {
            TimeSpan elapsedTime = Clock.Restart();

            DeltaTime = (float)elapsedTime.TotalSeconds * TimeScale;
            TotalTime += DeltaTime;

            CalculateFps();
        }

        private void CalculateFps()
        {
            if (RecentDeltaTimes.Count == GameSettings.DiagnosticsFpsSampleCount)
            {
                RecentDeltaTimes.Dequeue();
            }

            RecentDeltaTimes.Enqueue(DeltaTime);

            RollingFps = 1 / RecentDeltaTimes.Average();
            InstantFps = 1 / DeltaTime;
        }
    }
}
