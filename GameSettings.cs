using OpenTK.Mathematics;

namespace Tracks
{
    internal class GameSettings
    {
        public const string GameName = "Tracks";

        // Window
        public const int WindowWidth = 800;
        public const int WindowHeight = 600;
        public static readonly Color4 WindowClearColor = new Color4(30, 30, 30, 0);

        // Stats
        public const int StatsFpsSampleCount = 100;

        // Debug
        public static readonly Color4 DebugDefaultColor = Color4.Magenta;
        public const string DebugFontFileName = "unispace rg.ttf";

        // Splash
        public const string SplashScreenTextureFileName = "Splash.png";
        public const float SplashScreenTransitionDelaySeconds = 2;

        // Test Pattern
        public const string TestPatternTextureFileName = "TestPattern.png";

        // Shaders
        public const string DefaultVertexShaderFileName = "Default.vert";
        public const string DefaultFragmentShaderFileName = "Default.frag";

        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";
        public const string FontsDirectoryName = "Fonts";
        public const string ShadersDirectoryName = "Shaders";

        // Textures
        public enum TextureId
        {
            SplashScreen,
            TestPattern
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
            { (int)TextureId.TestPattern, TestPatternTextureFileName },
        };

        // Fonts
        public enum FontId
        {
            Debug
        }

        public static readonly Dictionary<int, string> Fonts = new Dictionary<int, string>
        {
            { (int)FontId.Debug, DebugFontFileName },
        };

        // Shaders
        public enum ShaderId
        {
            DefaultVertex,
            DefaultFragment
        }

        public static readonly Dictionary<int, string> Shaders = new Dictionary<int, string>
        {
            { (int)ShaderId.DefaultVertex, DefaultVertexShaderFileName },
            { (int)ShaderId.DefaultFragment, DefaultFragmentShaderFileName },
        };
    }
}
