namespace Tracks
{
    internal class GameObjectPool
    {
        private enum PoolState
        {
            Active,
            Pooled,
        }

        private Func<GameObject> GameObjectFactory { get; set; }

        private Queue<GameObject> PooledObjects = new Queue<GameObject>();
        private Dictionary<GameObject, PoolState> PooledObjectStates = new Dictionary<GameObject, PoolState>();

        public GameObjectPool(Func<GameObject> gameObjectFactory)
        {
            GameObjectFactory = gameObjectFactory;
        }

        public GameObject GetOrCreateObject()
        {
            GameObject obj = null;

            if (PooledObjects.Any())
            {
                // Objects from the pool are already registered with the core system,
                // so may have their Update() functions called this frame

                obj = PooledObjects.Dequeue();
                obj.IsEnabled = true;
                obj.Reset();
            }
            else
            {
                // Objects from the factory will not be registered with the core system
                // until the end of the current frame, so the earliest their Update()
                // functions will be called is next frame

                obj = GameObjectFactory();

                // Intended to prevent components from being added after the factory,
                // which is to ensure all pool objects are identical and avoid stupid mistakes
                obj.IsLocked = true;
            }

            PooledObjectStates[obj] = PoolState.Active;
            return obj;
        }

        public void OnDestroyed(GameObject obj)
        {
            if (PooledObjectStates[obj] == PoolState.Pooled)
            {
                return;
            }

            obj.IsEnabled = false;
            PooledObjects.Enqueue(obj);
            PooledObjectStates[obj] = PoolState.Pooled;
        }
    }
}
