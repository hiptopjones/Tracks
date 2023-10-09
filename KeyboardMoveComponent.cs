using NLog.Targets;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class KeyboardMoveComponent : Component
    {
        public float Speed { get; set; } = 5;

        private InputManager InputManager { get; set; }

        public override void Awake()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
        }

        public override void Update(float deltaTime)
        {
            Vector3 direction = Vector3.Zero;

            if (InputManager.IsKeyPressed(Keys.W))
            {
                direction += Owner.Transform.Forward;
            }

            if (InputManager.IsKeyPressed(Keys.A))
            {
                direction += Owner.Transform.Left;
            }

            if (InputManager.IsKeyPressed(Keys.S))
            {
                direction += Owner.Transform.Backward;
            }

            if (InputManager.IsKeyPressed(Keys.D))
            {
                direction += Owner.Transform.Right;
            }

            if (InputManager.IsKeyPressed(Keys.Q))
            {
                direction += Owner.Transform.Up;
            }

            if (InputManager.IsKeyPressed(Keys.E))
            {
                direction += Owner.Transform.Down;
            }


            if (direction != Vector3.Zero)
            {
                direction.Normalize();
                Owner.Transform.Position += direction * Speed * deltaTime;
            }

            if (InputManager.IsKeyPressed(Keys.R))
            {
                Owner.Transform.Position = Vector3.Zero;
            }

            Debug.DrawText($"Player: {Owner.Transform.Position}", new Vector2(-750, -550));

        }
    }
}
