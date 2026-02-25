using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace AimTrainer.Rendering
{
    public class GLContext
    {
        public GL GL { get; }

        public GLContext(IWindow window)
        {
            GL = GL.GetApi(window);
            GL.Enable(EnableCap.DepthTest);
        }

        public void Clear()
        {
            GL.ClearColor(0.1f, 0.1f, 0.15f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}