using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class SceneManager
    {
        private int nextSceneId = 0;

        private Dictionary<int, Scene> Scenes { get; } = new Dictionary<int, Scene>();
        private Scene CurrentScene { get; set; }

        public void Update(float deltaTime)
        {
            CurrentScene?.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            CurrentScene?.LateUpdate(deltaTime);
        }

        public void Draw()
        {
            CurrentScene?.Draw();
        }

        public int AddScene(Scene scene)
        {
            int sceneId = nextSceneId;
            nextSceneId++;

            Scenes[sceneId] = scene;
            scene.OnCreate();

            return sceneId;
        }

        public void SwitchTo(int sceneId)
        {
            if (Scenes.TryGetValue(sceneId, out Scene scene))
            {
                if (CurrentScene == scene)
                {
                    throw new Exception("Cannot switch to a scene that is already the current scene");
                }

                if (CurrentScene != null)
                {
                    CurrentScene.OnDeactivate();
                }

                CurrentScene = scene;
                scene.OnActivate();
            }
        }

        public void RemoveScene(int sceneId)
        {
            if (Scenes.TryGetValue(sceneId, out Scene scene))
            {
                if (scene == CurrentScene)
                {
                    throw new Exception("Cannot remove the current scene");
                }

                Scenes.Remove(sceneId);
                scene.OnDestroy();
            }
        }
    }
}
