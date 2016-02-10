﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.LikeARogue.Components;
using Helios.LikeARogue.Generators;
using Helios.RLToolkit.Generators;
using Helios.RLToolkit.Gfx;
using Helios.RLToolkit.Tiles;
using OpenTK;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Helios.LikeARogue
{
    class LikeARogue : Game
    {
        private GameWorld _gameWorld;
        private uint player;
        private uint orc;
        private Font _font;
        private Text _text;
        private Tilemap _map;
        private TextureAtlas _atlas;
        private Level _level;

        private const int CELL_SIZE = 8;
        private const int SPRITE_SIZE = 8;

        public LikeARogue() : base(1280, 720, "Like-A-Rogue!", new Color(101, 156, 239))
        {
        }

        protected override void LoadContent()
        {
            _font = new Font("Super Mario Bros.ttf");
            _text = new Text { Font = _font };
            _text.CharacterSize = 22;
            _text.Color = Color.White;

            _gameWorld = new GameWorld(10000, this, CELL_SIZE);

            _atlas = new TextureAtlas(new Texture("Content/terminal8x8.png"), 8);
            _atlas.Items.Add(TileType.Wall, new Vector2f(3, 2));
            _atlas.Items.Add(TileType.Floor, new Vector2f(14, 2));
            _atlas.Items.Add(TileType.RoomFloor, new Vector2f(14, 2));
            _atlas.Items.Add(TileType.Door, new Vector2f(11, 2));
            _atlas.Items.Add(TileType.OpenDoor, new Vector2f(13, 2));

            _level = Level.Generate(_gameWorld, 101, 101, _atlas, _gameWorld.CellSize);
            _gameWorld.CurrentLevel = _level;

            player = _gameWorld.EntityManager.CreateEntity();
            _level.Player = player;

            _gameWorld.HealthComponents.Add(new HealthComponent { MaxHealth = 100, CurrentHealth = 100, Owner = player });

            var sprite = new SpriteComponent { Sprite = new Sprite(_atlas.Texture), Owner = player };
            sprite.Sprite.TextureRect = new IntRect(new Vector2i(0, 4 * _atlas.SpriteSize), new Vector2i(_atlas.SpriteSize, _atlas.SpriteSize));
            _gameWorld.SpriteComponents.Add(sprite);

            var start = _level.GetRandomEmptyTile();
            _gameWorld.SpatialComponents.Add(new SpatialComponent { Position = new Vector2f(start.Cell.X, start.Cell.Y), Owner = player });

            _gameWorld.PhysicsComponents.Add(new PhysicsComponent { Owner = player });

            _gameWorld.CollisionComponents.Add(new TilemapCollisionComponent { Group =  CollisionGroup.Player, Owner = player });
            _gameWorld.InputComponents.Add(new InputComponent { Owner = player });


            _gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Health);
            //_gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Regeneration);
            _gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Sprite);
            _gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Spatial);
            _gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Physics);
            _gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Collision);
            //_gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Flammable);
            _gameWorld.EntityManager.AddComponent(player, XnaGameComponentType.Input);

            /*--------------------------*/

            for (int i = 1; i < 100; i++)
            {
                var ent = _gameWorld.EntityManager.CreateEntity();
                _gameWorld.HealthComponents.Add(new HealthComponent { MaxHealth = 25, CurrentHealth = 25, Owner = ent });

                sprite = new SpriteComponent { Sprite = new Sprite(_atlas.Texture), Owner = ent };
                sprite.Sprite.TextureRect = new IntRect(new Vector2i(15 * _atlas.SpriteSize, 6 * _atlas.SpriteSize), new Vector2i(_atlas.SpriteSize, _atlas.SpriteSize));
                sprite.Tint = new Color(27, 126, 1);
                _gameWorld.SpriteComponents.Add(sprite);

                start = _level.GetRandomEmptyTile();
                _gameWorld.SpatialComponents.Add(new SpatialComponent { Position = new Vector2f(start.Cell.X, start.Cell.Y), Owner = ent });
                _gameWorld.PhysicsComponents.Add(new PhysicsComponent { Owner = ent });
                _gameWorld.CollisionComponents.Add(new TilemapCollisionComponent { Group = CollisionGroup.Enemy, Owner = ent });
                _gameWorld.EnemyAIComponents.Add(new EnemyAIComponent { MoveChance = 30, Owner = ent });

                _gameWorld.EntityManager.AddComponent(ent, XnaGameComponentType.Sprite);
                _gameWorld.EntityManager.AddComponent(ent, XnaGameComponentType.Spatial);
                _gameWorld.EntityManager.AddComponent(ent, XnaGameComponentType.Physics);
                _gameWorld.EntityManager.AddComponent(ent, XnaGameComponentType.Collision);
                _gameWorld.EntityManager.AddComponent(ent, XnaGameComponentType.EnemyAI);
            }

            //orc = _gameWorld.EntityManager.CreateEntity();
            //_gameWorld.HealthComponents.Add(new HealthComponent { MaxHealth = 25, CurrentHealth = 25, Owner = orc });

            //sprite = new SpriteComponent { Sprite = new Sprite(_atlas.Texture), Owner = orc };
            //sprite.Sprite.TextureRect = new IntRect(new Vector2i(15 * _atlas.SpriteSize, 6 * _atlas.SpriteSize), new Vector2i(_atlas.SpriteSize, _atlas.SpriteSize));
            //sprite.Tint = new Color(27, 126, 1);
            //_gameWorld.SpriteComponents.Add(sprite);

            //start = _level.GetRandomEmptyTile();
            //_gameWorld.SpatialComponents.Add(new SpatialComponent { Position = new Vector2f(start.Cell.X, start.Cell.Y), Owner = orc });
            //_gameWorld.PhysicsComponents.Add(new PhysicsComponent { Owner = orc });
            //_gameWorld.CollisionComponents.Add(new TilemapCollisionComponent { Group = CollisionGroup.Enemy, Owner = orc });
            //_gameWorld.EnemyAIComponents.Add(new EnemyAIComponent { MoveChance = 30, Owner = orc });

            //_gameWorld.EntityManager.AddComponent(orc, XnaGameComponentType.Sprite);
            //_gameWorld.EntityManager.AddComponent(orc, XnaGameComponentType.Spatial);
            //_gameWorld.EntityManager.AddComponent(orc, XnaGameComponentType.Physics);
            //_gameWorld.EntityManager.AddComponent(orc, XnaGameComponentType.Collision);
            //_gameWorld.EntityManager.AddComponent(orc, XnaGameComponentType.EnemyAI);

            // _gameWorld.SpatialComponents[player].Position = _gameWorld.SpatialComponents[orc].Position;
            //var entity1 = _gameWorld.EntityManager.CreateEntity();

            //_gameWorld.HealthComponents[entity1].MaxHealth = 100;
            //_gameWorld.HealthComponents[entity1].CurrentHealth = 100;

            //_gameWorld.SpatialComponents[entity1].Position = new Vector2f(210, 210);
            //_gameWorld.SpriteComponents[entity1].Sprite.Texture = new Texture("blue-monster.png");

            //_gameWorld.CollisionComponents[entity1].Group = CollisionGroup.Enemy;
            //_gameWorld.CollisionComponents[entity1].CollisionBody = new Circle(new Vector2(242, 242), 32);

            //_gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Health);
            //_gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Sprite);
            //_gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Spatial);
            //_gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.Physics);
            //_gameWorld.EntityManager.AddComponent(entity1, XnaGameComponentType.CircleCollision);

            //Window.SetKeyRepeatEnabled(false);
        }

        protected override void Initialize()
        {
            //  throw new NotImplementedException();
            Window.KeyPressed += WindowOnKeyReleased;
        }

        private void WindowOnKeyReleased(object sender, KeyEventArgs keyEventArgs)
        {
            var input = _gameWorld.InputComponents.Single(x => x.Owner == player);

            input.WasKeyPressed = true;
            input.KeyPress = keyEventArgs.Code;
        }

        protected override void Tick(float elapsed)
        {
            //if (!keyPressed)
            //{
            //    _gameWorld.PhysicsComponents[entity].Velocity = new Vector2f(0, 0);
            //    _gameWorld.Update(elapsed);
            //    _level.UpdatePlayerFov(_gameWorld.SpatialComponents[entity].Position);
            //    return;
            //}
            _gameWorld.Update(elapsed);
        }

        protected override void Render()
        {
            Window.Draw(_level);
            _gameWorld.SpriteRendererSubsystem.Render();
            //  _text.DisplayedString = string.Format("Penguin's Health: {0}", _gameWorld.HealthComponents[player].CurrentHealth);
            var ppos = _gameWorld.SpatialComponents.Single(x => x.Owner == player).Position.ToString();
          //  var opos = _gameWorld.SpatialComponents.Single(x => x.Owner == orc).Position.ToString();
          //  _text.DisplayedString = string.Format("P pos: {0} | O pos: {1} || O AIState: {2}", ppos, opos, _gameWorld.EnemyAIComponents.Single(x => x.Owner == orc).States.Peek());
            Window.Draw(_text);
        }
    }
}
