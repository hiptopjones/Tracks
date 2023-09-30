using NLog;
using SFML.Graphics;
using SFML.System;

namespace Tracks
{
    internal class GameScene : Scene
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        protected GameObjectManager GameObjectManager { get; set; }

        public override void OnCreate()
        {
            GameObjectManager = new GameObjectManager();
            ServiceLocator.Instance.ProvideService(GameObjectManager);

            Create2dObject();
            CreateTestQuad();
        }

        private GameObject Create2dObject()
        {
            GameObject debug = GameObjectManager.CreateGameObject("Debug");
            debug.Transform.Position = new Vector2f(200, 200);

            DebugComponent debugComponent = debug.AddComponent<DebugComponent>();
            debugComponent.Extents = new Vector2f(100, 100);

            return debug;
        }

        private GameObject CreateTestQuad()
        {
            GameObject gameObject = GameObjectManager.CreateGameObject("Test Quad");

            Test3dComponent drawable3dComponent = gameObject.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = new[]
            {
                // Y is flipped due to difference in SFML texture origin (?)

                // Position         Texture coordinates
                0.5f,  0.5f, 0.0f, 1.0f, 0.0f, // top right
                 0.5f, -0.5f, 0.0f, 1.0f, 1.0f, // bottom right
                -0.5f, -0.5f, 0.0f, 0.0f, 1.0f, // bottom left
                -0.5f,  0.5f, 0.0f, 0.0f, 0.0f  // top left
            };

            drawable3dComponent.Indices = new uint[]
            {
                0, 1, 3,
                1, 2, 3
            };

            drawable3dComponent.TextureId = (int)GameSettings.TextureId.TestPattern;

            drawable3dComponent.VertexShaderId = (int)GameSettings.ShaderId.DefaultVertex;
            drawable3dComponent.FragmentShaderId = (int)GameSettings.ShaderId.DefaultFragment;

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
