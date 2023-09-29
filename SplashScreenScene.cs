using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class SplashScreenScene : Scene
    {
        public int TransitionSceneId { get; set; }

        private Sprite Sprite { get; set; }

        private float CurrentSeconds { get; set; }

        private SceneManager SceneManager { get; set; }

        public override void OnCreate()
        {
            SceneManager = ServiceLocator.Instance.GetService<SceneManager>();
            
            ResourceManager resourceManager = ServiceLocator.Instance.GetService<ResourceManager>();

            Texture texture = resourceManager.GetTexture((int)GameSettings.TextureId.SplashScreen);
            Sprite = new Sprite(texture);

            FloatRect spriteSize = Sprite.GetLocalBounds();
            Sprite.Origin = new Vector2f(spriteSize.Width, spriteSize.Height) * 0.5f;

            float scaleX = GameSettings.WindowWidth / spriteSize.Width;
            float scaleY = GameSettings.WindowHeight / spriteSize.Height;
            float scaleXY = Math.Max(scaleX, scaleY);
            Sprite.Scale = new Vector2f(scaleXY, scaleXY);

            GraphicsManager graphicsManager = ServiceLocator.Instance.GetService<GraphicsManager>();
            Sprite.Position = new Vector2f(graphicsManager.Width, graphicsManager.Height) * 0.5f;
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

        public override void Draw(GraphicsManager graphicsManager)
        {
            graphicsManager.Draw(Sprite);
        }
    }
}
