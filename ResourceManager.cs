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
        public string ShadersDirectory { get; }
        
        private Dictionary<int, Texture> Textures { get; } = new Dictionary<int, Texture>();

        private Dictionary<int, Font> Fonts { get; } = new Dictionary<int, Font>();

        private Dictionary<string, ShaderProgram> ShaderPrograms { get; } = new Dictionary<string, ShaderProgram>();

        public ResourceManager()
        {
            ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, GameSettings.ResourcesDirectoryName);
            TexturesDirectory = Path.Combine(ResourcesDirectory, GameSettings.TexturesDirectoryName);
            FontsDirectory = Path.Combine(ResourcesDirectory, GameSettings.FontsDirectoryName);
            ShadersDirectory = Path.Combine(ResourcesDirectory, GameSettings.ShadersDirectoryName);
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

        public ShaderProgram GetShaderProgram(int vertexShaderId, int fragmentShaderId)
        {
            string shaderProgramKey = $"{vertexShaderId}/{fragmentShaderId}";

            if (!ShaderPrograms.TryGetValue(shaderProgramKey, out ShaderProgram shaderProgram))
            {
                if (!GameSettings.Shaders.TryGetValue(vertexShaderId, out string vertexShaderFileName))
                {
                    throw new Exception($"Unable to locate a file path for vertex shader: {vertexShaderId}");
                }

                if (!GameSettings.Shaders.TryGetValue(fragmentShaderId, out string fragmentShaderFileName))
                {
                    throw new Exception($"Unable to locate a file path for fragment shader: {fragmentShaderId}");
                }

                string vertexShaderFilePath = Path.Combine(ShadersDirectory, vertexShaderFileName);
                string fragmentShaderFilePath = Path.Combine(ShadersDirectory, fragmentShaderFileName);

                shaderProgram = new ShaderProgram(vertexShaderFilePath, fragmentShaderFilePath);

                ShaderPrograms[shaderProgramKey] = shaderProgram;
            }

            return shaderProgram;
        }
    }
}