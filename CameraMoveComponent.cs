using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tracks
{
    internal class CameraMoveComponent : Component
    {
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
                gameObject = GetSelectableObject(0);
            }
            else if (InputManager.IsKeyDown(Keys.D2))
            {
                gameObject = GetSelectableObject(1);
            }
            else if (InputManager.IsKeyDown(Keys.D3))
            {
                gameObject = GetSelectableObject(2);
            }
            else if (InputManager.IsKeyDown(Keys.D4))
            {
                gameObject = GetSelectableObject(3);
            }
            else if (InputManager.IsKeyDown(Keys.D5))
            {
                gameObject = GetSelectableObject(4);
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
            GameObject[] gameObjects = GameObjectManager.FindGameObjectsByComponent<Drawable3dComponent>();
            if (i < gameObjects.Length)
            {
                return gameObjects[i];
            }

            return null;
        }

        // Tried to mimic Unity's editor movement
        private void HandleMouseMovement(float deltaTime)
        {
            if (InputManager.IsMousePressed(MouseButton.Button1))
            {
                if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt))
                {
                    Debug.DrawText("ORBIT", new Vector2(50, 50), Color4.Red, 0.5f);

                    // ACTION: Orbit around the target
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseRotateSensitivity;

                    // Use the shift key to do finer movements
                    if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                    {
                        Debug.DrawText("SLOW", new Vector2(100, 50), Color4.Red, 0.5f);
                        mouseDelta /= 10;
                    }

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
                    Debug.DrawText("SLIDE", new Vector2(50, 50), Color4.Red, 0.5f);

                    // ACTION: Slide left/right and up/down
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseMoveSensitivity;

                    // Use the shift key to do finer movements
                    if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                    {
                        Debug.DrawText("SLOW", new Vector2(100, 50), Color4.Red, 0.5f);
                        mouseDelta /= 10;
                    }

                    // Increase slide speed as the distance from the focused point increases
                    float slideSpeed = (float)Math.Max(1, OrbitDistance / 3);

                    Vector3 offsetRight = Owner.Transform.Right * -mouseDelta.X * slideSpeed;
                    Vector3 offsetUp = Owner.Transform.Up * mouseDelta.Y * slideSpeed;

                    Owner.Transform.Position += offsetRight + offsetUp;
                }
            }
            else if (InputManager.IsMousePressed(MouseButton.Button2))
            {
                if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt))
                {
                    Debug.DrawText("ZOOM", new Vector2(50, 50), Color4.Red, 0.5f);

                    // ACTION: Slide forward/back (like zooming in or out)
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseMoveSensitivity;

                    // Use the shift key for finer movements
                    if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                    {
                        Debug.DrawText("SLOW", new Vector2(100, 50), Color4.Red, 0.5f);
                        mouseDelta /= 10;
                    }

                    // Increase mouse wheel zoom speed as the distance from the focused point increases
                    float zoomSpeed = OrbitDistance;

                    // Down zooms in, Up zooms out
                    float signedOffset = mouseDelta.Length * Math.Sign(mouseDelta.Y) * zoomSpeed;

                    Owner.Transform.Position += Owner.Transform.Forward * signedOffset;

                    OrbitDistance = Math.Max(OrbitDistance - signedOffset, MinOrbitDistance);
                }
                else
                {
                    Debug.DrawText("SCAN", new Vector2(50, 50), Color4.Red, 0.5f);

                    // ACTION: Rotate around the camera origin (like scanning the area)
                    Vector2 mouseDelta = InputManager.MouseMoveDelta * MouseRotateSensitivity;
                    
                    // Use the shift key for finer movements
                    if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                    {
                        Debug.DrawText("SLOW", new Vector2(100, 50), Color4.Red, 0.5f);
                        mouseDelta /= 10;
                    }

                    if (mouseDelta.X != 0 || mouseDelta.Y != 0)
                    {
                        float radiansAroundX = -MathHelper.DegreesToRadians(mouseDelta.Y);
                        float radiansAroundY = -MathHelper.DegreesToRadians(mouseDelta.X);

                        Owner.Transform.Rotation = Quaternion.Multiply(Owner.Transform.Rotation, Quaternion.FromAxisAngle(Vector3.UnitX, radiansAroundX));
                        Owner.Transform.Rotation = Quaternion.Multiply(Quaternion.FromAxisAngle(Vector3.UnitY, radiansAroundY), Owner.Transform.Rotation);
                    }
                }
            }
            else
            {
                // ACTION: Slide forward/back (like zooming in or out)
                Vector2 mouseWheelDelta = InputManager.MouseWheelDelta * MouseWheelSensitivity;
                if (mouseWheelDelta.X != 0 || mouseWheelDelta.Y != 0)
                {
                    Debug.DrawText("WHEEL", new Vector2(100, 50), Color4.Red, 0.5f);

                    // Use the shift key for finer movements
                    if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                    {
                        Debug.DrawText("SLOW", new Vector2(100, 50), Color4.Red, 0.5f);
                        mouseWheelDelta /= 10;
                    }

                    // Increase mouse wheel zoom speed as the distance from the focused point increases
                    float zoomSpeed = OrbitDistance;

                    // Down zooms in, Up zooms out
                    float signedOffset = mouseWheelDelta.Length * Math.Sign(mouseWheelDelta.Y) * zoomSpeed;

                    Owner.Transform.Position += Owner.Transform.Forward * signedOffset;

                    OrbitDistance = Math.Max(OrbitDistance - signedOffset, MinOrbitDistance);
                }
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
                Debug.DrawText("WASD", new Vector2(50, 50), Color4.Red, 0.5f);

                // Prevents moving faster on diagonals
                direction.Normalize();

                // Use the shift key for finer movements
                if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
                {
                    Debug.DrawText("SLOW", new Vector2(100, 50), Color4.Red, 0.5f);
                    direction /= 10;
                }

                // Increase move speed as the distance from the focused point increases
                float moveSpeed = OrbitDistance;

                Vector3 translation = direction * moveSpeed * deltaTime;
                Owner.Transform.Position += translation;

                float projectedLength = Vector3.Dot(translation, Owner.Transform.Forward);
                OrbitDistance = Math.Max(OrbitDistance - projectedLength, MinOrbitDistance);
            }
        }
    }
}
