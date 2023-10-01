namespace Tracks
{
    internal abstract class Component
    {
        /// <summary>
        /// The game object to which this component is attached
        /// </summary>
        public GameObject Owner { get; set; }

        /// <summary>
        /// Called exactly once when the object is initialized
        /// </summary>
        public virtual void Awake() { }

        /// <summary>
        /// Called each time the object is enabled
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// Called exactly once when the object is enabled
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Update(float deltaTime) { }

        /// <summary>
        /// Called every frame (after all Update() calls complete)
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void LateUpdate(float deltaTime) { }

        /// <summary>
        /// Called each time the object is disabled
        /// </summary>
        public virtual void OnDisable() { }

        /// <summary>
        /// Intended to re-initialize any component state back to post Start() values
        /// </summary>
        public virtual void Reset() { }
    }
}
