using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tracks
{
    internal class CameraComponent : Component
    {
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearClippingDistance { get; set; }
        public float FarClippingDistance { get; set; }

        public GameObject Target { get; set; }

        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ProjectionMatrix { get; private set; }

        private bool IsUsingPerspective { get; set; } = true;

        private WindowManager WindowManager { get; set; }
        private InputManager InputManager { get; set; }

        public override void Awake()
        {
            WindowManager = ServiceLocator.Instance.GetService<WindowManager>();
            WindowManager.Resized += OnWindowResized;

            InputManager = ServiceLocator.Instance.GetService<InputManager>();

            UpdateProjectionMatrix();
            UpdateViewMatrix();
        }

        private void OnWindowResized(object sender, ResizeEventArgs e)
        {
            AspectRatio = WindowManager.Width / (float)WindowManager.Height;

            UpdateProjectionMatrix();
        }

        public override void Update(float deltaTime)
        {
            if (InputManager.IsKeyDown(Keys.P))
            {
                IsUsingPerspective = !IsUsingPerspective;
            }

            UpdateProjectionMatrix();
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            if (Target != null)
            {
                ViewMatrix = Matrix4.LookAt(Owner.Transform.Position, Target.Transform.Position, Vector3.UnitY);
            }
            else
            {
                ViewMatrix = Matrix4.LookAt(Owner.Transform.Position, Owner.Transform.Forward, Vector3.UnitY);
            }
        }

        private void UpdateProjectionMatrix()
        {
            if (IsUsingPerspective)
            {
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), AspectRatio, NearClippingDistance, FarClippingDistance);
            }
            else
            {
                ProjectionMatrix = Matrix4.CreateOrthographic(WindowManager.Width / 150, WindowManager.Height / 150, NearClippingDistance, FarClippingDistance);
            }

        }
    }
}
