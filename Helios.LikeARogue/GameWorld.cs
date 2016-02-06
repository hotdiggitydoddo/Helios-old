using Helios.Core;
using Helios.LikeARogue.Components;
using Helios.LikeARogue.Subsystems;

namespace Helios.LikeARogue
{
    public class GameWorld : World
    {
        //private Game1 _game;

        //Components
        public HealthComponent[] HealthComponents { get; }
        public SpatialComponent[] SpatialComponents { get; }
        public RegenerationComponent[] RegenerationComponents { get; }
        public SpriteComponent[] SpriteComponents { get; }
        public PhysicsComponent[] PhysicsComponents { get; }
		public CircleCollisionComponent[] CollisionComponents { get;}
		public FlammableComponent[] FlammableComponents { get; }

        //Subsystems
        public HealthSubsystem HealthSubsystem { get; }
        public SpriteRendererSubsystem SpriteRendererSubsystem { get; }
        public PhysicsSubsystem PhysicsSubsystem { get; }
		public CollisionSubsystem CollisionSubsystem { get; }
		public StatusEffectsSubsystem StatusEffectsSubsystem { get; }

        public GameWorld(uint maxEntities, Game game) : base(maxEntities) 
        {
            HealthComponents = new HealthComponent[maxEntities];
            SpatialComponents = new SpatialComponent[maxEntities];
            RegenerationComponents = new RegenerationComponent[maxEntities];
            SpriteComponents = new SpriteComponent[maxEntities];
            PhysicsComponents = new PhysicsComponent[maxEntities];
			CollisionComponents = new CircleCollisionComponent[maxEntities];
			FlammableComponents = new FlammableComponent[maxEntities];

            for (int i = 0; i < maxEntities; i++)
            {
                HealthComponents[i] = new HealthComponent();
                SpatialComponents[i] = new SpatialComponent();
                RegenerationComponents[i] = new RegenerationComponent();
                SpriteComponents[i] = new SpriteComponent();
                PhysicsComponents[i] = new PhysicsComponent();
				CollisionComponents [i] = new CircleCollisionComponent ();
				FlammableComponents [i] = new FlammableComponent ();
            }

            HealthSubsystem = new HealthSubsystem(this);
            PhysicsSubsystem = new PhysicsSubsystem(this);
            SpriteRendererSubsystem = new SpriteRendererSubsystem(game.Window, this);
			CollisionSubsystem = new CollisionSubsystem (this);
			StatusEffectsSubsystem = new StatusEffectsSubsystem (this);
        }

        public override void Update(float dt)
        {
			CollisionSubsystem.Update(dt);
			StatusEffectsSubsystem.Update (dt);
            HealthSubsystem.Update(dt);
            PhysicsSubsystem.Update(dt);
            SpriteRendererSubsystem.Update(dt);
        }

        public override void Render()
        {
            SpriteRendererSubsystem.Render();
        }
    }
}
