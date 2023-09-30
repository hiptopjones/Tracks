using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SFML.Graphics;
using SFML.System;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace Tracks
{
    internal class Test3dComponent : Drawable3dComponent
    {
        public Vector3f[] Positions { get; set; }
        public Vector3f[] Colors { get; set; }
        public Vector2f[] TextureCoordinates { get; set; }

        public int VertexShaderId { get; set; }
        public int FragmentShaderId { get; set; }
        public int TextureId { get; set; }

        private int VertexBufferId { get; set; }
        private int VertexArrayId { get; set; }
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
            float[] positionsFlattened = Positions.SelectMany(v => new[] { v.X, v.Y, v.Z }).ToArray();
            float[] colorsFlattened = Colors.SelectMany(v => new[] { v.X, v.Y, v.Z }).ToArray();
            float[] textureCoordinatesFlattened = TextureCoordinates.SelectMany(v => new[] { v.X, v.Y }).ToArray();

            float[] verticesFlattened = InterleaveVertexData(
                InterleaveVertexData(positionsFlattened, 3, colorsFlattened, 3), 6,
                textureCoordinatesFlattened, 2);
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
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            int colorLocation = 1;
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(colorLocation);

            int textureLocation = 2;
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(textureLocation);

            // Unbind the vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object
            GL.BindVertexArray(vertexArrayId);
        }

        private float[] InterleaveVertexData(float[] first, int firstStride, float[] second, int secondStride)
        {
            int firstElementCount = first.Length / firstStride;
            int secondElementCount = second.Length / secondStride;

            if (firstElementCount != secondElementCount)
            {
                throw new Exception($"Input arrays must be of equal size: {first.Length} != {second.Length}");
            }

            float[] output = new float[first.Length + second.Length];

            for (int i = 0; i < first.Length / firstStride; i++)
            {
                int outputIndex = i * (firstStride + secondStride);

                for (int j = 0; j < firstStride; j++)
                {
                    output[outputIndex + j] = first[i * firstStride + j];
                }

                for (int j = 0; j < secondStride; j++)
                {
                    output[outputIndex + firstStride + j] = second[i * secondStride + j];
                }
            }

            return output;
        }

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.ProgramId);
            GL.BindTexture(TextureTarget.Texture2D, Texture.NativeHandle);
            GL.BindVertexArray(VertexArrayId);

            // Rudimentary animation by creating and setting a transform based on the current time
            float seconds = (float)(DateTime.Now.TimeOfDay.TotalSeconds);
            float degreesPerSecond = 90;
            Matrix4 transform = Matrix4.Identity;
            transform *= Matrix4.CreateScale((float)Math.Pow(4, Math.Sin(MathHelper.DegreesToRadians(seconds * degreesPerSecond))));
            transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(seconds * degreesPerSecond));
            transform *= Matrix4.CreateTranslation(new Vector3((float)Math.Sin(MathHelper.DegreesToRadians(seconds * degreesPerSecond)) * 0.5f, 0, 0));
            int transformLocation = GL.GetUniformLocation(ShaderProgram.ProgramId, "transform");
            GL.UniformMatrix4(transformLocation, true, ref transform);

            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount / 3);

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }
    }
}
