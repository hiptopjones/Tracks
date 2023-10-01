using SFML.Graphics;
using SFML.System;
using System.Diagnostics;

namespace Tracks
{
    internal class DiagnosticsComponent : Component
    {
        private TimeManager TimeManager { get; set; }
        private Process CurrentProcess { get; set; }

        private Vector2f RowOffset { get; } = new Vector2f(0, 20);
        private Vector2f ColumnOffset { get; } = new Vector2f(60, 0);

        public override void Awake()
        {
            TimeManager = ServiceLocator.Instance.GetService<TimeManager>();
            CurrentProcess = Process.GetCurrentProcess();
        }

        public override void Update(float deltaTime)
        {
            DrawStats();
        }

        private void DrawStats()
        {
            Vector2f labelPosition = Owner.Transform.Position;
            Vector2f valuePosition = labelPosition + RowOffset;

            string fps = GetFps();
            Debug.DrawText("FPS", labelPosition, Color.Magenta);
            Debug.DrawText(fps, valuePosition, Color.White);

            labelPosition += ColumnOffset;
            valuePosition = labelPosition + RowOffset;

            string cpuUsage = GetCpuUsage();
            Debug.DrawText("CPU", labelPosition, Color.Magenta);
            Debug.DrawText(cpuUsage, valuePosition, Color.White);

            labelPosition += ColumnOffset;
            valuePosition = labelPosition + RowOffset;

            string memoryUsage = GetMemoryUsage();
            Debug.DrawText("MEM", labelPosition, Color.Magenta);
            Debug.DrawText(memoryUsage, valuePosition, Color.White);
        }

        private string GetMemoryUsage()
        {
            double memoryUsage = CurrentProcess.WorkingSet64 / 1000d / 1000d;
            return $"{memoryUsage:F1}M";
        }

        private string GetFps()
        {
            float fps = TimeManager.RollingFps;

            return $"{fps:F1}";
        }

        private string GetCpuUsage()
        {
            double totalAvailableTime = Environment.ProcessorCount * (DateTime.Now - CurrentProcess.StartTime).TotalMilliseconds;
            double totalUsedTime = CurrentProcess.TotalProcessorTime.TotalMilliseconds;
            double cpuUsage = totalUsedTime / totalAvailableTime * 100.0;

            return $"{cpuUsage:F1}%";
        }
    }
}