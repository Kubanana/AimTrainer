using System.Numerics;

using Silk.NET.OpenGL;

namespace AimTrainer.Rendering
{
    public class Crosshair : IDisposable
    {
        private GL _gl;
        private uint _vao;
        private uint _vbo;
        private Shader _shader;

        public Crosshair(GL gl, Shader shader)
        {
            _gl = gl;
            _shader = shader;

            float size = 0.02f;

            float[] vertices =
            {
                -size, 0f, 0f,
                size, 0f, 0f,

                0f, -size, 0f,
                0f, size, 0f  
            };

            _vao = _gl.GenVertexArray();
            _vbo = _gl.GenBuffer();

            _gl.BindVertexArray(_vao);

            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            _gl.BufferData(BufferTargetARB.ArrayBuffer, vertices, BufferUsageARB.StreamDraw);

            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            _gl.EnableVertexAttribArray(0);

            _gl.BindVertexArray(0);
        }

        public void Draw()
        {
            _gl.Disable(EnableCap.DepthTest);

            _gl.LineWidth(3f);

            _shader.Use();
            _shader.SetMatrix4("uModel", Matrix4x4.Identity);
            _shader.SetMatrix4("uView", Matrix4x4.Identity);
            _shader.SetMatrix4("uProjection", Matrix4x4.Identity);

            _gl.BindVertexArray(_vao);
            _gl.DrawArrays(PrimitiveType.Lines, 0, 4);
        }

        public void Dispose()
        {
            _gl.DeleteBuffer(_vbo);
            _gl.DeleteVertexArray(_vao);
        }
    }
}