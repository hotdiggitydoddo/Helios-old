namespace Helios.LikeARogue.Subsystems
{
	public class CollisionSubsystem : GameSubsystem
	{
		public CollisionSubsystem (GameWorld world) : base(world)
		{
			ComponentMask.SetBit (XnaGameComponentType.CircleCollision);
		}

		public override void Update (float dt)
		{
			foreach (var entity in RelevantEntities) 
			{
				var collision = World.CollisionComponents [entity];
				collision.CollidedWith = 9999;

				foreach (var other in RelevantEntities)
				{
					if (other == entity)
						continue;
					
					var otherCollision = World.CollisionComponents [other];
					if (collision.CollisionBody.Intersects (otherCollision.CollisionBody)) 
						collision.CollidedWith = other;
				}
			}
			base.Update (dt);
		}
	}
}

