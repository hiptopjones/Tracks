using NLog;
using NLog.Targets;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class ArcBallComponent : Component
    {
        public float MouseSensitivity { get; set; } = 0.1f;

        private InputManager InputManager { get; set; }
        private CameraComponent MainCamera { get; set; }

        public override void Awake()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");
        }

        public override void Update(float deltaTime)
        {
            if (InputManager.IsMousePressed(MouseButton.Button1))
            {
                Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseSensitivity;

                if (mouseDelta.X != 0 || mouseDelta.Y != 0)
                {
                    float radiansAroundX = MathHelper.DegreesToRadians(mouseDelta.Y);
                    float radiansAroundY = MathHelper.DegreesToRadians(mouseDelta.X);

                    MainCamera.FieldOfView = Math.Clamp(MainCamera.FieldOfView + InputManager.MouseWheelDelta.Y, 1, 89);

                    Owner.Transform.Rotation = Quaternion.Multiply(Owner.Transform.Rotation, Quaternion.FromAxisAngle(Vector3.UnitX, radiansAroundX));
                    Owner.Transform.Rotation = Quaternion.Multiply(Quaternion.FromAxisAngle(Vector3.UnitY, radiansAroundY), Owner.Transform.Rotation);

                }
            }
        }
    }
}
