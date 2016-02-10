namespace Helios.LikeARogue.Components
{
	//public enum CollisionGroup
	//{
	//	None,
	//	All,
	//	Player,
	//	Enemy
	//}

	public class CircleCollisionComponent : Component
	{
		public uint CollidedWith { get; set; }
		public CollisionGroup Group { get; set; }
		public Circle CollisionBody { get; set;}
		public CircleCollisionComponent ()
		{
			Group = CollisionGroup.None;
			CollidedWith = 9999;

		}
	}
}

