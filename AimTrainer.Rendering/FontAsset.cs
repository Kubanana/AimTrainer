using SixLabors.Fonts;

namespace AimTrainer.Rendering
{
    public class FontAsset
    {
        public Font Font { get; }

        public FontAsset(string path, float size)
        {
            var collection = new FontCollection();
            var family = collection.Add(path);
            Font = family.CreateFont(size);
        }
    }
}