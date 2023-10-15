using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Tracks
{
    internal class ModelComponent : Drawable3dComponent
    {
        public ModelId ModelId { get; set; }

        public ShaderId VertexShaderId { get; set; } = ShaderId.DefaultMeshVertex;
        public ShaderId FragmentShaderId { get; set; } = ShaderId.DefaultMeshFragment;

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
            GL.UseProgram(ShaderProgram.Handle);

            ShaderProgram.SetUniform("view", GetViewMatrix());
            ShaderProgram.SetUniform("projection", GetProjectionMatrix());

            Model.Draw(ShaderProgram, GetModelMatrix());

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
