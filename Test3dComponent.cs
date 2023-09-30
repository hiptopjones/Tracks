using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SFML.Graphics;
using SFML.System;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace Tracks
{
    internal class Test3dComponent : Drawable3dComponent
    {
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        public int VertexShaderId { get; set; }
        public int FragmentShaderId { get; set; }
        public int TextureId { get; set; }

        private int VertexArrayId { get; set; }
        private int VertexBufferId { get; set; }
        private int ElementBufferId { get; set; }
        private int VertexCount { get; set; }

        private ShaderProgram ShaderProgram { get; set; }
        private Texture Texture { get; set; }
        private ResourceManager ResourceManager { get; set; } = ServiceLocator.Instance.GetService<ResourceManager>();

        private TimeSpan ElapsedTime { get; set; }

        public override void Awake()
        {
            ShaderProgram = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);
            Texture = ResourceManager.GetTexture(TextureId);

            InitializeVertexBufferObject();
        }

        private void InitializeVertexBufferObject()
        {
            VertexCount = Vertices.Length;

            // Generate and bind a vertex array object
            GL.GenVertexArrays(1, out int vertexArrayId);
            GL.BindVertexArray(vertexArrayId);
            VertexArrayId = vertexArrayId;

            // Generate and bind a vertex buffer object
            GL.GenBuffers(1, out int vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            VertexBufferId = vertexBufferId;

            // Load vertex data into the buffer
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            // Generate and bind an element buffer object
            GL.GenBuffers(1, out int elementBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferId);
            ElementBufferId = elementBufferId;

            // Load vertex data into the buffer
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            // Specify the layout of the vertex data
            int positionLocation = 0;
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            int textureLocation = 1;
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(textureLocation);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(vertexArrayId);
        }

        public override void Update(float deltaTime)
        {
            ElapsedTime += TimeSpan.FromSeconds(deltaTime);
        }

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.ProgramId);
            GL.BindTexture(TextureTarget.Texture2D, Texture.NativeHandle);
            GL.BindVertexArray(VertexArrayId);

            Matrix4 model = GetModelMatrix();
            int modelLocation = GL.GetUniformLocation(ShaderProgram.ProgramId, "model");
            GL.UniformMatrix4(modelLocation, true, ref model);

            Matrix4 view = GetViewMatrix();
            int viewLocation = GL.GetUniformLocation(ShaderProgram.ProgramId, "view");
            GL.UniformMatrix4(viewLocation, true, ref view);

            Matrix4 projection = GetProjectionMatrix();
            int projectionLocation = GL.GetUniformLocation(ShaderProgram.ProgramId, "projection");
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);  
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }

        private Matrix4 GetModelMatrix()
        {
            const float degreesPerSecond = 90;

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians((float)ElapsedTime.TotalSeconds * degreesPerSecond));
            return model;
        }

        private Matrix4 GetViewMatrix()
        {
            // Move back slightly
            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.09f, -3.0f);
            return view;
        }

        private Matrix4 GetProjectionMatrix()
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), GameSettings.WindowWidth / (float)GameSettings.WindowHeight, 0.1f, 100.0f);
            return projection;
        }
    }
}
