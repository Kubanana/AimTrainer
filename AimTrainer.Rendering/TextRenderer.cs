using System.Numerics;

using Silk.NET.OpenGL;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AimTrainer.Rendering
{
    public class TextRenderer
    {
        private GL _gl = null!;
        private Shader _shader = null!;
        private uint _vao;
        private uint _vbo;
        private Font _font = null!;

        private Dictionary<char, Glyph> _glyphs = new();

        public void Init(GL gl, Font font)
        {
            _gl = gl;
            _font = font;

            _shader = new Shader(_gl, Path.Combine(AppContext.BaseDirectory, "AimTrainer.Rendering", "Shaders", "Text", "text.vert"), 
                                      Path.Combine(AppContext.BaseDirectory, "AimTrainer.Rendering", "Shaders", "Text", "text.frag"));

            SetupQuad();
        }

        private Glyph GetGlyph(char c)
        {
            if (!_glyphs.TryGetValue(c, out var g))
            {
                g = Bake(c);
                _glyphs[c] = g;
            }

            return g;
        }

        private Glyph Bake(char c)
        {
            var options = new TextOptions(_font);
            var size = TextMeasurer.MeasureSize(c.ToString(), options);

            using var img = new Image<Rgba32>((int)size.Width, (int)size.Height);
            img.Mutate(x => x.DrawText(c.ToString(), _font, Color.White, new PointF(0, 0)));

            var pixels = new byte[img.Width * img.Height * 4];
            img.CopyPixelDataTo(pixels);

            uint tex = _gl.GenTexture();
            _gl.BindTexture(TextureTarget.Texture2D, tex);

            unsafe
            {
                fixed(byte* p = pixels)
                {
                    _gl.TexImage2D(TextureTarget.Texture2D, 0, 
                        InternalFormat.Rgba, (uint)img.Width, 
                        (uint)img.Height, 0, PixelFormat.Rgba,
                         PixelType.UnsignedByte, p);
                }
            }

            _gl.TextureParameter(tex, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            _gl.TextureParameter(tex, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

            return new Glyph
            {
                Texture = tex,
                Width = img.Width,
                Height = img.Height,
                Advance = img.Width  
            };
        }

        public void Draw(string text, float x, float y, float scale)
        {
            _shader.Use();
            _gl.BindVertexArray(_vao);

            foreach (char c in text)
            {
                var g = GetGlyph(c);
                DrawGlyph(g, ref x, y, scale);
            }

            _gl.BindVertexArray(0);
        }

        private void DrawGlyph(Glyph g, ref float x, float y, float scale)
        {
            _gl.BindTexture(TextureTarget.Texture2D, g.Texture);

            Matrix4x4 model = Matrix4x4.CreateScale(g.Width * scale, g.Height * scale, 1) *
                              Matrix4x4.CreateTranslation(x, y, 0);

            _shader.SetMatrix4("uModel", model);

            _gl.BindVertexArray(_vao);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
            _gl.BindVertexArray(0);
            
            x += g.Advance * scale;
        }

        private void SetupQuad()
        {
            float[] vertices =
            {
                0f, 1f, 0f, 1f,
                0f, 0f, 0f, 0f,
                1f, 0f, 1f, 0f,

                0f, 1f, 0f, 1f,
                1f, 0f, 1f, 0f,
                1f, 1f, 1f, 1f
            };

            _vao = _gl.GenVertexArray();
            _vbo = _gl.GenBuffer();

            _gl.BindVertexArray(_vao);
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            unsafe
            {
                fixed(float* v = vertices)
                {
                    _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)), v, BufferUsageARB.StaticDraw);
                }

                _gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)0);
                _gl.EnableVertexAttribArray(0);

                _gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), (void*)(2 * sizeof(float)));
                _gl.EnableVertexAttribArray(1);

                _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
                _gl.BindVertexArray(0);
            }
            
        }
    }
}