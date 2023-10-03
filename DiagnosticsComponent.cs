﻿using OpenTK.Mathematics;
using System.Diagnostics;
using System.Drawing;

namespace Tracks
{
    internal class DiagnosticsComponent : Component
    {
        public Vector2 RowOffset { get; } = new Vector2(0, -60);
        public Vector2 ColumnOffset { get; } = new Vector2(250, 0);

        private TimeManager TimeManager { get; set; }
        private GameObjectManager GameObjectManager { get; set; }
        private Process CurrentProcess { get; set; }

        public override void Awake()
        {
            TimeManager = ServiceLocator.Instance.GetService<TimeManager>();
            GameObjectManager = ServiceLocator.Instance.GetService<GameObjectManager>();
            CurrentProcess = Process.GetCurrentProcess();
        }

        public override void Update(float deltaTime)
        {
            DrawStats();
        }

        private void DrawStats()
        {
            Vector2 labelPosition = Owner.Transform.Position.Xy;
            Vector2 valuePosition = labelPosition + RowOffset;

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

            labelPosition += ColumnOffset;
            valuePosition = labelPosition + RowOffset;

            string numObjects = GetObjectCount();
            Debug.DrawText("OBJ", labelPosition, Color.Magenta);
            Debug.DrawText(numObjects, valuePosition, Color.White);

            labelPosition += ColumnOffset;
            valuePosition = labelPosition + RowOffset;

            string numComponents = GetComponentCount();
            Debug.DrawText("CMP", labelPosition, Color.Magenta);
            Debug.DrawText(numComponents, valuePosition, Color.White);
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