using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class GameSettings
    {
        public const string GameName = "Tracks";

        // Window
        public const int WindowWidth = 1600;
        public const int WindowHeight = 1200;
        public static readonly Color WindowClearColor = new Color(30, 30, 30);

        // Stats
        public const int StatsFpsSampleCount = 100;

        // Debug
        public static readonly Color DebugDefaultColor = Color.Magenta;
        public const string DebugFontFileName = "unispace rg.ttf";

        // Splash
        public const string SplashScreenTextureFileName = "Splash.png";
        public const float SplashScreenTransitionDelaySeconds = 2;
        
        // Resources
        public const string ResourcesDirectoryName = "Assets";
        public const string TexturesDirectoryName = "Textures";
        public const string FontsDirectoryName = "Fonts";

        // Textures
        public enum TextureId
        {
            SplashScreen
        }

        public static readonly Dictionary<int, string> Textures = new Dictionary<int, string>
        {
            { (int)TextureId.SplashScreen, SplashScreenTextureFileName },
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
    }
}
