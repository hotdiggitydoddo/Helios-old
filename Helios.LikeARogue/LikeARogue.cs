using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.LikeARogue.Components;
using OpenTK;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Helios.LikeARogue
{
    class LikeARogue : Game
    {
        private GameWorld _gameWorld;
        private uint entity;
        private Font _font;
        private Text _text;

        public LikeARogue() : base(800, 600, "Like-A-Rogue!", new Color(101, 156, 239))
        {
        }

        protected override void LoadContent()
        {
            _font = new Font("Super Mario Bros.ttf");
            _text = new Text {Font = _font};
            _text.CharacterSize = 54;
            _text.Color = Color.White;
          //  _text.Style = Text.Styles.Bold;

            _gameWorld = new GameWorld(10000, this);
            entity = _gameWorld.EntityManager.CreateEntity();


            _gameWorld.HealthComponents[entity].MaxHealth = 100;
            _gameWorld.HealthComponents[entity].CurrentHealth = 100;

            _gameWorld.SpatialComponents[entity].Position = new Vector2f(10, 10);
            _gameWorld.SpriteComponents[entity].Sprite.Texture = new Texture("blue-monster.png");

            _gameWorld.CollisionComponents[entity].Group = CollisionGroup.Player;
            _gameWorld.CollisionComponents[entity].CollisionBody = new Circle(new Vector2(42, 42), 32);

            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Health);
            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Regeneration);
            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Sprite);
            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Spatial);
            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Physics);
            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.CircleCollision);
            _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Flammable);

            var entity1 = _gameWorld.EntityManager.CreateEntity();

            _gameWorld.HealthComponents[entity1].MaxHealth = 100;
            _gameWorld.HealthComponents[entity1].CurrentHealth = 100;

            _gameWorld.SpatialComponents[entity1].Position = new Vector2f(210, 210);
            _gameWorld.SpriteComponents[entity1].Sprite.Texture = new Texture("blue-monster.png");

            _gameWorld.CollisionComponents[entity1].Group = CollisionGroup.Enemy;
            _gameWorld.CollisionComponents[entity1].CollisionBody = new Circle(new Vector2(242, 242), 32);

            _gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Health);
            _gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Sprite);
            _gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Spatial);
            _gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Physics);
            _gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.CircleCollision);

        }

        protected override void Initialize()
        {
            //  throw new NotImplementedException();
        }

        protected override void Tick(float elapsed)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                _gameWorld.HealthComponents[entity].Damage = 27;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
            {
                _gameWorld.RegenerationComponents[entity].AmountToHeal = 500;
                _gameWorld.RegenerationComponents[entity].Frequency = 1f;
                _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Regeneration);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.C))
            {
                _gameWorld.EntityManager.RemoveComponent(entity, XnaGameComponentType.Regeneration);
            }




            float x = 0f, y = 0f;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                x = 400 * elapsed;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                x = -400 * elapsed;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                y = 400 * elapsed;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                y = -400 * elapsed;
            }


            _gameWorld.PhysicsComponents[entity].Velocity = new Vector2f(x, y);

            _gameWorld.Update(elapsed);
        }

        protected override void Render()
        {
            _gameWorld.SpriteRendererSubsystem.Render();
            _text.DisplayedString = string.Format("Penguin's Health: {0}", _gameWorld.HealthComponents[entity].CurrentHealth);
            Window.Draw(_text);
        }
    }
}
