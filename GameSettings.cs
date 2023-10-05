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

        // Splash
        public const string SplashScreenTextureFileName = "Splash.png";
        public const float SplashScreenTransitionDelaySeconds = 2;

        // Test Pattern
        public const string TestPatternTextureFileName = "TestPattern.png";
        public const string TestPaletteTextureFileName = "TestPalette.png";
        public const string BlankTextureFileName = "Blank.png";

        // Debug
        public const string DebugFontTextureFileName = "CascadiaMono33.png";
        public static readonly Vector2i DebugFontGlyphSize = new Vector2i(21, 28);
        public const int DebugFontGlyphCount = 94;

        // Shaders
        public const string DefaultVertexShaderFileName = "Default.vert";
        public const string DefaultFragmentShaderFileName = "Default.frag";
        public const string SpriteVertexShaderFileName = "Sprite.vert";
        public const string SpriteFragmentShaderFileName = "Sprite.frag";
        public const string TileVertexShaderFileName = "Tile.vert";
        public const string TileFragmentShaderFileName = "Tile.frag";

        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";
        public const string FontsDirectoryName = "Fonts";
        public const string ShadersDirectoryName = "Shaders";

        // Textures
        public enum TextureId
        {
            SplashScreen,
            TestPattern,
            TestPalette,
            Blank,
            DebugFont,
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
            { (int)TextureId.TestPattern, TestPatternTextureFileName },
            { (int)TextureId.TestPalette, TestPaletteTextureFileName },
            { (int)TextureId.Blank, BlankTextureFileName },
            { (int)TextureId.DebugFont, DebugFontTextureFileName },
        };

        // Shaders
        public enum ShaderId
        {
            DefaultVertex,
            DefaultFragment,
            SpriteVertex,
            SpriteFragment,
            TileVertex,
            TileFragment,
        }

        public static readonly Dictionary<int, string> Shaders = new Dictionary<int, string>
        {
            { (int)ShaderId.DefaultVertex, DefaultVertexShaderFileName },
            { (int)ShaderId.DefaultFragment, DefaultFragmentShaderFileName },
            { (int)ShaderId.SpriteVertex, SpriteVertexShaderFileName },
            { (int)ShaderId.SpriteFragment, SpriteFragmentShaderFileName },
            { (int)ShaderId.TileVertex, TileVertexShaderFileName },
            { (int)ShaderId.TileFragment, TileFragmentShaderFileName },
        };

        public static readonly float[] CubeVertices = new[]
        {
            // Position           Texture coordinates
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
        };
    }
}
