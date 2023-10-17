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
    internal class OrbitMoveComponent : Component
    {
        public float AngularSpeedDegrees { get; set; }

        public float OrbitalRadius { get; set; }

        public GameObject OrbitalTarget { get; set; }

        private float CurrentAngleDegrees { get; set; }
        private bool IsOrbiting { get; set; } = true;

        private InputManager InputManager { get; set; }

        public override void Awake()
        {
            InputManager = ServiceLocator.Instance.GetService<InputManager>();
        }

        public override void Update(float deltaTime)
        {
            if (InputManager.IsKeyDown(Keys.T))
            {
                IsOrbiting = !IsOrbiting;
            }

            if (IsOrbiting)
            {
                CurrentAngleDegrees += AngularSpeedDegrees * deltaTime;

                float x = OrbitalRadius * MathF.Cos(MathHelper.DegreesToRadians(CurrentAngleDegrees));
                float y = OrbitalRadius * MathF.Sin(MathHelper.DegreesToRadians(CurrentAngleDegrees));

                Owner.Transform.Position = OrbitalTarget.Transform.Position + new Vector3(x, 0, y);
                Owner.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(CurrentAngleDegrees));
            }
        }
    }
}
