using System.Collections.Generic;
using System.Linq;
using Helios.RLToolkit.Generators;
using SFML.System;

namespace Helios.LikeARogue.Subsystems
{
    public class CollisionSubsystem : GameSubsystem
    {
        public List<uint> EntitiesWithCollisions { get; }
        public CollisionSubsystem(GameWorld world) : base(world)
        {
            ComponentMask.SetBit(XnaGameComponentType.Collision);
            ComponentMask.SetBit(XnaGameComponentType.Spatial);
            ComponentMask.SetBit(XnaGameComponentType.Physics);
        }

        public override void Update(float dt)
        {
            CheckCollisions();
            base.Update(dt);
        }

        private void CheckCollisions()
        {
            foreach (var entity in RelevantEntities)
            {
                var spatial = World.SpatialComponents[entity];
                var collision = World.CollisionComponents[entity];

                collision.CollidedWithEntity = null;
                collision.CollidedWithTile = null;

                var physics = World.PhysicsComponents[entity];
                if (physics.Velocity != new Vector2f(0, 0))
                {
                    var tile = World.CurrentLevel.GetTile((int) spatial.Position.X, (int) spatial.Position.Y);
                    collision.CollidedWithTile = tile;

                    if (tile.Entity != null)
                        collision.CollidedWithEntity = tile.Entity;
                }
                


                //foreach (var other in RelevantEntities)
                //{
                //    if (other == entity)
                //        continue;

                //    var otherSpatial = World.SpatialComponents.Single(x => x.Owner == other);
                //    if (spatial.Position == otherSpatial.Position)
                //    {
                //        collision.CollidedWithEntity = other;
                //    }
                //}
            }
        }
        public override void LateUpdate()
        {
           EntitiesWithCollisions.Clear();
        }
    }
}

