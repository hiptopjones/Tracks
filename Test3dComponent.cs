using OpenTK.Graphics.OpenGL;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class Test3dComponent : Drawable3dComponent
    {
        public float[] Vertices { get; set; }
        public float[] Colors { get; set; }

        public int VertexShaderId { get; set; }
        public int FragmentShaderId { get; set; }

        private int VertexBufferId { get; set; }
        private int VertexArrayId { get; set; }

        private ShaderProgram ShaderProgram { get; set; }

        private ResourceManager ResourceManager { get; set; } = ServiceLocator.Instance.GetService<ResourceManager>();

        public override void Awake()
        {
            ShaderProgram = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);

            InitializeVertexBufferObject();
        }

        private void InitializeVertexBufferObject()
        {
            // Generate and bind a vertex array object
            GL.GenVertexArrays(1, out int vertexArrayId);
            GL.BindVertexArray(vertexArrayId);
            VertexArrayId = vertexArrayId;

            // Generate and bind a vertex buffer object
            GL.GenBuffers(1, out int vertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferId);
            VertexBufferId = vertexBufferId;

            // Load vertex data into the buffer
            GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    Vertices.Length * sizeof(float),
                    Vertices,
                    BufferUsageHint.StaticDraw);

            // Specify the layout of the vertex data
            int location = 0;
            GL.VertexAttribPointer(location, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(location);

            // Unbind the vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object
            GL.BindVertexArray(vertexArrayId);
        }

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.ProgramId);
            GL.BindVertexArray(VertexArrayId);

            GL.DrawArrays(PrimitiveType.Triangles, 0, Vertices.Length / 3);

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
