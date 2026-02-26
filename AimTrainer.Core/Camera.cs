using System.Numerics;

using Silk.NET.Input;

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

        public float Speed = 3f;
        public float Sensitivity = 0.001f;

        private float _yaw = -90f;
        private float _pitch = 0f;

        private Vector2 _lastMousePos;
        private bool _firstMouse = true;

        public Matrix4x4 GetViewMatrix()
        {
            return Matrix4x4.CreateLookAt(Position, Position + Front, Up);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI * Fov / 180f, AspectRatio, Near, Far);
        }

        public void Update(float delta, IKeyboard keyboard, IMouse mouse)
        {
            if (keyboard.IsKeyPressed(Key.W))
                Position += Front * Speed * delta;
            
            if (keyboard.IsKeyPressed(Key.S))
                Position -= Front * Speed * delta;

            var right = Vector3.Normalize(Vector3.Cross(Front, Up));

            if (keyboard.IsKeyPressed(Key.A))
                Position -= right * Speed * delta;

            if (keyboard.IsKeyPressed(Key.D))
                Position += right * Speed * delta;

            var currentPos = new Vector2(mouse.Position.X, mouse.Position.Y);

            Vector2 mouseDelta;

            if (_firstMouse)
            {
                mouseDelta = Vector2.Zero;
                _firstMouse = false;
            }

            else
            {
                mouseDelta = currentPos - _lastMousePos;
            }

            _lastMousePos = currentPos;
            
            _yaw += mouseDelta.X * Sensitivity;
            _pitch -= mouseDelta.Y * Sensitivity;
            
            _pitch = Math.Clamp(_pitch, -1.55f, 1.55f);
            
            UpdateVectors();
        }

        private void UpdateVectors()
        {
            Front = Vector3.Normalize(new Vector3(
                MathF.Cos(_pitch) * MathF.Cos(_yaw),
                MathF.Sin(_pitch),
                MathF.Cos(_pitch) * MathF.Sin(_yaw)
            ));
        }
    }
}