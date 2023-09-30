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

            CreateDebugObject();

            Vector3f[] leftTrianglePositions = new[]
            {
                new Vector3f(-0.5f, -0.25f, 0.0f),
                new Vector3f(0.0f, -0.25f, 0.0f),
                new Vector3f(-0.25f, 0.25f, 0.0f)
            };

            Vector3f[] rightTrianglePositions = new[]
            {
                new Vector3f(0.0f, -0.25f, 0.0f),
                new Vector3f(0.5f, -0.25f, 0.0f),
                new Vector3f(0.25f,  0.25f, 0.0f)
            };

            Vector3f[] triangleColors = new[]
            {
                new Vector3f(1.0f, 0.0f, 0.0f),
                new Vector3f(0.0f, 1.0f, 0.0f),
                new Vector3f(0.0f, 0.0f, 1.0f)
            };

            Vector2f[] textureCoordinates = new[]
            {
                // Y is flipped due to difference in SFML texture origin (?)
                new Vector2f(0.0f, 1.0f),
                new Vector2f(1.0f, 1.0f),
                new Vector2f(0.5f, 0.0f)
            };

            CreateTriangleObject("Left Triangle", leftTrianglePositions, triangleColors, textureCoordinates);
            CreateTriangleObject("Right Triangle", rightTrianglePositions, triangleColors, textureCoordinates);
        }

        private GameObject CreateDebugObject()
        {
            GameObject debug = GameObjectManager.CreateGameObject("Debug");
            debug.Transform.Position = new Vector2f(GameSettings.WindowWidth / 2, GameSettings.WindowHeight / 2);

            DebugComponent debugComponent = debug.AddComponent<DebugComponent>();
            debugComponent.Extents = new Vector2f(GameSettings.WindowWidth - 10, GameSettings.WindowHeight - 10);

            return debug;
        }

        private GameObject CreateTriangleObject(string name, Vector3f[] positions, Vector3f[] colors, Vector2f[] textureCoordinates)
        {
            float minX = (positions.Min(v => v.X) + 1) / 2f;
            float minY = (positions.Min(v => v.Y) + 1) / 2f;
            float maxX = (positions.Max(v => v.X) + 1) / 2f;
            float maxY = (positions.Max(v => v.Y) + 1) / 2f;

            FloatRect bounds = new FloatRect(
                minX * GameSettings.WindowWidth,
                minY * GameSettings.WindowHeight,
                (maxX - minX) * GameSettings.WindowWidth,
                (maxY - minY) * GameSettings.WindowHeight
                );

            GameObject triangle = GameObjectManager.CreateGameObject(name);
            triangle.Transform.Position = new Vector2f(
                bounds.Left + bounds.Width / 2,
                bounds.Top + bounds.Height / 2
                );

            DebugComponent debugComponent = triangle.AddComponent<DebugComponent>();
            debugComponent.Extents = new Vector2f(bounds.Width, bounds.Height);

            Test3dComponent drawable3dComponent = triangle.AddComponent<Test3dComponent>();
            drawable3dComponent.Positions = positions;
            drawable3dComponent.Colors = colors;
            drawable3dComponent.TextureCoordinates = textureCoordinates;
            drawable3dComponent.TextureId = (int)GameSettings.TextureId.TestPattern;

            drawable3dComponent.VertexShaderId = (int)GameSettings.ShaderId.DefaultVertex;
            drawable3dComponent.FragmentShaderId = (int)GameSettings.ShaderId.DefaultFragment;

            return triangle;
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
