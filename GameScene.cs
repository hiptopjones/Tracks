using OpenTK.Mathematics;

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

            //CreateTestCubeRing();
            CreateLowPolyCars();

            //GameObject lightCube = CreateLightSource();
            //CreateIlluminatedCube(lightCube);

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

            CameraMoveComponent exploreComponent = gameObject.AddComponent<CameraMoveComponent>();

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

            ServiceLocator.Instance.ProvideService<DiagnosticsComponent>(diagnosticsComponent);

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

                TextureId textureId = TextureId.TestPalette;
                if (degrees == 0)
                {
                    textureId = TextureId.TestPattern;
                }

                CreateTestCube(position, Quaternion.Identity, Vector3.One * scale, textureId);
            }
        }

        private GameObject CreateLightSource()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Light Source");
            gameObject.Transform.Position = new Vector3(2, 0, -2);
            gameObject.Transform.Scale = new Vector3(0.5f);

            CubeComponent cubeComponent = gameObject.AddComponent<CubeComponent>();
            cubeComponent.Vertices = GameSettings.CubeVertices;
            cubeComponent.VertexShaderId = ShaderId.LightSourceVertex;
            cubeComponent.FragmentShaderId = ShaderId.LightSourceFragment;
            cubeComponent.Color = Color4.White;

            ServiceLocator.Instance.ProvideService("Light Source", cubeComponent);

            return gameObject;
        }

        private GameObject CreateIlluminatedCube(GameObject lightCube)
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Illuminated Target");
            gameObject.Transform.Position = new Vector3(1, 1, -1);

            CubeComponent cubeComponent = gameObject.AddComponent<CubeComponent>();
            cubeComponent.Vertices = GameSettings.CubeVertices;
            cubeComponent.VertexShaderId = ShaderId.IlluminatedTargetVertex;
            cubeComponent.FragmentShaderId = ShaderId.IlluminatedTargetFragment;
            cubeComponent.IsIlluminated = true;
            cubeComponent.Color = Color4.Red;

            OrbitMoveComponent orbitMoveComponent = gameObject.AddComponent<OrbitMoveComponent>();
            orbitMoveComponent.OrbitalRadius = 3;
            orbitMoveComponent.OrbitalTarget = lightCube;
            orbitMoveComponent.AngularSpeedDegrees = 45;

            return gameObject;
        }

        private void CreateLowPolyCars()
        {
            CreateLowPolyCar(ModelId.LowPolyCar, new Vector3(10, 0, 10), Quaternion.Identity, new Vector3(0.01f));
            CreateLowPolyCar(ModelId.CartoonCar, new Vector3(-10, 0, 10), Quaternion.Identity, new Vector3(0.01f));
            CreateLowPolyCar(ModelId.RaceCar, new Vector3(10, 0, -10), Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(90)), new Vector3(0.01f));
            CreateLowPolyCar(ModelId.Jeep, new Vector3(-10, 0, -10), Quaternion.Identity, new Vector3(0.01f));
        }


        private GameObject CreateLowPolyCar(ModelId modelId, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject gameObject = GameObjectManager.CreateGameObject(modelId.ToString());
            gameObject.Transform.Position = position;
            gameObject.Transform.Rotation = rotation;
            gameObject.Transform.Scale = scale;

            ModelComponent modelComponent = gameObject.AddComponent<ModelComponent>();
            modelComponent.ModelId = modelId;

            return gameObject;
        }

        private GameObject CreateTestCube(Vector3 position, Quaternion rotation, Vector3 scale, TextureId textureId)
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Test Cube");
            gameObject.Transform.Position = position;
            gameObject.Transform.Rotation = rotation;
            gameObject.Transform.Scale = scale;

            Test3dComponent drawable3dComponent = gameObject.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = GameSettings.CubeVertices;
            drawable3dComponent.TextureId = textureId;
            drawable3dComponent.VertexShaderId = ShaderId.DefaultVertex;
            drawable3dComponent.FragmentShaderId = ShaderId.DefaultFragment;

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
