using SFML.Graphics;

namespace Helios.LikeARogue.Components
{
    public class SpriteComponent : Component
    {
        public Sprite Sprite { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Color Tint { get { return Sprite.Color; } set { Sprite.Color = value; } }

        public SpriteComponent()
        {
            Sprite = new Sprite();

            Scale = 1.0f;
            Rotation = 0f;
            Tint = Color.White;

        }
    }
}
