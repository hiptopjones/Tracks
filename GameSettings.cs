using OpenTK.Mathematics;
using static System.Net.WebRequestMethods;

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
        public const float MainCameraFarClipDistance = 10000f;

        // UI Camera
        public static readonly Box2 UiCameraBounds = new Box2(0, 0, WindowWidth, WindowHeight);
        public const float UiCameraNearClipDistance = -1f;
        public const float UiCameraFarClipDistance = 1f;

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
        public const string DefaultMeshVertexShaderFileName = "DefaultMesh.vert";
        public const string DefaultMeshFragmentShaderFileName = "DefaultMesh.frag";
        public const string LightSourceVertexShaderFileName = "LightSource.vert";
        public const string LightSourceFragmentShaderFileName = "LightSource.frag";
        public const string IlluminatedTargetVertexShaderFileName = "IlluminatedTarget.vert";
        public const string IlluminatedTargetFragmentShaderFileName = "IlluminatedTarget.frag";

        // Models
        public const string LowPolyCarModelFileName = "LowPolyCar.fbx";
        public const string CartoonCarModelFileName = "CartoonCar.fbx";
        public const string RaceCarModelFileName = "RaceCar.fbx";
        public const string JeepModelFileName = "Jeep.fbx";

        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";
        public const string FontsDirectoryName = "Fonts";
        public const string ShadersDirectoryName = "Shaders";
        public const string ModelsDirectoryName = "Models";

        // Textures
        public static readonly Dictionary<TextureId, string> Textures = new Dictionary<TextureId, string>
        {
            { TextureId.SplashScreen, SplashScreenTextureFileName },
            { TextureId.TestPattern, TestPatternTextureFileName },
            { TextureId.TestPalette, TestPaletteTextureFileName },
            { TextureId.Blank, BlankTextureFileName },
            { TextureId.DebugFont, DebugFontTextureFileName },
            { TextureId.GridPattern, GridPatternTextureFileName },
        };

        // Shaders
        public static readonly Dictionary<ShaderId, string> Shaders = new Dictionary<ShaderId, string>
        {
            { ShaderId.DefaultVertex, DefaultVertexShaderFileName },
            { ShaderId.DefaultFragment, DefaultFragmentShaderFileName },
            { ShaderId.SpriteVertex, SpriteVertexShaderFileName },
            { ShaderId.SpriteFragment, SpriteFragmentShaderFileName },
            { ShaderId.TileVertex, TileVertexShaderFileName },
            { ShaderId.TileFragment, TileFragmentShaderFileName },
            { ShaderId.DebugGridVertex, DebugGridVertexShaderFileName },
            { ShaderId.DebugGridFragment, DebugGridFragmentShaderFileName },
            { ShaderId.DefaultMeshVertex, DefaultMeshVertexShaderFileName },
            { ShaderId.DefaultMeshFragment, DefaultMeshFragmentShaderFileName },
            { ShaderId.LightSourceVertex, LightSourceVertexShaderFileName },
            { ShaderId.LightSourceFragment, LightSourceFragmentShaderFileName },
            { ShaderId.IlluminatedTargetVertex, IlluminatedTargetVertexShaderFileName },
            { ShaderId.IlluminatedTargetFragment, IlluminatedTargetFragmentShaderFileName },
        };

        // Models
        public static readonly Dictionary<ModelId, string> Models = new Dictionary<ModelId, string>
        {
            { ModelId.LowPolyCar, LowPolyCarModelFileName }, // https://sketchfab.com/3d-models/low-poly-car-fcdb0c27f7d04d47a518a249ae7093a2
            { ModelId.CartoonCar, CartoonCarModelFileName }, // https://www.turbosquid.com/3d-models/cartoon-car-3d-model-2127680
            { ModelId.RaceCar, RaceCarModelFileName }, // https://www.turbosquid.com/3d-models/3d-model-car-lowpoly-2119296
            { ModelId.Jeep, JeepModelFileName }, // https://www.turbosquid.com/3d-models/3d-model-lowpoy-suv-car-1937266
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
             // position          // normal           // uv
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f
        };
    }

    public enum ModelId
    {
        None,
        LowPolyCar,
        CartoonCar,
        RaceCar,
        Jeep,
    }

    public enum ShaderId
    {
        None,
        DefaultVertex,
        DefaultFragment,
        SpriteVertex,
        SpriteFragment,
        TileVertex,
        TileFragment,
        DebugGridVertex,
        DebugGridFragment,
        DefaultMeshVertex,
        DefaultMeshFragment,
        LightSourceVertex,
        LightSourceFragment,
        IlluminatedTargetVertex,
        IlluminatedTargetFragment,
    }

    public enum TextureId
    {
        None,
        SplashScreen,
        TestPattern,
        TestPalette,
        Blank,
        DebugFont,
        GridPattern,
    }
}
