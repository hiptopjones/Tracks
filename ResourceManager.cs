namespace Tracks
{
    internal class ResourceManager
    {
        public string ResourcesDirectory { get; }
        public string TexturesDirectory { get; }
        public string ShadersDirectory { get; }
        
        private Dictionary<int, Texture> Textures { get; } = new Dictionary<int, Texture>();

        private Dictionary<string, Shader> ShaderPrograms { get; } = new Dictionary<string, Shader>();

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

                string textureFilePath = Path.Combine(TexturesDirectory, textureFileName);
                texture = Texture.LoadFromFile(textureFilePath);

                Textures[textureId] = texture;
            }

            return texture;
        }

        public Shader GetShader(int vertexShaderId, int fragmentShaderId)
        {
            string shaderProgramKey = $"{vertexShaderId}/{fragmentShaderId}";

            if (!ShaderPrograms.TryGetValue(shaderProgramKey, out Shader shaderProgram))
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

                shaderProgram = Shader.LoadFromFile(vertexShaderFilePath, fragmentShaderFilePath);

                ShaderPrograms[shaderProgramKey] = shaderProgram;
            }

            return shaderProgram;
        }
    }
}