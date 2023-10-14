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
        public float TranslationSpeed { get; set; } = 5;

        public float MouseRotateSensitivity { get; set; } = 0.1f;
        public float MouseMoveSensitivity { get; set; } = 0.01f;
        public float MouseWheelSensitivity { get; set; } = 0.1f;

        public float MinOrbitDistance { get; set; } = 1;
        public float SelectionOrbitDistance { get; set; } = 3;

        private Vector3 StartingPosition { get; set; }
        private Quaternion StartingRotation { get; set; }
        private float OrbitDistance { get; set; }

        private InputManager InputManager { get; set; }
        private CameraComponent MainCamera { get; set; }
        private GameObjectManager GameObjectManager { get; set; }

        public override void Awake()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
            MainCamera = ServiceLocator.Instance.GetService<CameraComponent>("Main Camera");
            GameObjectManager = ServiceLocator.Instance.GetService<GameObjectManager>();

            StartingPosition = Owner.Transform.Position;
            StartingRotation = Owner.Transform.Rotation;

            OrbitDistance = MinOrbitDistance;
        }

        public override void Update(float deltaTime)
        {
            if (InputManager.IsKeyPressed(Keys.R))
            {
                Owner.Transform.Position = StartingPosition;
                Owner.Transform.Rotation = StartingRotation;
                MainCamera.FieldOfView = GameSettings.MainCameraFieldOfView;
                OrbitDistance = MinOrbitDistance;
            }

            // Allow selecting some objects to simulate Unity's F key
            HandleObjectSelection();

            HandleMouseMovement(deltaTime);
            HandleKeyboardMovement(deltaTime);
        }

        private void HandleObjectSelection()
        {
            GameObject gameObject = null;

            if (InputManager.IsKeyDown(Keys.D1))
            {
                gameObject = GetSelectableObject(1);
            }
            else if (InputManager.IsKeyDown(Keys.D2))
            {
                gameObject = GetSelectableObject(2);
            }
            else if (InputManager.IsKeyDown(Keys.D3))
            {
                gameObject = GetSelectableObject(3);
            }
            else if (InputManager.IsKeyDown(Keys.D4))
            {
                gameObject = GetSelectableObject(4);
            }
            else if (InputManager.IsKeyDown(Keys.D5))
            {
                gameObject = GetSelectableObject(5);
            }

            if (gameObject != null)
            {
                OrbitDistance = SelectionOrbitDistance;

                // Move the camera to the selected object, then back it off by orbit distance
                Owner.Transform.Position = gameObject.Transform.Position;
                Owner.Transform.Position -= Owner.Transform.Forward * OrbitDistance;
            }
        }

        private GameObject GetSelectableObject(int i)
        {
            GameObject[] gameObjects = GameObjectManager.FindGameObjectsByComponent<Test3dComponent>();
            return gameObjects[i];
        }

        // Tried to mimic Unity's editor movement
        private void HandleMouseMovement(float deltaTime)
        {
            if (InputManager.IsMousePressed(MouseButton.Button1))
            {
                if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt))
                {
                    // ACTION: Orbit around the target
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseRotateSensitivity;

                    if (mouseDelta.X != 0 || mouseDelta.Y != 0)
                    {
                        float radiansAroundX = -MathHelper.DegreesToRadians(mouseDelta.Y);
                        float radiansAroundY = -MathHelper.DegreesToRadians(mouseDelta.X);

                        Owner.Transform.Position += Owner.Transform.Forward * OrbitDistance;

                        Owner.Transform.Rotation = Quaternion.Multiply(Owner.Transform.Rotation, Quaternion.FromAxisAngle(Vector3.UnitX, radiansAroundX));
                        Owner.Transform.Rotation = Quaternion.Multiply(Quaternion.FromAxisAngle(Vector3.UnitY, radiansAroundY), Owner.Transform.Rotation);

                        Owner.Transform.Position -= Owner.Transform.Forward * OrbitDistance;
                    }
                }
                else
                {
                    // ACTION: Slide left/right and up/down
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseMoveSensitivity;

                    Vector3 offsetRight = Owner.Transform.Right * -mouseDelta.X;
                    Vector3 offsetUp = Owner.Transform.Up * mouseDelta.Y;

                    Owner.Transform.Position += offsetRight + offsetUp;
                }
            }
            else if (InputManager.IsMousePressed(MouseButton.Button2))
            {
                if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                {
                    // ACTION: Rotate around the camera origin (like scanning the area)
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseRotateSensitivity;

                    if (mouseDelta.X != 0 || mouseDelta.Y != 0)
                    {
                        float radiansAroundX = -MathHelper.DegreesToRadians(mouseDelta.Y);
                        float radiansAroundY = -MathHelper.DegreesToRadians(mouseDelta.X);

                        Owner.Transform.Rotation = Quaternion.Multiply(Owner.Transform.Rotation, Quaternion.FromAxisAngle(Vector3.UnitX, radiansAroundX));
                        Owner.Transform.Rotation = Quaternion.Multiply(Quaternion.FromAxisAngle(Vector3.UnitY, radiansAroundY), Owner.Transform.Rotation);
                    }
                }
                else if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt))
                {
                    // ACTION: Slide forward/back (like zooming in or out)
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseMoveSensitivity;

                    // Down zooms in, Up zooms out
                    float signedOffset = mouseDelta.Length * Math.Sign(mouseDelta.Y);

                    Owner.Transform.Position += Owner.Transform.Forward * signedOffset;

                    OrbitDistance = Math.Max(OrbitDistance - signedOffset, MinOrbitDistance);
                }
            }
            else
            {
                // ACTION: Slide forward/back (like zooming in or out)
                Vector2 mouseDelta = InputManager.MouseWheelDelta * MouseWheelSensitivity;

                // Down zooms in, Up zooms out
                float signedOffset = mouseDelta.Length * Math.Sign(mouseDelta.Y);

                Owner.Transform.Position += Owner.Transform.Forward * signedOffset;

                OrbitDistance = Math.Max(OrbitDistance - signedOffset, MinOrbitDistance);
            }
        }

        private void HandleKeyboardMovement(float deltaTime)
        {
            Vector3 direction = Vector3.Zero;

            if (InputManager.IsKeyPressed(Keys.W))
            {
                direction += Owner.Transform.Forward;
            }

            if (InputManager.IsKeyPressed(Keys.A))
            {
                direction += Owner.Transform.Left;
            }

            if (InputManager.IsKeyPressed(Keys.S))
            {
                direction += Owner.Transform.Backward;
            }

            if (InputManager.IsKeyPressed(Keys.D))
            {
                direction += Owner.Transform.Right;
            }

            if (InputManager.IsKeyPressed(Keys.Q))
            {
                direction += Owner.Transform.Up;
            }

            if (InputManager.IsKeyPressed(Keys.E))
            {
                direction += Owner.Transform.Down;
            }

            if (direction != Vector3.Zero)
            {
                direction.Normalize();

                float scaledTranslationSpeed = TranslationSpeed;
                if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                {
                    scaledTranslationSpeed /= 10;
                }

                Owner.Transform.Position += direction * scaledTranslationSpeed * deltaTime;
                
                // TODO: Should W and A affect orbit distance?
            }
        }
    }
}
