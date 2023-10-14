using OpenTK.Mathematics;

namespace Tracks
{
    internal class SplashScreenScene : Scene
    {
        public int TransitionSceneId { get; set; }
        public TextureId TextureId { get; set; }

        private Sprite Sprite { get; set; }

        private float CurrentSeconds { get; set; }

        private SceneManager SceneManager { get; set; }

        public override void OnCreate()
        {
            SceneManager = ServiceLocator.Instance.GetService<SceneManager>();

            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();

            Texture texture = resourceManager.GetTexture(TextureId);
            Sprite = new Sprite(texture, Vector2.Zero, Vector2.One, new Vector2(texture.Width / 2, texture.Height / 2));
        }

        public override void OnDestroy()
        {
            // Nothing
        }

        public override void OnActivate()
        {
            CurrentSeconds = 0;
        }

        public override void OnDeactivate()
        {
            // Nothing
        }

        public override void Update(float deltaTime)
        {
            CurrentSeconds += deltaTime;
            if (CurrentSeconds >= GameSettings.SplashScreenTransitionDelaySeconds)
            {
                SceneManager.SwitchTo(TransitionSceneId);
            }
        }

        public override void LateUpdate(float deltaTime)
        {
            // Nothing
        }

        public override void Draw()
        {
            Sprite.Draw();
        }
    }
}
