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

        private bool UseMouseLook { get; set; } = true;
        private Vector3 CumulativeAxisAngles { get; set; }

        private InputManager InputManager { get; set; }
        private CameraComponent MainCamera { get; set; }

        public override void Awake()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");
        }

        public override void Update(float deltaTime)
        {
            if (InputManager.IsKeyDown(Keys.T))
            {
                UseMouseLook = !UseMouseLook;
            }

            if (InputManager.IsKeyPressed(Keys.A))
            {
                Owner.Transform.Position += Vector3.UnitY * deltaTime;
            }

            if (InputManager.IsKeyPressed(Keys.Z))
            {
                Owner.Transform.Position -= Vector3.UnitY * deltaTime;
            }

            if (InputManager.IsKeyPressed(Keys.R))
            {
                Owner.Transform.Position = new Vector3(Owner.Transform.Position.X, 0, Owner.Transform.Position.Z);
            }

            float radiansAroundX = 0;
            float radiansAroundY = 0;

            if (UseMouseLook)
            {
                if (InputManager.IsMousePressed(MouseButton.Button1))
                {
                    //Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseSensitivity;
                    Vector2 mouseDelta = InputManager.MouseWheelDelta * MouseSensitivity;

                    if (mouseDelta.X != 0 || mouseDelta.Y != 0)
                    {
                        radiansAroundX = MathHelper.DegreesToRadians(mouseDelta.Y);
                        radiansAroundY = MathHelper.DegreesToRadians(mouseDelta.X);

                        CumulativeAxisAngles += new Vector3(radiansAroundX, radiansAroundY, 0);
                    }
                }
            }
            else
            {
                const float degreesPerSecond = 90;

                radiansAroundY = MathHelper.DegreesToRadians(degreesPerSecond * deltaTime);
                CumulativeAxisAngles += new Vector3(0, radiansAroundY, 0);
            }

            MainCamera.FieldOfView = Math.Clamp(MainCamera.FieldOfView + InputManager.MouseWheelDelta.Y, 1, 89);

            // BOTH OF THESE METHODS (relative and absolute) WORK FINE

            // Relative
            //Owner.Transform.Rotation = Quaternion.Multiply(Owner.Transform.Rotation, Quaternion.FromAxisAngle(Vector3.UnitX, radiansAroundX));
            //Owner.Transform.Rotation = Quaternion.Multiply(Quaternion.FromAxisAngle(Vector3.UnitY, radiansAroundY), Owner.Transform.Rotation);

            // Absolute
            Owner.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, CumulativeAxisAngles.Y);
            Owner.Transform.Rotation = Quaternion.Multiply(Owner.Transform.Rotation, Quaternion.FromAxisAngle(Vector3.UnitX, CumulativeAxisAngles.X));
        }
    }
}
