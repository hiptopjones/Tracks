using NLog;

namespace Tracks
{
    internal sealed class GameObject
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static int nextGameObjectId = 0;

        public int Id { get; private set; }
        public string Name { get; set; }
        public TransformComponent Transform { get; private set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    if (_isEnabled)
                    {
                        OnEnable();
                    }
                    else
                    {
                        OnDisable();
                    }
                }
            }
        }

        // Can only be set through Destroy()
        public bool IsAlive { get; private set; } = true;

        public bool IsLocked { get; set; }

        public Action<GameObject> OnDestroyed;

        public int ComponentCount => Components.Count;

        private List<Component> Components { get; } = new List<Component>();

        public GameObject()
        {
            Id = nextGameObjectId++;
            Name = $"Object{Id}";

            Transform = AddComponent<TransformComponent>();

            // Default is to actually destroy the object
            // Can be overwritten to allow pooling behavior
            OnDestroyed = (g) =>
            {
                g.IsEnabled = false;
                g.IsAlive = false;
            };
        }

        /// <summary>
        /// Called exactly once when the object is initialized
        /// </summary>
        public void Awake()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Awake();
            }
        }

        /// <summary>
        /// Called each time the object is enabled
        /// </summary>
        public void OnEnable()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].OnEnable();
            }
        }

        /// <summary>
        /// Called exactly once when the object is enabled
        /// </summary>
        public void Start()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Start();
            }
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Update(deltaTime);
            }
        }

        /// <summary>
        /// Called every frame (after all Update() calls complete)
        /// </summary>
        /// <param name="deltaTime"></param>
        public void LateUpdate(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].LateUpdate(deltaTime);
            }
        }

        /// <summary>
        /// Called each time the object is disabled
        /// </summary>
        public void OnDisable()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].OnDisable();
            }
        }

        /// <summary>
        /// Intended to re-initialize any component state back to post Start() values
        /// </summary>
        public void Reset()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Reset();
            }
        }

        /// <summary>
        /// Takes this object out of service
        /// </summary>
        public void Destroy()
        {
            // Use redirection to enable object pooling
            OnDestroyed(this);
        }

        public T AddComponent<T>() where T : Component, new()
        {
            if (IsLocked)
            {
                throw new Exception("Object has been locked");
            }

            T component = Components.OfType<T>().SingleOrDefault();
            if (component == null)
            {
                component = new T();
                component.Owner = this;

                Components.Add(component);
            }
            else
            {
                Logger.Error($"Component of type {typeof(T).Name} already exists on object '{Name}'");
            }

            return component;
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return Components.OfType<T>();
        }

        public T GetComponent<T>() where T : Component
        {
            return GetComponents<T>().FirstOrDefault();
        }

        public bool HasComponent<T>() where T : Component
        {
            return Components.OfType<T>().Any();
        }

        public override string ToString()
        {
            string components = string.Join("\n    ", Components.Select(x => $"{x.GetType().Name}({x})"));
            return $"[GameObject] Id({Id}) Name({Name}) IsEnabled({IsEnabled}) IsAlive({IsAlive}) IsLocked({IsLocked}) Components(\n    {components})";
        }
    }
}
