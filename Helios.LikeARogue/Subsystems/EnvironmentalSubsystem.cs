using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.LikeARogue.Components;
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
                var spatial = World.SpatialComponents[entity];
                var physics = World.PhysicsComponents[entity];
                var collision = World.CollisionComponents[entity];

                var blockedByEntity = false;

                var collidedEntity = collision.CollidedWithEntity;
                if (collidedEntity.HasValue)
                {
                    var otherCollision = World.CollisionComponents[collidedEntity.Value];
                    switch (otherCollision.Group)
                    {
                        case CollisionGroup.Enemy:
                            spatial.Position -= physics.Velocity;
                            blockedByEntity = true;
                            break;
                        case CollisionGroup.Player:
                            spatial.Position -= physics.Velocity;
                            blockedByEntity = true;
                            break;
                    }
                }

                var collidedTile = collision.CollidedWithTile;
                if (collidedTile != null)
                {
                    if (!collidedTile.Cell.IsWalkable)
                    {
                        spatial.Position -= physics.Velocity;
                        collidedTile.Entity = null;
                    }
                    else if (!blockedByEntity)
                        World.CurrentLevel.GetTile(spatial.Position - physics.Velocity).Entity = null;

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
