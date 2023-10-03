using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class TileRenderer
    {
        private static float[] Vertices { get; } = new[]
        {
            // pos            // tex
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,

            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f
        };

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }
        private int VertexCount { get; set; }

        private ShaderProgram ShaderProgram { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private WindowManager WindowManager { get; set; }

        public TileRenderer()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            WindowManager = ServiceLocator.Instance.GetService<WindowManager>();

            InitializeShaders();
            InitializeVertexBufferObject();
        }

        private void InitializeShaders()
        {
            int vertexShaderId = (int)ShaderId.TileVertex;
            int fragmentShaderId = (int)ShaderId.TileFragment;

            ShaderProgram = ResourceManager.GetShaderProgram(vertexShaderId, fragmentShaderId);
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

            // Specify the layout of the vertex data
            int positionLocationHandle = 0;
            GL.VertexAttribPointer(positionLocationHandle, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocationHandle);

            int textureCoordinatesLocationHandle = 1;
            GL.VertexAttribPointer(textureCoordinatesLocationHandle, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(textureCoordinatesLocationHandle);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(0);
        }

        public void Draw(Tile tile)
        {
            GL.UseProgram(ShaderProgram.Handle);
            GL.BindTexture(TextureTarget.Texture2DArray, tile.TextureArray.Handle);
            GL.BindVertexArray(VertexArrayHandle);

            Matrix4 model = GetModelMatrix(tile);
            int modelUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "model");
            GL.UniformMatrix4(modelUniformHandle, false, ref model);

            Matrix4 projection = GetProjectionMatrix();
            int projectionUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "projection");
            GL.UniformMatrix4(projectionUniformHandle, false, ref projection);

            int layerUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "layer");
            GL.Uniform1(layerUniformHandle, tile.TileIndex);

            int colorUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "color");
            GL.Uniform4(colorUniformHandle, tile.Color);

            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.UseProgram(0);
        }

        private Matrix4 GetModelMatrix(Tile tile)
        {
            Matrix4 model = Matrix4.Identity;

            // Quad vertices are from 0,0 to 1,1, so this scaling
            // enables using the same VAO for all sprites
            model *= Matrix4.CreateScale(tile.TextureArray.TileWidth, tile.TextureArray.TileWidth, 1.0f);

            // Positions the sprite correctly
            model *= Matrix4.CreateTranslation(new Vector3(tile.Position.X, tile.Position.Y, 0));

            model *= Matrix4.CreateScale(tile.Scale.X, tile.Scale.Y, 1.0f);

            return model;
        }

        private Matrix4 GetProjectionMatrix()
        {
            Matrix4 projection = Matrix4.CreateOrthographic(WindowManager.Width, WindowManager.Height, -1.0f, 1.0f);
            return projection;
        }
    }
}
