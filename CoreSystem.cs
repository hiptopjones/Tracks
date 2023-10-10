using System.Xml.Linq;

namespace Tracks
{
    internal class CoreSystem
    {
        private List<GameObject> GameObjects { get; } = new List<GameObject>();

        public int GameObjectCount => GameObjects.Count;
        public int ComponentCount => GameObjects.Sum(x => x.ComponentCount);

        public void ProcessAdditions(IEnumerable<GameObject> newGameObjects)
        {
            GameObjects.AddRange(newGameObjects);
        }

        public void ProcessRemovals()
        {
            // Remove any objects from consideration that are dead
            Utilities.DeleteWithSwapAndPop(GameObjects, x => !x.IsAlive);
        }

        public void Update(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject.IsEnabled)
                {
                    gameObject.Update(deltaTime);
                }
            }
        }

        public void LateUpdate(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject.IsEnabled)
                {
                    gameObject.LateUpdate(deltaTime);
                }
            }
        }

        internal GameObject FindGameObjectByName(string name)
        {
            return GameObjects.FirstOrDefault(x => x.Name == name);
        }

        internal GameObject[] FindGameObjectsByName(string name)
        {
            return GameObjects.Where(x => x.Name == name).ToArray();
        }

        internal GameObject FindGameObjectByComponent<T>() where T : Component
        {
            return GameObjects.FirstOrDefault(x => x.HasComponent<T>());
        }

        internal GameObject[] FindGameObjectsByComponent<T>() where T : Component
        {
            return GameObjects.Where(x => x.HasComponent<T>()).ToArray();
        }
    }
}
