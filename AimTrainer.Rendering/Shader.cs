using Silk.NET.OpenGL;

namespace AimTrainer.Rendering
{
    public class Shader
    {
        private GL _gl;
        private uint _program;

        public Shader(GL gl)
        {
            this._gl = gl;

            string basePath = AppContext.BaseDirectory;

            string vertSrc = File.ReadAllText(Path.Combine(basePath, "AimTrainer.Rendering", "Shaders", "Mesh", "unlit.vert"));
            string fragSrc = File.ReadAllText(Path.Combine(basePath, "AimTrainer.Rendering", "Shaders", "Mesh", "unlit.frag"));

            uint vert = Compile(ShaderType.VertexShader, vertSrc);
            uint frag = Compile(ShaderType.FragmentShader, fragSrc);

            _program = _gl.CreateProgram();
            _gl.AttachShader(_program, vert);
            _gl.AttachShader(_program, frag);
            _gl.LinkProgram(_program);

            _gl.DeleteShader(vert);
            _gl.DeleteShader(frag);
        }

        uint Compile(ShaderType type, string src)
        {
            uint shader = _gl.CreateShader(type);
            _gl.ShaderSource(shader, src);
            _gl.CompileShader(shader);

            return shader;
        }

        public void Use() => _gl.UseProgram(_program);
    }
}