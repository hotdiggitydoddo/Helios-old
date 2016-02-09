using Helios.Core;
using Helios.LikeARogue.Components;
using Helios.LikeARogue.Generators;
using Helios.LikeARogue.Subsystems;

namespace Helios.LikeARogue
{
    public class GameWorld : World
    {
        //private Game1 _game;
        public Level CurrentLevel { get; set; }
        public int CellSize { get; }
        //Components
        public HealthComponent[] HealthComponents { get; }
        public SpatialComponent[] SpatialComponents { get; }
        public RegenerationComponent[] RegenerationComponents { get; }
        public SpriteComponent[] SpriteComponents { get; }
        public PhysicsComponent[] PhysicsComponents { get; }
		public TilemapCollisionComponent[] CollisionComponents { get;}
		public FlammableComponent[] FlammableComponents { get; }
        public InputComponent[] InputComponents { get; }
        public EnemyAIComponent[] EnemyAIComponents { get; }

        //Subsystems
        public HealthSubsystem HealthSubsystem { get; }
        public SpriteRendererSubsystem SpriteRendererSubsystem { get; }
        public PhysicsSubsystem PhysicsSubsystem { get; }
		public CollisionSubsystem CollisionSubsystem { get; }
		public StatusEffectsSubsystem StatusEffectsSubsystem { get; }
        public InputSubsystem InputSubsystem { get; }
        public EnvironmentalSubsystem  EnvironmentalSubsystem { get; }
        public AISubsystem AISubsystem { get; }
        public GameWorld(uint maxEntities, Game game, int cellSize) : base(maxEntities)
        {
            CellSize = cellSize;

            HealthComponents = new HealthComponent[maxEntities];
            SpatialComponents = new SpatialComponent[maxEntities];
            RegenerationComponents = new RegenerationComponent[maxEntities];
            SpriteComponents = new SpriteComponent[maxEntities];
            PhysicsComponents = new PhysicsComponent[maxEntities];
			CollisionComponents = new TilemapCollisionComponent[maxEntities];
			FlammableComponents = new FlammableComponent[maxEntities];
            InputComponents = new InputComponent[maxEntities];
            EnemyAIComponents = new EnemyAIComponent[maxEntities];

            for (int i = 0; i < maxEntities; i++)
            {
                HealthComponents[i] = new HealthComponent();
                SpatialComponents[i] = new SpatialComponent();
                RegenerationComponents[i] = new RegenerationComponent();
                SpriteComponents[i] = new SpriteComponent();
                PhysicsComponents[i] = new PhysicsComponent();
				CollisionComponents [i] = new TilemapCollisionComponent();
				FlammableComponents [i] = new FlammableComponent ();
                InputComponents[i] = new InputComponent();
                EnemyAIComponents[i] = new EnemyAIComponent();
            }

            HealthSubsystem = new HealthSubsystem(this);
            PhysicsSubsystem = new PhysicsSubsystem(this);
            SpriteRendererSubsystem = new SpriteRendererSubsystem(game.Window, this);
			CollisionSubsystem = new CollisionSubsystem (this);
			StatusEffectsSubsystem = new StatusEffectsSubsystem (this);
            InputSubsystem = new InputSubsystem(this);
            EnvironmentalSubsystem = new EnvironmentalSubsystem(this);
            AISubsystem = new AISubsystem(this);
        }

        

        public override void Update(float dt)
        {
            if (InputSubsystem.IsPlayerTurn)
            {
                InputSubsystem.Update(dt);
            }
            else
            {
                AISubsystem.Update(dt);
                PhysicsSubsystem.Update(dt);
                CollisionSubsystem.Update(dt);
                EnvironmentalSubsystem.Update(dt);
                StatusEffectsSubsystem.Update(dt);
                HealthSubsystem.Update(dt);
                SpriteRendererSubsystem.Update(dt);

                InputSubsystem.IsPlayerTurn = true;
            }
            
        }

        public override void Render()
        {
            SpriteRendererSubsystem.Render();
        }
    }
}
