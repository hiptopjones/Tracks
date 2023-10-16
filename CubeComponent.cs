using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Reflection;

namespace Tracks
{
    internal class CubeComponent : Drawable3dComponent
    {
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        public ShaderId VertexShaderId { get; set; }
        public ShaderId FragmentShaderId { get; set; }
        public Color4 Color { get; set; } = Color4.White;
        public Color4 LightColor { get; set; } = Color4.White;
        public bool IsLightSource { get; set; }

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }

        private int VertexCount { get; set; }

        private ShaderProgram ShaderProgram { get; set; }
        private Texture Texture { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private CameraComponent MainCamera { get; set; }

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");

            ShaderProgram = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);

            InitializeVertexBufferObject();
        }

        private void InitializeVertexBufferObject()
        {
            VertexCount = Vertices.Length / 8;

            // Generate and bind a vertex array object
            VertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayHandle);

            // Generate and bind a vertex buffer object
            VertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            // Specify the layout of the vertex data
            int positionAttribHandle = 0;
            GL.VertexAttribPointer(positionAttribHandle, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionAttribHandle);

            int normalAttribHandle = 1;
            GL.VertexAttribPointer(normalAttribHandle, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(normalAttribHandle);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(0);
        }

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.Handle);
            GL.BindVertexArray(VertexArrayHandle);

            ShaderProgram.SetUniform("model", GetModelMatrix());
            ShaderProgram.SetUniform("view", GetViewMatrix());
            ShaderProgram.SetUniform("projection", GetProjectionMatrix());

            ShaderProgram.SetUniform("color", Color);
            if (!IsLightSource)
            {
                ShaderProgram.SetUniform("light_color", LightColor);
                ShaderProgram.SetUniform("light_pos", new Vector3(2, 0, -2)); // TODO: Get the real light position
            }

            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);

            GL.BindVertexArray(0);  
            GL.UseProgram(0);
        }

        private Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.Identity;

            // Always scale, then rotation, then translation
            // And in OpenTK, it's represented in that order
            model *= Matrix4.CreateScale(Owner.Transform.Scale);
            model *= Matrix4.CreateFromQuaternion(Owner.Transform.Rotation);
            model *= Matrix4.CreateTranslation(Owner.Transform.Position);

            return model;
        }

        private Matrix4 GetViewMatrix()
        {
            return MainCamera.ViewMatrix;
        }

        private Matrix4 GetProjectionMatrix()
        {
            return MainCamera.ProjectionMatrix;
        }
    }
}
