using NLog;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal static class Debug
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static List<Drawable> Drawables { get; } = new List<Drawable>();

        private static Font _defaultFont;
        private static Font DefaultFont
        {
            get
            {
                if (_defaultFont == null)
                {
                    ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
                    _defaultFont = resourceManager.GetFont((int)GameSettings.FontId.Debug);
                }

                return _defaultFont;
            }
        }

        private static Color DefaultColor
        {
            get
            {
                return GameSettings.DebugDefaultColor;
            }
        }

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

        public static void Draw()
        {
            WindowManager windowManager = ServiceLocator.Instance.GetService<WindowManager>();

            foreach (Drawable drawable in Drawables)
            {
                windowManager.Draw(drawable);
            }

            // Clear on every frame after drawing
            Drawables.Clear();
        }

        public static void DrawLine(params Vector2f[] points)
        {
            DrawLine(points, DefaultColor);
        }

        public static void DrawLine(Vector2f[] points, Color color)
        {
            VertexArray vertexArray = new VertexArray
            {
                PrimitiveType = PrimitiveType.LineStrip
            };

            foreach (Vector2f point in points)
            {
                vertexArray.Append(new Vertex(point, color));
            }

            Drawables.Add(vertexArray);
        }

        public static void DrawRect(FloatRect rect)
        {
            DrawRect(rect, DefaultColor);
        }

        public static void DrawRect(FloatRect rect, Color color)
        {
            DrawLine(
                new[] {
                    new Vector2f(rect.Left, rect.Top),
                    new Vector2f(rect.Left + rect.Width, rect.Top),
                    new Vector2f(rect.Left + rect.Width, rect.Top + rect.Height),
                    new Vector2f(rect.Left, rect.Top + rect.Height),
                    new Vector2f(rect.Left, rect.Top)
                }, color);

        }

        public static void DrawText(string message, Vector2f position)
        {
            DrawText(message, position, DefaultColor);
        }

        public static void DrawText(string message, Vector2f position, Color color)
        {
            Text text = new Text(message, DefaultFont, 12);
            text.FillColor = color;
            text.Position = position;

            Drawables.Add(text);
        }
    }
}
