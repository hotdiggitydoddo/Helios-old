using OpenTK;

namespace Helios.LikeARogue.Subsystems
{
    public class PhysicsSubsystem : GameSubsystem
    {
        public PhysicsSubsystem(GameWorld world) : base(world)
        {
            ComponentMask.SetBit(XnaGameComponentType.Spatial);
            ComponentMask.SetBit(XnaGameComponentType.Physics);
            ComponentMask.SetBit(XnaGameComponentType.CircleCollision);
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var spatial = World.SpatialComponents[entity];
                var physics = World.PhysicsComponents[entity];
                var collision = World.CollisionComponents[entity];

                spatial.Position += physics.Velocity;

                collision.CollisionBody = new Circle(new Vector2(spatial.Position.X + 32, spatial.Position.Y + 32), 32);
            }
            base.Update(dt);
        }
    }
}
