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

            // Unbind the vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object
            GL.BindVertexArray(vertexArrayId);
        }


        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.ProgramId);
            GL.BindTexture(TextureTarget.Texture2D, Texture.NativeHandle);
            GL.BindVertexArray(VertexArrayId);

            Matrix4 transform = GetTransform();
            int transformLocation = GL.GetUniformLocation(ShaderProgram.ProgramId, "transform");
            GL.UniformMatrix4(transformLocation, true, ref transform);

            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }

        private Matrix4 GetTransform()
        {
            // Rudimentary animation by creating and setting a transform based on the current time
            float seconds = (float)(DateTime.Now.TimeOfDay.TotalSeconds);
            float degreesPerSecond = 90;
            Matrix4 transform = Matrix4.Identity;
            //transform *= Matrix4.CreateScale((float)Math.Pow(4, Math.Sin(MathHelper.DegreesToRadians(seconds * degreesPerSecond))));
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(seconds * degreesPerSecond));
            //transform *= Matrix4.CreateTranslation(new Vector3((float)Math.Sin(MathHelper.DegreesToRadians(seconds * degreesPerSecond)) * 0.5f, 0, 0));

            return transform;
        }
    }
}
