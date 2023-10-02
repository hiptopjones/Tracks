
using NLog;

namespace Tracks
{
    internal class ResourceManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string ResourcesDirectory { get; }
        public string TexturesDirectory { get; }
        public string ShadersDirectory { get; }
        
        private Dictionary<int, Texture> Textures { get; } = new Dictionary<int, Texture>();

        private Dictionary<string, ShaderProgram> ShaderPrograms { get; } = new Dictionary<string, ShaderProgram>();

        public ResourceManager()
        {
            ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, GameSettings.ResourcesDirectoryName);
            TexturesDirectory = Path.Combine(ResourcesDirectory, GameSettings.TexturesDirectoryName);
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

                Logger.Info($"Loading texture: {textureFileName}");

                string textureFilePath = Path.Combine(TexturesDirectory, textureFileName);
                texture = Texture.LoadFromFile(textureFilePath);

                Textures[textureId] = texture;
            }

            return texture;
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

                Logger.Info($"Loading shader program: {vertexShaderFileName} | {fragmentShaderFileName}");

                string vertexShaderFilePath = Path.Combine(ShadersDirectory, vertexShaderFileName);
                string fragmentShaderFilePath = Path.Combine(ShadersDirectory, fragmentShaderFileName);

                shaderProgram = ShaderProgram.LoadFromFile(vertexShaderFilePath, fragmentShaderFilePath);

                ShaderPrograms[shaderProgramKey] = shaderProgram;
            }

            return shaderProgram;
        }
    }
}