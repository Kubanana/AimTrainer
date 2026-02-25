using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

public class Program
{
    private static GL? _gl;
    private static uint _vao;
    private static uint _vbo;

    static void Main()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 720);
        options.Title = "AimTrainer";

        var window = Window.Create(options);

        window.Load += () =>
        {
            _gl = GL.GetApi(window);

            float[] _vertices =
            {
                0.0f, 0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.5f,  -0.5f, 0.0f
            };

            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

            unsafe
            {
                fixed (float* v = _vertices)
                {
                    _gl.BufferData(BufferTargetARB.ArrayBuffer, 
                        (nuint)(_vertices.Length * sizeof(float)), v, BufferUsageARB.StreamDraw);
                }
            }

            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            _gl.EnableVertexAttribArray(0);
        };

        window.Render += delta =>
        {
            _gl.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);
            _gl.Clear(ClearBufferMask.ColorBufferBit);

            _gl.BindVertexArray(_vao);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
        };

        window.Run();
    }
}