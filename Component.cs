using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal abstract class Component
    {
        public GameObject Owner { get; set; }

        public virtual void Awake() { }
        public virtual void OnEnable() { }
        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }
        public virtual void LateUpdate(float deltaTime) { }
        public virtual void OnDisable() { }

        public virtual void Reset() { }
    }
}
