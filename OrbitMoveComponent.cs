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

        public override void Update(float deltaTime)
        {
            CurrentAngleDegrees += AngularSpeedDegrees * deltaTime;

            float x = OrbitalRadius * (float)Math.Cos(MathHelper.DegreesToRadians(CurrentAngleDegrees));
            float y = OrbitalRadius * (float)Math.Sin(MathHelper.DegreesToRadians(CurrentAngleDegrees));

            Owner.Transform.Position = OrbitalTarget.Transform.Position + new Vector3(x, 0, y);
            Owner.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(CurrentAngleDegrees));
        }
    }
}
