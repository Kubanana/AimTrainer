using Silk.NET.OpenGL;

namespace AimTrainer.Rendering
{
    public class Mesh
    {
        private GL _gl;
        private uint _vao;
        private uint _vbo;
        private int _vertexCount;

        public Mesh(GL gl, float[] vertices)
        {
            this._gl = gl;
            _vertexCount = vertices.Length / 3;

            _vao = gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = gl.GenBuffer();
            gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

            unsafe
            {
                fixed (float* v = vertices)
                {
                    _gl.BufferData(BufferTargetARB.ArrayBuffer, 
                        (nuint)(vertices.Length * sizeof(float)), v, BufferUsageARB.StreamDraw);
                }
            }

            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            _gl.EnableVertexAttribArray(0);
        }

        public void Draw()
        {
            _gl.BindVertexArray(_vao);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)_vertexCount);
        }

        public static Mesh CreateTriangle(GL gl)
        {
            float[] verts =
            {
                0f, 0.5f, 0f,
                -0.5f, -0.5f, 0f,
                0.5f, -0.5f, 0f  
            };

            return new Mesh(gl, verts);
        }

        public static Mesh CreateCube(GL gl, float size = 1f)
        {
            float s = size / 2f;

            float[] vertices =
            {
                //Front face
                -s, -s, s, s, -s, s, s, s, s,
                -s, -s, s, s, s, s, -s, s, s,

                //Backface
                -s, -s, -s, -s, s, -s, s, s, -s,
                -s, -s, -s, s, s, -s, s, -s, -s,
                
                //Left face
                -s, -s, -s, -s, -s, s, -s, s, s,
                -s, -s, -s, -s, s, s, -s, s, -s,

                //Right face
                s, -s, -s, s, s, -s, s, s, s,
                s, -s, -s, s, s, s, s, -s, s,

                //Top face
                -s, s, -s, -s, s, s, s, s, s,
                -s, s, -s, s, s, s, s, s, -s,

                //Bottom face
                -s, -s, -s, s, -s, -s, s, -s, s,
                -s, -s, -s, s, -s, s, -s, -s, s
            };

            return new Mesh(gl, vertices);
        }
    }
}