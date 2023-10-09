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

            if (InputManager.IsKeyPressed(Keys.R))
            {
                Owner.Transform.Position = Vector3.Zero;
                Owner.Transform.Rotation = Quaternion.Identity;
                FieldOfView = GameSettings.CameraFieldOfView;
            }

            UpdateProjectionMatrix();
            UpdateViewMatrix();
        }

        private void UpdateViewMatrix()
        {
            Vector3 eye = Owner.Transform.Position;
            Vector3 target = Owner.Transform.Position + Owner.Transform.Forward; // Look in front of wherever the camera is!
            Vector3 up = Vector3.UnitY;

            ViewMatrix = Matrix4.LookAt(eye, target, up);
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
