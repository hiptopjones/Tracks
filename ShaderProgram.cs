using OpenTK.Graphics.OpenGL;

namespace Tracks
{
    internal class ShaderProgram
    {
        public int Handle { get; private set; }

        public static ShaderProgram LoadFromFile(string vertexShaderFilePath, string fragmentShaderFilePath)
        {
            int vertexShaderHandle = LoadShader(ShaderType.VertexShader, vertexShaderFilePath);
            int fragmentShaderHandle = LoadShader(ShaderType.FragmentShader, fragmentShaderFilePath);

            int programHandle = LinkShaders(vertexShaderHandle, fragmentShaderHandle);

            UnloadShader(vertexShaderHandle);
            UnloadShader(fragmentShaderHandle);

            return new ShaderProgram
            {
                Handle = programHandle
            };
        }

        private static int LoadShader(ShaderType shaderType, string shaderFilePath)
        {
            string shaderText = File.ReadAllText(shaderFilePath);

            int shaderHandle = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderHandle, shaderText);
            GL.CompileShader(shaderHandle);

            GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shaderHandle);
                throw new Exception($"Error compiling shader {Enum.GetName(shaderType)}: {infoLog}");
            }
    
            return shaderHandle;
        }

        private static void UnloadShader(int shaderId)
        {
            GL.DeleteShader(shaderId);
        }

        private static int LinkShaders(int vertexShaderHandle, int fragmentShaderHandle)
        {
            int programHandle = GL.CreateProgram();
            GL.AttachShader(programHandle, vertexShaderHandle);
            GL.AttachShader(programHandle, fragmentShaderHandle);
            GL.LinkProgram(programHandle);

            GL.GetProgram(programHandle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(programHandle);
                throw new Exception($"Error linking shader program: {infoLog}");
            }

            return programHandle;
        }
    }
}
