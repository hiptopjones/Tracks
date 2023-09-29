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
            dummy.Transform.Position = new Vector2f(200, 200);

            DebugComponent debugComponent = dummy.AddComponent<DebugComponent>();

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

        public override void Draw(GraphicsManager graphicsManager)
        {
            GameObjectManager.Draw(graphicsManager);
        }
    }
}
