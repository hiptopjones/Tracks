using NLog;
using SFML.Graphics;
using SFML.System;
using System.Numerics;

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

            CreateDummyObject();
        }

        private GameObject CreateDummyObject()
        {
            GameObject dummy = GameObjectManager.CreateGameObject("Dummy");
            dummy.Transform.Position = new Vector2f(600, 600);

            DebugComponent debugComponent = dummy.AddComponent<DebugComponent>();

            Test3dComponent drawable3dComponent = dummy.AddComponent<Test3dComponent>();
            drawable3dComponent.Vertices = new[] {
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.0f,  0.5f, 0.0f
            };

            drawable3dComponent.VertexShaderId = (int)GameSettings.ShaderId.DefaultVertex;
            drawable3dComponent.FragmentShaderId = (int)GameSettings.ShaderId.DefaultFragment;

            return dummy;
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
