using System.Drawing;
using System.Numerics;

using AimTrainer.Rendering;

using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

using SixLabors.Fonts;

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
        private Crosshair _crosshair = null!;
        
        private AimTrainer.Rendering.TextRenderer _textRenderer = null!;

        public Game()
        {
            var options = WindowOptions.Default;
            options.Size = new Vector2D<int>(1280, 720);
            options.Title = "AimTrainer";
            options.WindowBorder = WindowBorder.Resizable;

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Resize += OnResize;
        }

        public void Run() => _window.Run();

        private void OnLoad()
        {
            _glContext = new GLContext(_window);

            var collection = new FontCollection();
            var family = collection.Add("Arial.ttf");
            var font = family.CreateFont(32);

            _textRenderer = new AimTrainer.Rendering.TextRenderer();
            _textRenderer.Init(_glContext.GL, font);

            _shader = new Shader(_glContext.GL, Path.Combine(AppContext.BaseDirectory, "AimTrainer.Rendering", "Shaders", "Mesh", "unlit.vert"),
                                                Path.Combine(AppContext.BaseDirectory, "AimTrainer.Rendering", "Shaders", "Mesh", "unlit.frag"));

            _traingle = Mesh.CreateCube(_glContext.GL, 1f);

            var input = _window.CreateInput();
            _keyboard = input.Keyboards[0];
            _mouse = input.Mice[0];

            _mouse.Cursor.CursorMode = CursorMode.Disabled;

            _mouse.MouseMove += (_, delta) =>
            {
                _mouseDelta = delta;
            };

            _crosshair = new Crosshair(_glContext.GL, _shader);

            OnResize(_window.Size);
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
            _crosshair.Draw();

            _textRenderer.Draw($"FPS: {200}", 10, 10, 1f);
        }

        private void OnResize(Vector2D<int> size)
        {
            _glContext.GL.Viewport(0, 0, (uint)size.X, (uint)size.Y);
            _camera.AspectRatio = size.X / (float)size.Y;
        }
    }
}