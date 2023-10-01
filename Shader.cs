using OpenTK.Graphics.OpenGL;

namespace Tracks
{
    internal class Shader
    {
        public int ProgramId { get; private set; }

        public static Shader LoadFromFile(string vertexShaderFilePath, string fragmentShaderFilePath)
        {
            int vertexShaderId = LoadShader(ShaderType.VertexShader, vertexShaderFilePath);
            int fragmentShaderId = LoadShader(ShaderType.FragmentShader, fragmentShaderFilePath);

            int programId = LinkShaders(vertexShaderId, fragmentShaderId);

            UnloadShader(vertexShaderId);
            UnloadShader(fragmentShaderId);

            return new Shader
            {
                ProgramId = programId
            };
        }

        private static int LoadShader(ShaderType shaderType, string shaderFilePath)
        {
            string shaderText = File.ReadAllText(shaderFilePath);

            int shaderId = GL.CreateShader(shaderType);
            GL.ShaderSource(shaderId, shaderText);
            GL.CompileShader(shaderId);

            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shaderId);
                throw new Exception($"Error compiling shader {Enum.GetName(shaderType)}: {infoLog}");
            }
    
            return shaderId;
        }

        private static void UnloadShader(int shaderId)
        {
            GL.DeleteShader(shaderId);
        }

        private static int LinkShaders(int vertexShaderId, int fragmentShaderId)
        {
            int programId = GL.CreateProgram();
            GL.AttachShader(programId, vertexShaderId);
            GL.AttachShader(programId, fragmentShaderId);
            GL.LinkProgram(programId);

            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(programId);
                throw new Exception($"Error linking shader program: {infoLog}");
            }

            return programId;
        }
    }
}
