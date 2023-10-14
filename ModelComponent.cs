using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class ModelComponent : Drawable3dComponent
    {
        public int ModelId { get; set; }

        public int VertexShaderId { get; set; } = (int)GameSettings.ShaderId.DefaultMeshVertex;
        public int FragmentShaderId { get; set; } = (int)GameSettings.ShaderId.DefaultMeshFragment;

        public int VertexCount => Model.Meshes.Sum(x => x.Vertices.Count);

        private ShaderProgram ShaderProgram { get; set; }
        private Model Model { get; set; }

        private ResourceManager ResourceManager { get; set; }
        private CameraComponent MainCamera { get; set; }

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");

            ShaderProgram = ResourceManager.GetShaderProgram(VertexShaderId, FragmentShaderId);
            Model = ResourceManager.GetModel(ModelId);
        }

        public override void Draw()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.UseProgram(ShaderProgram.Handle);

            ShaderProgram.SetUniform(GetModelMatrix(), "model");
            ShaderProgram.SetUniform(GetViewMatrix(), "view");
            ShaderProgram.SetUniform(GetProjectionMatrix(), "projection");

            Model.Draw(ShaderProgram);

            GL.UseProgram(0);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
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
