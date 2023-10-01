using OpenTK.Mathematics;

namespace Tracks
{
    internal class CameraComponent : Component
    {
        public Vector3 Up { get; set; } = new Vector3(0.0f, 1.0f, 0.0f);
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearClippingDistance { get; set; }
        public float FarClippingDistance { get; set; }

        public Vector3 Target { get; set; }

        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ProjectionMatrix { get; private set; }

        public override void Awake()
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), AspectRatio, NearClippingDistance, FarClippingDistance);
            UpdateViewMatrix();
        }

        public override void Update(float deltaTime)
        {
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4.LookAt(Owner.Transform.Position, Owner.Transform.Position + Target, Up);
        }
    }
}
