using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;
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
            CreateMainCamera();
            CreateTestCubeRing();
        }

        private GameObject CreateMainCamera()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Main Camera");
            gameObject.Transform.Position = new Vector3(0, 0, 0);

            KeyboardMoveComponent moveComponent = gameObject.AddComponent<KeyboardMoveComponent>();
            ArcBallComponent arcBallComponent = gameObject.AddComponent<ArcBallComponent>();

            CameraComponent cameraComponent = gameObject.AddComponent<CameraComponent>();
            cameraComponent.AspectRatio = GameSettings.CameraAspectRatio;
            cameraComponent.FieldOfView = GameSettings.CameraFieldOfView;
            cameraComponent.NearClippingDistance = GameSettings.CameraNearClippingDistance;
            cameraComponent.FarClippingDistance = GameSettings.CameraFarClippingDistance;

            ServiceLocator.Instance.ProvideService("Main Camera", cameraComponent);

            return gameObject;
        }

        private GameObject CreateDiagnostics()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Diagnostics");
            gameObject.Transform.Position = new Vector3(-400, 600, 0);

            DiagnosticsComponent diagnosticsComponent = gameObject.AddComponent<DiagnosticsComponent>();

            return gameObject;
        }

        private void CreateTestCubeRing()
        {
            float distance = 3f;

            for (int degrees = 0; degrees < 360; degrees += 30)
            {
                float radians = MathHelper.DegreesToRadians(degrees);
                Vector3 position = new Vector3((float)Math.Cos(radians), 0, (float)Math.Sin(radians)) * distance;

                float scale = ((degrees % 60) + 30) / 90f;
                CreateTestCube(position, Quaternion.Identity, Vector3.One * scale);
            }
        }

        private GameObject CreateTestCube(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Test Cube");
            gameObject.Transform.Position = position;
            gameObject.Transform.Rotation = rotation;
            gameObject.Transform.Scale = scale;

            Test3dComponent drawable3dComponent = gameObject.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = GameSettings.CubeVertices;
            drawable3dComponent.TextureId = (int)TextureId.TestPalette;
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
