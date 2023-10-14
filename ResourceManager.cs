
using NLog;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class ResourceManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string ResourcesDirectory { get; }
        public string TexturesDirectory { get; }
        public string ShadersDirectory { get; }
        public string ModelsDirectory { get; }

        private Dictionary<int, Texture> Textures { get; } = new Dictionary<int, Texture>();
        private Dictionary<int, TextureArray> TextureArrays { get; } = new Dictionary<int, TextureArray>();
        private Dictionary<string, ShaderProgram> ShaderPrograms { get; } = new Dictionary<string, ShaderProgram>();
        private Dictionary<int, Model> Models { get; } = new Dictionary<int, Model>();

        public ResourceManager()
        {
            ResourcesDirectory = Path.Combine(Environment.CurrentDirectory, GameSettings.ResourcesDirectoryName);
            TexturesDirectory = Path.Combine(ResourcesDirectory, GameSettings.TexturesDirectoryName);
            ShadersDirectory = Path.Combine(ResourcesDirectory, GameSettings.ShadersDirectoryName);
            ModelsDirectory = Path.Combine(ResourcesDirectory, GameSettings.ModelsDirectoryName);
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

        public TextureArray GetTextureArray(int textureId, int tileWidth, int tileHeight, int tileCount)
        {
            if (!TextureArrays.TryGetValue(textureId, out TextureArray texture))
            {
                if (!GameSettings.Textures.TryGetValue(textureId, out string textureFileName))
                {
                    throw new Exception($"Unable to locate a file path for texture: {textureId}");
                }

                Logger.Info($"Loading texture array: {textureFileName}");

                string textureFilePath = Path.Combine(TexturesDirectory, textureFileName);
                texture = TextureArray.LoadFromFile(textureFilePath, tileWidth, tileHeight, tileCount);

                TextureArrays[textureId] = texture;
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

        public Model GetModel(int modelId)
        {
            if (!Models.TryGetValue(modelId, out Model model))
            {
                if (!GameSettings.Models.TryGetValue(modelId, out string modelFileName))
                {
                    throw new Exception($"Unable to locate a file path for model: {modelId}");
                }

                Logger.Info($"Loading model: {modelFileName}");

                string modelFilePath = Path.Combine(ModelsDirectory, modelFileName);
                model = Model.LoadFromFile(modelFilePath);

                Models[modelId] = model;
            }

            return model;
        }
    }
}