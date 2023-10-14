using OpenTK.Mathematics;
using System.Diagnostics;
using System.Drawing;

namespace Tracks
{
    internal class DiagnosticsComponent : Component
    {
        public Vector2 RowOffset { get; set; }
        public Vector2 ColumnOffset { get; set; }

        private TimeManager TimeManager { get; set; }
        private GameObjectManager GameObjectManager { get; set; }
        private Process CurrentProcess { get; set; }

        private Dictionary<string, Func<string>> StatisticValueGetters { get; set; } = new Dictionary<string, Func<string>>();
        private List<string> StatisticLabels { get; set; } = new List<string>();

        public override void Awake()
        {
            TimeManager = ServiceLocator.Instance.GetService<TimeManager>();
            GameObjectManager = ServiceLocator.Instance.GetService<GameObjectManager>();
            CurrentProcess = Process.GetCurrentProcess();

            AddStatisticValue("FPS", GetFps);
            AddStatisticValue("CPU", GetCpuUsage);
            AddStatisticValue("MEM", GetMemoryUsage);
            AddStatisticValue("OBJ", GetObjectCount);
            AddStatisticValue("CMP", GetComponentCount);
        }

        public override void Update(float deltaTime)
        {
            DrawStatistics();
        }

        public void AddStatisticValue(string statisticLabel, Func<string> statisticValueGetter)
        {
            StatisticLabels.Add(statisticLabel);
            StatisticValueGetters[statisticLabel] = statisticValueGetter;
        }

        private void DrawStatistics()
        {
            float scale = Owner.Transform.Scale.X;

            Vector2 labelPosition = Owner.Transform.Position.Xy;
            Vector2 valuePosition = labelPosition + RowOffset * scale;

            foreach (string statisticLabel in StatisticLabels)
            {
                Func<string> statisticValueGetter = StatisticValueGetters[statisticLabel];
                string statisticValue = statisticValueGetter.Invoke();

                Debug.DrawText(statisticLabel, labelPosition, Color.Magenta, scale);
                Debug.DrawText(statisticValue, valuePosition, Color.White, scale);

                labelPosition += ColumnOffset * scale;
                valuePosition = labelPosition + RowOffset * scale;
            }
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

        private string GetMemoryUsage()
        {
            double memoryUsage = CurrentProcess.WorkingSet64 / 1000d / 1000d;
            return $"{memoryUsage:F1}M";
        }

        private string GetObjectCount()
        {
            int gameObjectCount = GameObjectManager.GameObjectCount;
            return $"{gameObjectCount}";
        }

        private string GetComponentCount()
        {
            int componentCount = GameObjectManager.ComponentCount;
            return $"{componentCount}";
        }
    }
}