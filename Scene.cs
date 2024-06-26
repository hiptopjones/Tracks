﻿namespace Tracks
{
    internal abstract class Scene
    {
        public abstract void OnCreate();

        public abstract void OnDestroy();

        public abstract void OnActivate();

        public abstract void OnDeactivate();

        public abstract void Update(float deltaTime);

        public abstract void LateUpdate(float deltaTime);

        public abstract void Draw();
    }
}
