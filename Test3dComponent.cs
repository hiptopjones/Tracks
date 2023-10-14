using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Reflection;

namespace Tracks
{
    internal class Test3dComponent : Drawable3dComponent
    {
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        public int VertexShaderId { get; set; }
        public int FragmentShaderId { get; set; }
        public int TextureId { get; set; }
        public Color4 Color { get; set; } = Color4.White;

        public bool IsWireframe { get; set; }

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }
        private int ElementBufferHandle { get; set; }
        private int VertexCount { get; set; }
        private bool UseElementArray { get; set; }

        private ShaderProgram ShaderProgram { get; set; }
        private Texture Texture { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private CameraComponent MainCamera { get; set; }

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");

            ShaderProgram = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);
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

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.Handle);
            GL.BindTexture(TextureTarget.Texture2D, Texture.Handle);
            GL.BindVertexArray(VertexArrayHandle);

            Matrix4 model = GetModelMatrix();
            int modelUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "model");
            GL.UniformMatrix4(modelUniformHandle, false, ref model);

            Matrix4 view = GetViewMatrix();
            int viewUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "view");
            GL.UniformMatrix4(viewUniformHandle, false, ref view);

            Matrix4 projection = GetProjectionMatrix();
            int projectionUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "projection");
            GL.UniformMatrix4(projectionUniformHandle, false, ref projection);

            int colorUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "color");
            GL.Uniform4(colorUniformHandle, Color);

            if (IsWireframe)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

                int isWireframeUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "isWireframe");
                GL.Uniform1(isWireframeUniformHandle, Convert.ToInt32(IsWireframe));
            }

            if (UseElementArray)
            {
                GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);
            }

            if (IsWireframe)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            GL.BindVertexArray(0);  
            GL.BindTexture(TextureTarget.Texture2D, 0);
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
