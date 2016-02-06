using Helios.Core;
using OpenTK;

namespace Helios.LikeARogue
{
    class XnaGameComponentType : ComponentType
    {
        public const int Health = 1;
        public const int Flammable = 2;
        public const int Sprite = 3;
        public const int Regeneration = 4;
        public const int Spatial = 5;
        public const int Physics = 6;
        public const int Input = 7;
		public const int CircleCollision = 8;
    }


		public struct Circle
		{
			public Vector2 Center { get; set; }
			public float Radius { get; set; }

			public Circle(Vector2 center, float radius)
			{
				Center = center;
				Radius = radius;
			}

			public bool Contains(Vector2 point)
			{
			    return (point - Center).Length <= Radius;
			}

			public bool Intersects(Circle other)
			{
			    return ((other.Center - Center).X*(other.Center - Center).X) +
			           ((other.Center - Center).Y*(other.Center - Center).Y) < Radius * Radius;

			}
		}

}
