using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;

namespace Tracks
{
    internal class Mesh
    {
        public List<Vertex> Vertices { get; private set; }
        public List<int> Indices { get; private set; }
        public List<Texture> Textures { get; private set; }

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }
        private int ElementBufferHandle { get; set; }

        public Mesh(List<Vertex> vertices, List<int> indices, List<Texture> textures)
        {
            Vertices = vertices;
            Indices = indices;
            Textures = textures;

            Initialize();
        }

        public void Draw(ShaderProgram shaderProgram)
        {
            GL.BindVertexArray(VertexArrayHandle);

            GL.DrawElements(PrimitiveType.Triangles, Indices.Count, DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        private void Initialize()
        {
            int vector3StructFloatCount = 3;
            int vector2StructFloatCount = 2;
            int vertexStructFloatCount = vector3StructFloatCount + vector2StructFloatCount;

            int positionFloatStartIndex = 0;
            int texCoordsFloatStartIndex = positionFloatStartIndex + vector3StructFloatCount;

            // Generate and bind a vertex array object
            VertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayHandle);

            // Generate and bind a vertex buffer object
            VertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * vertexStructFloatCount * sizeof(float), ToFloatArray(Vertices), BufferUsageHint.StaticDraw);

            // Generate and bind an element buffer object
            ElementBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);

            // Specify the layout of the vertex data
            int positionAttribHandle = 0;
            GL.VertexAttribPointer(positionAttribHandle, vector3StructFloatCount, VertexAttribPointerType.Float, false, vertexStructFloatCount * sizeof(float), positionFloatStartIndex);
            GL.EnableVertexAttribArray(positionAttribHandle);

            int textureCoordinatesAttribHandle = 1;
            GL.VertexAttribPointer(textureCoordinatesAttribHandle, vector2StructFloatCount, VertexAttribPointerType.Float, false, vertexStructFloatCount * sizeof(float), texCoordsFloatStartIndex * sizeof(float));
            GL.EnableVertexAttribArray(textureCoordinatesAttribHandle);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(0);

        }

        private float[] ToFloatArray(List<Vertex> vertexStructs)
        {
            List<float> vertexFloats = new List<float>();

            foreach (Vertex vertexStruct in vertexStructs)
            {
                vertexFloats.Add(vertexStruct.Position.X);
                vertexFloats.Add(vertexStruct.Position.Y);
                vertexFloats.Add(vertexStruct.Position.Z);

                vertexFloats.Add(vertexStruct.TexCoords.X);
                vertexFloats.Add(vertexStruct.TexCoords.Y);
            }

            return vertexFloats.ToArray();
        }
    }
}
