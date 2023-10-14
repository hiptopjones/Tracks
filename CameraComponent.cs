using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tracks
{
    internal class CameraComponent : Component
    {
        // Orthographic only
        public bool IsOrthographic { get; set; }
        public Box2 OrthographicBounds { get; set; }

        // Perspective only
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }

        // Shared
        public float NearClipDistance { get; set; }
        public float FarClipDistance { get; set; }

        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ProjectionMatrix { get; private set; }


        private WindowManager WindowManager { get; set; }

        public override void Awake()
        {
            WindowManager = ServiceLocator.Instance.GetService<WindowManager>();
            WindowManager.Resized += OnWindowResized;

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
            if (IsOrthographic)
            {
                ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(
                    OrthographicBounds.Min.X, OrthographicBounds.Max.X,
                    OrthographicBounds.Min.Y, OrthographicBounds.Max.Y,
                    NearClipDistance, FarClipDistance);
            }
            else
            {
                // TODO: How to make orthographic view maintain the rough dimensions of things in the foreground?
                ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), AspectRatio, NearClipDistance, FarClipDistance);
            }
        }
    }
}
