using SFML.Graphics;
using SFML.System;

namespace Helios.LikeARogue.Subsystems
{
    public class SpriteRendererSubsystem : GameSubsystem
    {
        private RenderWindow _renderWindow;

        public SpriteRendererSubsystem(RenderWindow sb, GameWorld theWorld) : base(theWorld)
        {
            ComponentMask.SetBit(XnaGameComponentType.Sprite);
            ComponentMask.SetBit(XnaGameComponentType.Spatial);

            _renderWindow = sb;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public void Render()
        {
            foreach (var entity in RelevantEntities)
            {
                var sprite = World.SpriteComponents[entity];
                var spatial = World.SpatialComponents[entity];

                sprite.Sprite.Position = spatial.Position * World.CellSize;
                //sprite.Sprite.Scale = new Vector2f(2, 2);
                _renderWindow.Draw(sprite.Sprite);
            }
        }

      
    }
}
