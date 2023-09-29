using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class GameObjectManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        // Systems
        private CoreSystem CoreSystem { get; } = new CoreSystem();
        private DrawableSystem DrawableSystem { get; } = new DrawableSystem();
        //private CollisionSystem CollisionSystem { get; } = new CollisionSystem();

        // Double Buffer (to prevent race conditions)
        private bool IsUsingFirstCollection { get; set; }
        private List<GameObject> NewGameObjects1 { get; } = new List<GameObject>();
        private List<GameObject> NewGameObjects2 { get; } = new List<GameObject>();
        private List<GameObject> NewGameObjects
        {
            get
            {
                return IsUsingFirstCollection ? NewGameObjects1 : NewGameObjects2;
            }
        }

        public GameObject CreateGameObject()
        {
            GameObject gameObject = new GameObject();

            NewGameObjects.Add(gameObject);
            return gameObject;
        }

        public GameObject CreateGameObject(string name)
        {
            GameObject gameObject = new GameObject
            {
                Name = name
            };

            NewGameObjects.Add(gameObject);
            return gameObject;
        }

        public void Update(float deltaTime)
        {
            ProcessRemovals();
            ProcessAdditions();

            CoreSystem.Update(deltaTime);
            //CollisionSystem.Update();
        }

        public void LateUpdate(float deltaTime)
        {
            CoreSystem.LateUpdate(deltaTime);
        }

        public void Draw(GraphicsManager graphicsManager)
        {
            DrawableSystem.Draw(graphicsManager);
        }

        private void ProcessRemovals()
        {
            CoreSystem.ProcessRemovals();
            DrawableSystem.ProcessRemovals();
            //CollisionSystem.ProcessRemovals();
        }

        private void ProcessAdditions()
        {
            if (!NewGameObjects.Any())
            {
                return;
            }

            // Using a double-buffering pattern to avoid race problems
            // if any Awake() or Start() implementations add new objects
            // (Those new objects would be processed next frame.)
            List<GameObject> addedGameObjects = NewGameObjects;
            IsUsingFirstCollection = !IsUsingFirstCollection;

            foreach (GameObject gameObject in addedGameObjects)
            {
                gameObject.Awake();
            }

            foreach (GameObject gameObject in addedGameObjects)
            {
                gameObject.OnEnable();
            }

            foreach (GameObject gameObject in addedGameObjects)
            {
                gameObject.Start();
            }

            CoreSystem.ProcessAdditions(addedGameObjects);
            DrawableSystem.ProcessAdditions(addedGameObjects);
            //CollisionSystem.ProcessAdditions(addedGameObjects);

            addedGameObjects.Clear();
        }
    }
}
