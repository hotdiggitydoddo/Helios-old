using SFML.Graphics;

namespace Helios.LikeARogue.Components
{
    public class SpriteComponent
    {
        public Sprite Sprite { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Color Tint { get; set; }

        public SpriteComponent()
        {
            Scale = 1.0f;
            Rotation = 0f;
            Tint = Color.White;

            Sprite = new Sprite();
        }
    }
}
