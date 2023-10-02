using static Tracks.GameSettings;

namespace Tracks
{
    internal class SplashScreenScene : Scene
    {
        public int TransitionSceneId { get; set; }
        public int TextureId { get; set; }

        private Sprite Sprite { get; set; }

        private float CurrentSeconds { get; set; }

        private SceneManager SceneManager { get; set; }

        public override void OnCreate()
        {
            SceneManager = ServiceLocator.Instance.GetService<SceneManager>();

            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();

            Texture texture = resourceManager.GetTexture(TextureId);
            Sprite = new Sprite(texture);
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
