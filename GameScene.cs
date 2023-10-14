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

            CreateMainCamera();
            CreateUiCamera();

            CreateTestCubeRing();

            // This needs to be at the end of the draw list to make blending work properly with depth testing
            CreateDebugGrid();

            CreateDiagnostics();
        }

        private GameObject CreateDebugGrid()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Debug Grid");

            DebugGridComponent debugGridComponent = gameObject.AddComponent<DebugGridComponent>();

            return gameObject;
        }

        private GameObject CreateMainCamera()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Main Camera");
            gameObject.Transform.Position = new Vector3(0, 0, 1);

            ArcBallComponent exploreComponent = gameObject.AddComponent<ArcBallComponent>();

            CameraComponent cameraComponent = gameObject.AddComponent<CameraComponent>();
            cameraComponent.AspectRatio = GameSettings.MainCameraAspectRatio;
            cameraComponent.FieldOfView = GameSettings.MainCameraFieldOfView;
            cameraComponent.NearClipDistance = GameSettings.MainCameraNearClipDistance;
            cameraComponent.FarClipDistance = GameSettings.MainCameraFarClipDistance;

            ServiceLocator.Instance.ProvideService("Main Camera", cameraComponent);

            return gameObject;
        }

        private GameObject CreateUiCamera()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("UI Camera");
            gameObject.Transform.Position = new Vector3(0, 0, 1);

            CameraComponent cameraComponent = gameObject.AddComponent<CameraComponent>();
            cameraComponent.OrthographicBounds = GameSettings.UiCameraBounds;
            cameraComponent.NearClipDistance = GameSettings.UiCameraNearClipDistance;
            cameraComponent.FarClipDistance = GameSettings.UiCameraFarClipDistance;
            cameraComponent.IsOrthographic = true;

            ServiceLocator.Instance.ProvideService("UI Camera", cameraComponent);

            return gameObject;
        }

        private GameObject CreateDiagnostics()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Diagnostics");
            gameObject.Transform.Position = GameSettings.DiagnosticsPosition;
            gameObject.Transform.Scale = Vector3.One * GameSettings.DiagnosticsScale;

            DiagnosticsComponent diagnosticsComponent = gameObject.AddComponent<DiagnosticsComponent>();
            diagnosticsComponent.RowOffset = GameSettings.DiagnosticsRowOffset;
            diagnosticsComponent.ColumnOffset = GameSettings.DiagnosticsColumnOffset;

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

                int textureId = (int)TextureId.TestPalette;
                if (degrees == 0)
                {
                    textureId = (int)TextureId.TestPattern;
                }

                CreateTestCube(position, Quaternion.Identity, Vector3.One * scale, textureId);
            }
        }

        private GameObject CreateTestCube(Vector3 position, Quaternion rotation, Vector3 scale, int textureId)
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Test Cube");
            gameObject.Transform.Position = position;
            gameObject.Transform.Rotation = rotation;
            gameObject.Transform.Scale = scale;

            Test3dComponent drawable3dComponent = gameObject.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = GameSettings.CubeVertices;
            drawable3dComponent.TextureId = textureId;
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
