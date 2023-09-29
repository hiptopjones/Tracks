using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class ResourceManager
    {
        public string ResourcesDirectory { get; }
        public string TexturesDirectory { get; }
        public string FontsDirectory { get; }

        private Dictionary<int, Texture> Textures { get; } = new Dictionary<int, Texture>();

        private Dictionary<int, Font> Fonts { get; } = new Dictionary<int, Font>();

        public ResourceManager()
        {
            ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, GameSettings.ResourcesDirectoryName);
            TexturesDirectory = Path.Combine(ResourcesDirectory, GameSettings.TexturesDirectoryName);
            FontsDirectory = Path.Combine(ResourcesDirectory, GameSettings.FontsDirectoryName);
        }

        public Texture GetTexture(int textureId)
        {
            if (!Textures.TryGetValue(textureId, out Texture texture))
            {
                if (!GameSettings.Textures.TryGetValue(textureId, out string textureFileName))
                {
                    throw new Exception($"Unable to locate a file path for texture: {textureId}");
                }

                string textureFilePath = Path.Combine(TexturesDirectory, textureFileName);
                texture = new Texture(textureFilePath);

                Textures[textureId] = texture;
            }

            return texture;
        }

        public Font GetFont(int fontId)
        {
            if (!Fonts.TryGetValue(fontId, out Font font))
            {
                if (!GameSettings.Fonts.TryGetValue(fontId, out string fontFileName))
                {
                    throw new Exception($"Unable to locate a file path for font: {fontId}");
                }

                string fontFilePath = Path.Combine(FontsDirectory, fontFileName);
                font = new Font(fontFilePath);

                Fonts[fontId] = font;
            }

            return font;
        }
    }
}