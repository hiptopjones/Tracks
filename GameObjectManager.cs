namespace Tracks
{
    internal class GameObjectManager
    {
        public int GameObjectCount => CoreSystem.GameObjectCount;
        public int ComponentCount => CoreSystem.ComponentCount;

        // Systems
        private CoreSystem CoreSystem { get; } = new CoreSystem();
        private Drawable3dSystem Drawable3dSystem { get; } = new Drawable3dSystem();

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

        public GameObject FindGameObjectByName(string name)
        {
            return CoreSystem.FindGameObjectByName(name);
        }

        public GameObject[] FindGameObjectsByName(string name)
        {
            return CoreSystem.FindGameObjectsByName(name);
        }

        public GameObject FindGameObjectByComponent<T>() where T : Component
        {
            return CoreSystem.FindGameObjectByComponent<T>();
        }

        public GameObject[] FindGameObjectsByComponent<T>() where T : Component
        {
            return CoreSystem.FindGameObjectsByComponent<T>();
        }

        public void Update(float deltaTime)
        {
            ProcessRemovals();
            ProcessAdditions();

            CoreSystem.Update(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            CoreSystem.LateUpdate(deltaTime);
        }

        public void Draw()
        {
            Drawable3dSystem.Draw();
        }

        private void ProcessRemovals()
        {
            CoreSystem.ProcessRemovals();
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
            Drawable3dSystem.ProcessAdditions(addedGameObjects);

            addedGameObjects.Clear();
        }
    }
}
