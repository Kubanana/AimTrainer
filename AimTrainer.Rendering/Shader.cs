using System.Numerics;

using Silk.NET.OpenGL;

namespace AimTrainer.Rendering
{
    public class Shader
    {
        private GL _gl;
        private uint _program;

        public Shader(GL gl, string vertPath, string fragPath)
        {
            this._gl = gl;

            string vertSrc = File.ReadAllText(vertPath);
            string fragSrc = File.ReadAllText(fragPath);

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

        public void SetMatrix4(string name, Matrix4x4 matrix)
        {
            int location = _gl.GetUniformLocation(_program, name);

            unsafe
            {
                _gl.UniformMatrix4(location, 1, false, (float*)&matrix);
            }
        }

        public void Use() => _gl.UseProgram(_program);
    }
}