using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.RLToolkit.Generators;
using SFML.System;

namespace Helios.LikeARogue.Subsystems
{
    public class EnvironmentalSubsystem : GameSubsystem
    {
        public EnvironmentalSubsystem(GameWorld theWorld) : base(theWorld)
        {
            ComponentMask.SetBit(XnaGameComponentType.Collision);
            ComponentMask.SetBit(XnaGameComponentType.Physics);
            ComponentMask.SetBit(XnaGameComponentType.Spatial);
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var spatial = World.SpatialComponents.Single(x => x.Owner == entity);
                var physics = World.PhysicsComponents.Single(x => x.Owner == entity);
                var collision = World.CollisionComponents.Single(x => x.Owner == entity);

                var collidedTile = collision.CollidedWithTile;
                if (collidedTile != null)
                {
                    if (!collidedTile.Cell.IsWalkable)
                    {
                        spatial.Position -= physics.Velocity;
                        collidedTile.Entity = null;
                    }

                    if (collidedTile.Type == TileType.Door)
                    {
                        collidedTile.Type = TileType.OpenDoor;
                        World.CurrentLevel.SetTileProperties(collidedTile, true, true, entity == World.CurrentLevel.Player);
                        spatial.Position -= physics.Velocity;
                        collidedTile.Entity = null;
                    }
                    if (entity == World.CurrentLevel.Player)
                        World.CurrentLevel.UpdatePlayerFov(spatial.Position);

                    World.CurrentLevel.GetTile(spatial.Position).Entity = entity;
                    physics.Velocity = new Vector2f(0, 0);
                }
            }

            base.Update(dt);
        }
    }
}
