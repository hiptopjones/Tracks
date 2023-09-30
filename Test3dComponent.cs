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
        public Vector3f[] Positions { get; set; }
        public Vector3f[] Colors { get; set; }

        public int VertexShaderId { get; set; }
        public int FragmentShaderId { get; set; }

        private int VertexBufferId { get; set; }
        private int VertexArrayId { get; set; }
        private int VertexCount { get; set; }

        private ShaderProgram ShaderProgram { get; set; }
        private ResourceManager ResourceManager { get; set; } = ServiceLocator.Instance.GetService<ResourceManager>();

        public override void Awake()
        {
            ShaderProgram = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);

            InitializeVertexBufferObject();
        }

        private void InitializeVertexBufferObject()
        {
            float[] positionsFlattened = Positions.SelectMany(v => new[] { v.X, v.Y, v.Z }).ToArray();
            float[] colorsFlattened = Colors.SelectMany(v => new[] { v.X, v.Y, v.Z }).ToArray();

            float[] verticesFlattened = InterleaveVertexData(positionsFlattened, 3, colorsFlattened, 3);
            VertexCount = verticesFlattened.Length;

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
                    verticesFlattened.Length * sizeof(float),
                    verticesFlattened,
                    BufferUsageHint.StaticDraw);

            // Specify the layout of the vertex data
            int positionLocation = 0;
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            int colorLocation = 1;
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(colorLocation);

            // Unbind the vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object
            GL.BindVertexArray(vertexArrayId);
        }

        private float[] InterleaveVertexData(float[] first, int firstStride, float[] second, int secondStride)
        {
            if (first.Length != second.Length)
            {
                throw new Exception($"Input arrays must be of equal size: {first.Length} != {second.Length}");
            }

            float[] output = new float[first.Length + second.Length];

            for (int i = 0; i < first.Length / firstStride; i++)
            {
                int outputIndex = i * (firstStride + secondStride);

                output[outputIndex + 0] = first[i * firstStride + 0];
                output[outputIndex + 1] = first[i * firstStride + 1];
                output[outputIndex + 2] = first[i * firstStride + 2];

                output[outputIndex + 3] = second[i * secondStride + 0];
                output[outputIndex + 4] = second[i * secondStride + 1];
                output[outputIndex + 5] = second[i * secondStride + 2];
            }

            return output;
        }

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.ProgramId);
            GL.BindVertexArray(VertexArrayId);

            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount / 3);

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
