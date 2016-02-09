using Helios.RLToolkit.Tiles;

namespace Helios.LikeARogue.Components
{
	public enum CollisionGroup
	{
		None,
		All,
		Player,
		Enemy
	}

	public class TilemapCollisionComponent
	{
		public uint? CollidedWithEntity { get; set; }
	    public Tile CollidedWithTile { get; set; }
		public CollisionGroup Group { get; set; }
		public TilemapCollisionComponent()
		{
			Group = CollisionGroup.None;
		}
	}
}

