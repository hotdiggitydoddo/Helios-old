using Helios.RLToolkit.Generators;
using OpenTK;
using SFML.Graphics;
using SFML.System;

namespace Helios.LikeARogue.Subsystems
{
    public class PhysicsSubsystem : GameSubsystem
    {
        public PhysicsSubsystem(GameWorld world) : base(world)
        {
            ComponentMask.SetBit(XnaGameComponentType.Spatial);
            ComponentMask.SetBit(XnaGameComponentType.Physics);
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var spatial = World.SpatialComponents[entity];
                var physics = World.PhysicsComponents[entity];

                spatial.Position += physics.Velocity;
            }
            base.Update(dt);
        }
    }
}
