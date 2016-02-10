using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.Core;
using SFML.System;
using SFML.Window;

namespace Helios.LikeARogue.Subsystems
{
    public class InputSubsystem : GameSubsystem
    {
        public bool IsPlayerTurn { get; set; }
        public InputSubsystem(GameWorld world) : base(world)
        {
            ComponentMask.SetBit(XnaGameComponentType.Physics);
            ComponentMask.SetBit(XnaGameComponentType.Input);
            IsPlayerTurn = true;
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var input = World.InputComponents[entity];
                if (!input.WasKeyPressed) continue;

                var physics = World.PhysicsComponents[entity];

                float x = 0f, y = 0f;

                switch (input.KeyPress)
                {
                    case Keyboard.Key.Right:
                        x = 1;
                        break;
                    case Keyboard.Key.Left:
                        x = -1;
                        break;
                    case Keyboard.Key.Up:
                        y = -1;
                        break;
                    case Keyboard.Key.Down:
                        y = 1;
                        break;

                }

                physics.Velocity = new Vector2f(x, y);
                input.WasKeyPressed = false;
                IsPlayerTurn = false;

                //if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                //{
                //    _gameWorld.HealthComponents[entity].Damage = 27;
                //    keyPressed = false;
                //}
                //if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                //{
                //    _gameWorld.RegenerationComponents[entity].AmountToHeal = 500;
                //    _gameWorld.RegenerationComponents[entity].Frequency = 1f;
                //    _gameWorld.EntityManager.AddComponent(entity, XnaGameComponentType.Regeneration);
                //    keyPressed = false;
                //}
                //if (Keyboard.IsKeyPressed(Keyboard.Key.C))
                //{
                //    _gameWorld.EntityManager.RemoveComponent(entity, XnaGameComponentType.Regeneration);
                //    keyPressed = false;
                //}
            }

            base.Update(dt);
        }
    }
}
