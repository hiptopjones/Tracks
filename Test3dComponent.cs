using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Tracks
{
    internal class Test3dComponent : Drawable3dComponent
    {
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        public int VertexShaderId { get; set; }
        public int FragmentShaderId { get; set; }
        public int TextureId { get; set; }

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }
        private int ElementBufferHandle { get; set; }
        private int VertexCount { get; set; }
        private bool UseElementArray { get; set; }

        private ShaderProgram Shader { get; set; }
        private Texture Texture { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private WindowManager WindowManager { get; set; }

        private TimeSpan ElapsedTime { get; set; }

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            WindowManager = ServiceLocator.Instance.GetService<WindowManager>();

            Shader = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);
            Texture = ResourceManager.GetTexture(TextureId);

            InitializeVertexBufferObject();
        }

        private void InitializeVertexBufferObject()
        {
            VertexCount = Vertices.Length / 5;

            // Generate and bind a vertex array object
            VertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayHandle);

            // Generate and bind a vertex buffer object
            VertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            // Only use element buffers if indices were provided
            UseElementArray = Indices != null && Indices.Any();
            if (UseElementArray)
            {
                // Generate and bind an element buffer object
                ElementBufferHandle = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferHandle);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
            }

            // Specify the layout of the vertex data
            int positionAttribHandle = 0;
            GL.VertexAttribPointer(positionAttribHandle, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionAttribHandle);

            int textureCoordinatesAttribHandle = 1;
            GL.VertexAttribPointer(textureCoordinatesAttribHandle, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(textureCoordinatesAttribHandle);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(0);
        }

        public override void Update(float deltaTime)
        {
            ElapsedTime += TimeSpan.FromSeconds(deltaTime);
        }

        public override void Draw()
        {
            GL.UseProgram(Shader.Handle);
            GL.BindTexture(TextureTarget.Texture2D, Texture.Handle);
            GL.BindVertexArray(VertexArrayHandle);

            Matrix4 model = GetModelMatrix();
            int modelUniformHandle = GL.GetUniformLocation(Shader.Handle, "model");
            GL.UniformMatrix4(modelUniformHandle, false, ref model);

            Matrix4 view = GetViewMatrix();
            int viewUniformHandle = GL.GetUniformLocation(Shader.Handle, "view");
            GL.UniformMatrix4(viewUniformHandle, false, ref view);

            Matrix4 projection = GetProjectionMatrix();
            int projectionUniformHandle = GL.GetUniformLocation(Shader.Handle, "projection");
            GL.UniformMatrix4(projectionUniformHandle, false, ref projection);

            if (UseElementArray)
            {
                GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);
            }

            GL.BindVertexArray(0);  
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }

        private Matrix4 GetModelMatrix()
        {
            const float degreesPerSecond = 90;

            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians((float)ElapsedTime.TotalSeconds * degreesPerSecond) * 1.0f);
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians((float)ElapsedTime.TotalSeconds * degreesPerSecond) * 0.5f);
            return model;
        }

        private Matrix4 GetViewMatrix()
        {
            // Move back slightly
            Matrix4 view = Matrix4.Identity;
            view *= Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            return view;
        }

        private Matrix4 GetProjectionMatrix()
        {
            Matrix4 projection = Matrix4.Identity;
            projection *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), WindowManager.Width / (float)WindowManager.Height, 0.1f, 100.0f);
            return projection;
        }
    }
}
