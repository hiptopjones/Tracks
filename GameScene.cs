using OpenTK.Mathematics;
using static Tracks.GameSettings;

namespace Tracks
{
    internal class GameScene : Scene
    {
        protected GameObjectManager GameObjectManager { get; set; }

        public override void OnCreate()
        {
            GameObjectManager = new GameObjectManager();
            ServiceLocator.Instance.ProvideService(GameObjectManager);

            CreateDiagnostics();

            GameObject camera = CreateMainCamera();
            GameObject mobileCube = CreateMobileTestCube();

            CreateStationaryTestCube(new Vector3(0, 0, 0), Vector3.Zero, 1, Color4.White);
            CreateStationaryTestCube(new Vector3(0, 1, 0), Vector3.Zero, 1, Color4.Green);
            CreateStationaryTestCube(new Vector3(1, 0, 0), Vector3.Zero, 1, Color4.Red);
            CreateStationaryTestCube(new Vector3(0, 0, 1), Vector3.Zero, 1, Color4.Blue);

            CameraComponent cameraComponent = camera.GetComponent<CameraComponent>();
            cameraComponent.Target = mobileCube;
        }

        private GameObject CreateMainCamera()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Main Camera");
            gameObject.Transform.Position = new Vector3(0, 0, -10);

            KeyboardMoveComponent moveComponent = gameObject.AddComponent<KeyboardMoveComponent>();
            moveComponent.Speed = 10f;

            CameraComponent cameraComponent = gameObject.AddComponent<CameraComponent>();
            cameraComponent.AspectRatio = GameSettings.WindowWidth / (float)GameSettings.WindowHeight;
            cameraComponent.FieldOfView = 45f;
            cameraComponent.NearClippingDistance = 0.1f;
            cameraComponent.FarClippingDistance = 100f;

            ServiceLocator.Instance.ProvideService<CameraComponent>(cameraComponent);

            return gameObject;
        }

        private GameObject CreateDiagnostics()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Diagnostics");
            gameObject.Transform.Position = new Vector3(-400, 600, 0);

            DiagnosticsComponent diagnosticsComponent = gameObject.AddComponent<DiagnosticsComponent>();

            return gameObject;
        }
        private GameObject CreateStationaryTestCube(Vector3 position, Vector3 rotation, float scale, Color4 color)
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Stationary Test Cube");
            gameObject.Transform.Position = position;
            gameObject.Transform.Rotation = rotation;
            gameObject.Transform.Scale = Vector3.One * scale;

            Test3dComponent drawable3dComponent = gameObject.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = GameSettings.CubeVertices;

            drawable3dComponent.TextureId = (int)TextureId.Blank;
            drawable3dComponent.Color = color;

            drawable3dComponent.VertexShaderId = (int)ShaderId.DefaultVertex;
            drawable3dComponent.FragmentShaderId = (int)ShaderId.DefaultFragment;

            return gameObject;
        }
    
        private GameObject CreateMobileTestCube()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Mobile Test Cube");

            Test3dComponent drawable3dComponent = gameObject.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = GameSettings.CubeVertices;

            drawable3dComponent.TextureId = (int)TextureId.TestPattern;

            drawable3dComponent.VertexShaderId = (int)ShaderId.DefaultVertex;
            drawable3dComponent.FragmentShaderId = (int)ShaderId.DefaultFragment;

            return gameObject;
        }

        public override void OnDestroy()
        {
            // Nothing
        }

        public override void OnActivate()
        {
            // Nothing
        }

        public override void OnDeactivate()
        {
            // Nothing
        }

        public override void Update(float deltaTime)
        {
            GameObjectManager.Update(deltaTime);
        }

        public override void LateUpdate(float deltaTime)
        {
            GameObjectManager.LateUpdate(deltaTime);
        }

        public override void Draw()
        {
            GameObjectManager.Draw();
        }
    }
}
