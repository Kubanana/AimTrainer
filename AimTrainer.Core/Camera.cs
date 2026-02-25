using System.Numerics;

namespace AimTrainer.Core
{
    public class Camera
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 3);
        public Vector3 Front { get; private set; } = new Vector3(0, 0, -1);
        public Vector3 Up { get; private set; } = Vector3.UnitY;

        public float Fov {get ; set; } = 80f;
        public float AspectRatio { get; set; } = 4 / 3f;
        public float Near { get; set; } = 0.01f;
        public float Far { get; set; } = 1000f;

        public Matrix4x4 GetViewMatrix()
        {
            return Matrix4x4.CreateLookAt(Position, Position + Front, Up);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI * Fov / 180f, AspectRatio, Near, Far);
        }
    }
}