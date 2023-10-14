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

        // Main Camera
        public const float MainCameraAspectRatio = WindowWidth / (float)WindowHeight;
        public const float MainCameraFieldOfView = 60f;
        public const float MainCameraNearClipDistance = 0.1f;
        public const float MainCameraFarClipDistance = 100f;

        // UI Camera
        public static readonly Box2 UiCameraBounds = new Box2(0, 0, WindowWidth, WindowHeight);
        public const float UiCameraNearClipDistance = -1f;
        public const float UiCameraFarClipDistance =1f;

        // Diagnostics
        public const int DiagnosticsFpsSampleCount = 100;
        public static readonly Vector3 DiagnosticsPosition = new Vector3(20, WindowHeight - 20, 0);
        public static readonly float DiagnosticsScale = 0.5f;
        public static readonly Vector2 DiagnosticsRowOffset = new Vector2(0, -20);
        public static readonly Vector2 DiagnosticsColumnOffset = new Vector2(150, 0);

        // Splash
        public const string SplashScreenTextureFileName = "Splash.png";
        public const float SplashScreenTransitionDelaySeconds = 2;

        // Test Pattern
        public const string TestPatternTextureFileName = "TestPattern.png";
        public const string TestPaletteTextureFileName = "TestPalette.png";
        public const string BlankTextureFileName = "Blank.png";
        public const string GridPatternTextureFileName = "GridPattern.png";

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
        public const string DebugGridVertexShaderFileName = "DebugGrid.vert";
        public const string DebugGridFragmentShaderFileName = "DebugGrid.frag";

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
            GridPattern,
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
            { (int)TextureId.TestPattern, TestPatternTextureFileName },
            { (int)TextureId.TestPalette, TestPaletteTextureFileName },
            { (int)TextureId.Blank, BlankTextureFileName },
            { (int)TextureId.DebugFont, DebugFontTextureFileName },
            { (int)TextureId.GridPattern, GridPatternTextureFileName },
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
            DebugGridVertex,
            DebugGridFragment,
        }

        public static readonly Dictionary<int, string> Shaders = new Dictionary<int, string>
        {
            { (int)ShaderId.DefaultVertex, DefaultVertexShaderFileName },
            { (int)ShaderId.DefaultFragment, DefaultFragmentShaderFileName },
            { (int)ShaderId.SpriteVertex, SpriteVertexShaderFileName },
            { (int)ShaderId.SpriteFragment, SpriteFragmentShaderFileName },
            { (int)ShaderId.TileVertex, TileVertexShaderFileName },
            { (int)ShaderId.TileFragment, TileFragmentShaderFileName },
            { (int)ShaderId.DebugGridVertex, DebugGridVertexShaderFileName },
            { (int)ShaderId.DebugGridFragment, DebugGridFragmentShaderFileName },
        };

        public static readonly float[] QuadVertices = new[]
        {
            // pos            // tex
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,

            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        };

        public static readonly float[] CubeVertices = new[]
        {
            // pos            // tex
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
