using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Reflection.Metadata;

namespace Tracks
{
    internal class Mesh
    {
        public string Name { get; private set; }
        public List<Vertex> Vertices { get; private set; }
        public List<int> Indices { get; private set; }
        public List<ColorMap> ColorMaps { get; private set; }
        public Matrix4 Transform { get; private set; }

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }
        private int ElementBufferHandle { get; set; }

        private bool AreAttributesBound { get; set; }

        // Constants
        private const int Vector3StructFloatCount = 3;
        private const int Vector2StructFloatCount = 2;
        private const int VertexStructFloatCount = Vector3StructFloatCount + Vector2StructFloatCount;

        private const int PositionFloatStartIndex = 0;
        private const int TexCoordsFloatStartIndex = PositionFloatStartIndex + Vector3StructFloatCount;

        public Mesh(string name, List<Vertex> vertices, List<int> indices, List<ColorMap> colorMaps, Matrix4 transform)
        {
            Name = name;
            Vertices = vertices;
            Indices = indices;
            ColorMaps = colorMaps;
            Transform = transform;

            Initialize();
        }

        public void Draw(ShaderProgram shaderProgram)
        {
            GL.BindVertexArray(VertexArrayHandle);

            if (!AreAttributesBound)
            {
                BindAttributes(shaderProgram);
                AreAttributesBound = true;
            }

            foreach (ColorMap colorMap in ColorMaps)
            {
                shaderProgram.SetUniform(colorMap);
            }

            GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        private void Initialize()
        {
            // Generate and bind a vertex array object
            VertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayHandle);

            // Generate and bind a vertex buffer object
            VertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * VertexStructFloatCount * sizeof(float), Vertices.ToFloatArray(), BufferUsageHint.StaticDraw);

            // Generate and bind an element buffer object
            ElementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(0);
        }

        private void BindAttributes(ShaderProgram shaderProgram)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);

            int positionAttribHandle = GL.GetAttribLocation(shaderProgram.Handle, "vs_in_pos");
            if (positionAttribHandle < 0)
            {
                throw new Exception("Unable to get attribute location");
            }

            GL.VertexAttribPointer(positionAttribHandle, Vector3StructFloatCount, VertexAttribPointerType.Float, false, VertexStructFloatCount * sizeof(float), PositionFloatStartIndex);
            GL.EnableVertexAttribArray(positionAttribHandle);

            int textureCoordinatesAttribHandle = GL.GetAttribLocation(shaderProgram.Handle, "vs_in_texcoord");
            if (textureCoordinatesAttribHandle < 0)
            {
                throw new Exception("Unable to get attribute location");
            }

            GL.VertexAttribPointer(textureCoordinatesAttribHandle, Vector2StructFloatCount, VertexAttribPointerType.Float, false, VertexStructFloatCount * sizeof(float), TexCoordsFloatStartIndex * sizeof(float));
            GL.EnableVertexAttribArray(textureCoordinatesAttribHandle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
