using System.Linq;

namespace Helios.LikeARogue.Subsystems
{
	public class StatusEffectsSubsystem : GameSubsystem
	{
		public StatusEffectsSubsystem(GameWorld world) : base(world)
        {
			ComponentMask.SetBit (XnaGameComponentType.CircleCollision);
			ComponentMask.SetBit (XnaGameComponentType.Flammable);
		}

		public override void Update (float dt)
		{
			foreach (var entity in RelevantEntities)
			{
			    var collision = World.CollisionComponents.Single(x => x.Owner == entity);

			    var flammable = World.FlammableComponents.Single(x => x.Owner == entity);

				if (collision.CollidedWithEntity != null)
				{
				    flammable.ElapsedTime = 0f;
				    flammable.Cooldown = 0;
					flammable.Duration = 10;
					flammable.Frequency = 3.5f;
				}

			    if (flammable.OnFire)
			    {
			        if (flammable.Cooldown <= 0)
			        {
			            flammable.Damage = 7;
			            flammable.Cooldown = flammable.Frequency;
			        }
			        else
			        {
			            flammable.Cooldown -= dt;
			            flammable.Damage = 0;
			        }
			        flammable.ElapsedTime += dt;
			    }

            }
			base.Update (dt);
		}
	}
}

