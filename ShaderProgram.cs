using Assimp.Unmanaged;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Xml.Linq;

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
        
        public int GetUniformLocation(string name)
        {
            int uniformHandle = GL.GetUniformLocation(Handle, name);
            if (uniformHandle < 0)
            {
                throw new Exception($"Unable to get uniform location: '{name}'");
            }

            return uniformHandle;
        }

        public void SetUniform(string name, Matrix4 matrix)
        {
            int uniformHandle = GetUniformLocation(name);
            GL.UniformMatrix4(uniformHandle, false, ref matrix);
        }

        public void SetUniform(string name, bool value)
        {
            SetUniform(name, Convert.ToInt32(value));
        }

        public void SetUniform(string name, int value)
        {
            int uniformHandle = GetUniformLocation(name);
            GL.Uniform1(uniformHandle, value);
        }

        public void SetUniform(string name, Color4 value)
        {
            int uniformHandle = GetUniformLocation(name);
            GL.Uniform4(uniformHandle, value);
        }

        public void SetUniform(string name, Vector4 value)
        {
            int uniformHandle = GetUniformLocation(name);
            GL.Uniform4(uniformHandle, value);
        }

        public void SetUniform(string name, Vector3 value)
        {
            int uniformHandle = GetUniformLocation(name);
            GL.Uniform3(uniformHandle, value);
        }

        public void SetUniform(string name, Vector2 value)
        {
            int uniformHandle = GetUniformLocation(name);
            GL.Uniform2(uniformHandle, value);
        }

        public void SetUniform(ColorMap colorMap)
        {
            string namePrefix = $"map_{colorMap.Name}.";
            bool hasTexture = colorMap.Texture != null;

            SetUniform(namePrefix + "color", colorMap.Color);
            SetUniform(namePrefix + "has_tex", hasTexture);

            if (hasTexture)
            {
                // TODO: If attaching multiple textures, will need to index to the proper texture unit
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, colorMap.Texture.Handle);

                SetUniform(namePrefix + "tex", 0);
            }
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
