using System.Numerics;

using AimTrainer.Rendering;

using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace AimTrainer.Core
{
    public class Game
    {
        private IWindow _window;
        private GLContext _glContext = null!;
        private Mesh _traingle = null!;
        private Shader _shader = null!;

        private Camera _camera = new Camera();
        private Matrix4x4 _model = Matrix4x4.Identity;
        private IKeyboard _keyboard = null!;
        private IMouse _mouse = null!;
        private Vector2 _mouseDelta;

        public Game()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1280, 720);
            options.Title = "AimTrainer";

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
        }

        public void Run() => _window.Run();

        private void OnLoad()
        {
            _glContext = new GLContext(_window);

            _shader = new Shader(_glContext.GL);

            _traingle = Mesh.CreateCube(_glContext.GL, 1f);

            var input = _window.CreateInput();
            _keyboard = input.Keyboards[0];
            _mouse = input.Mice[0];

            _mouse.Cursor.CursorMode = CursorMode.Disabled;

            _mouse.MouseMove += (_, delta) =>
            {
                _mouseDelta = delta;
            };
        }

        private void OnUpdate(double delta)
        {
            _camera.Update((float)delta, _keyboard, _mouse);
        }

        private void OnRender(double delta)
        {
            _glContext.Clear();

            _shader.Use();

            _shader.SetMatrix4("uModel", _model);
            _shader.SetMatrix4("uView", _camera.GetViewMatrix());
            _shader.SetMatrix4("uProjection", _camera.GetProjectionMatrix());

            _traingle.Draw();
        }
    }
}