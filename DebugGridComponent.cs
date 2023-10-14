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
    // Inspired by https://learnopengl.com/In-Practice/2D-Game/Rendering-Sprites
    internal class DebugGridComponent : Drawable3dComponent
    {
        private float[] Vertices { get; } = GameSettings.QuadVertices;

        private int VertexArrayHandle { get; set; }
        private int VertexBufferHandle { get; set; }
        private int VertexCount { get; set; }

        private ShaderProgram ShaderProgram { get; set; }
        private ResourceManager ResourceManager { get; set; }
        private WindowManager WindowManager { get; set; }
        private CameraComponent MainCamera { get; set; }

        public DebugGridComponent()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            WindowManager = ServiceLocator.Instance.GetService<WindowManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");

            InitializeShaders();
            InitializeVertexBufferObject();
        }

        private void InitializeShaders()
        {
            ShaderId vertexShaderId = ShaderId.DebugGridVertex;
            ShaderId fragmentShaderId = ShaderId.DebugGridFragment;

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
            GL.VertexAttribPointer(positionLocationHandle, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocationHandle);

            // Unbind the vertex buffer object (does not affect vertex array)
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Unbind the vertex array object (also unbinds element buffer)
            GL.BindVertexArray(0);
        }

        public override void Draw()
        {
            GL.UseProgram(ShaderProgram.Handle);
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

            int nearUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "near");
            GL.Uniform1(nearUniformHandle, GameSettings.MainCameraNearClipDistance);

            int farUniformHandle = GL.GetUniformLocation(ShaderProgram.Handle, "far");
            GL.Uniform1(farUniformHandle, GameSettings.MainCameraFarClipDistance);

            GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        private Matrix4 GetModelMatrix()
        {
            return Matrix4.Identity;
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
