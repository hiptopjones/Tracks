using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class ShaderProgram
    {
        public int ProgramId { get; }

        public ShaderProgram(string vertexShaderFilePath, string fragmentShaderFilePath)
        {
            int vertexShaderId = LoadShader(ShaderType.VertexShader, vertexShaderFilePath);
            int fragmentShaderId = LoadShader(ShaderType.FragmentShader, fragmentShaderFilePath);

            ProgramId = LinkShaders(vertexShaderId, fragmentShaderId);

            UnloadShader(vertexShaderId);
            UnloadShader(fragmentShaderId);
        }

        private int LoadShader(ShaderType shaderType, string shaderFilePath)
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

        private void UnloadShader(int shaderId)
        {
            GL.DeleteShader(shaderId);
        }

        private int LinkShaders(int vertexShaderId, int fragmentShaderId)
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
