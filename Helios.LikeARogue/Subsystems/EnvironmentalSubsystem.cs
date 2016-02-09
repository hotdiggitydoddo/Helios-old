﻿using System;
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
                var spatial = World.SpatialComponents[entity];
                var physics = World.PhysicsComponents[entity];
                var collision = World.CollisionComponents[entity];

                var collidedTile = collision.CollidedWithTile;
                if (collidedTile != null)
                {
                    if (!collidedTile.Cell.IsWalkable)
                    {
                        if (collidedTile.Type == TileType.Door)
                        {
                            collidedTile.Type = TileType.OpenDoor;
                            World.CurrentLevel.SetTileProperties(collidedTile, true, true, true);
                        }
                        spatial.Position -= physics.Velocity;
                    }
                    physics.Velocity = new Vector2f(0, 0);
                }
            }

            base.Update(dt);
        }
    }
}