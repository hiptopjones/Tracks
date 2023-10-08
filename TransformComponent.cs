using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracks
{
    internal class TransformComponent : Component
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public Vector3 Scale { get; set; } = Vector3.One;

        public Vector3 Right => Vector3.Transform(Vector3.UnitX, Rotation).Normalized();
        public Vector3 Left => Vector3.Transform(-Vector3.UnitX, Rotation).Normalized();

        public Vector3 Up => Vector3.Transform(Vector3.UnitY, Rotation).Normalized();
        public Vector3 Down => Vector3.Transform(-Vector3.UnitY, Rotation).Normalized();

        public Vector3 Forward => Vector3.Transform(-Vector3.UnitZ, Rotation).Normalized();
        public Vector3 Backward => Vector3.Transform(Vector3.UnitZ, Rotation).Normalized();

        public override void Reset()
        {
            Position = new Vector3();
            Rotation = new Quaternion();
            Scale = new Vector3();
        }

        public override string ToString()
        {
            return $"[TransformComponent] Position({Position}) Rotation({Rotation}) Scale({Scale})";
        }
    }
}
