using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Called exactly once when the object is initialized
        public void Awake()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Awake();
            }
        }

        // Called each time the object is enabled
        public void OnEnable()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].OnEnable();
            }
        }

        // Called exactly once when the object is enabled
        public void Start()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Start();
            }
        }

        // Called every frame
        public void Update(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Update(deltaTime);
            }
        }

        // Called every frame (after all Update() calls complete)
        public void LateUpdate(float deltaTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].LateUpdate(deltaTime);
            }
        }

        // Called each time the object is disabled
        public void OnDisable()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].OnDisable();
            }
        }

        // Intended to re-initialize any component state back to post Start() values
        public void Reset()
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                Components[i].Reset();
            }
        }

        // Takes this object out of service
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
                Logger.Info($"Component of type {typeof(T).Name} already exists on object '{Name}'");
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
