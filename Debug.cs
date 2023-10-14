using NLog;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;

namespace Tracks
{
    internal static class Debug
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static List<Text> TextDrawables { get; } = new List<Text>();
        private static List<Sprite> SpriteDrawables { get; } = new List<Sprite>();

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

        public static void DrawText(string message, Vector2 position)
        {
            DrawText(message, position, Color4.White);
        }

        public static void DrawText(string message, Vector2 position, Color4 color)
        {
            DrawText(message, position, color, 1.0f);
        }

        public static void DrawText(string message, Vector2 position, Color4 color, float scale)
        {
            FontAtlas fontAtlas = ServiceLocator.Instance.GetService<FontAtlas>();
            Text text = new Text(message, position, fontAtlas, color, scale);

            TextDrawables.Add(text);
        }

        public static void DrawImage(int textureId, Vector2 position)
        {
            DrawImage(textureId, position, Vector2.One);
        }

        public static void DrawImage(int textureId, Vector2 position, Vector2 scale)
        {
            DrawImage(textureId, position, scale, Vector2.Zero);
        }

        public static void DrawImage(int textureId, Vector2 position, Vector2 scale, Vector2 normalizedOrigin)
        {
            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            Texture texture = resourceManager.GetTexture(textureId);

            Vector2 origin = new Vector2(normalizedOrigin.X * texture.Width, normalizedOrigin.Y * texture.Height);
            Sprite sprite = new Sprite(texture, position, scale, origin);
            SpriteDrawables.Add(sprite);
        }

        public static void Draw()
        {
            foreach (Sprite sprite in SpriteDrawables)
            {
                sprite.Draw();
            }

            foreach (Text text in TextDrawables)
            {
                text.Draw();
            }

            // Clear on every frame
            SpriteDrawables.Clear();
            TextDrawables.Clear();
        }
    }
}
