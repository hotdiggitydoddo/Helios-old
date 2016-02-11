using System;
using System.Collections.Generic;
using System.Linq;
using Helios.Core;
using Helios.LikeARogue.Components;
using Helios.LikeARogue.Generators;
using Helios.LikeARogue.Subsystems;

namespace Helios.LikeARogue
{
    public class GameWorld : World
    {
        public const int MAX_ENTITIES = 10000;
        public static Random RNG = new Random();
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
        public MeleeCombatComponent[] MeleeCombatComponents { get; }

        //Subsystems
        public HealthSubsystem HealthSubsystem { get; }
        public SpriteRendererSubsystem SpriteRendererSubsystem { get; }
        public PhysicsSubsystem PhysicsSubsystem { get; }
		public CollisionSubsystem CollisionSubsystem { get; }
		public StatusEffectsSubsystem StatusEffectsSubsystem { get; }
        public InputSubsystem InputSubsystem { get; }
        public EnvironmentalSubsystem  EnvironmentalSubsystem { get; }
        public AISubsystem AISubsystem { get; }
        public MeleeCombatSubsystem MeleeCombatSubsystem { get; }

        public GameWorld(uint maxEntities, Game game, int cellSize)
        {
            CellSize = cellSize;

            HealthComponents = new HealthComponent[MAX_ENTITIES];
            SpatialComponents = new SpatialComponent[MAX_ENTITIES];
            RegenerationComponents = new RegenerationComponent[MAX_ENTITIES];
            SpriteComponents = new SpriteComponent[MAX_ENTITIES];
            PhysicsComponents = new PhysicsComponent[MAX_ENTITIES];
			CollisionComponents = new TilemapCollisionComponent[MAX_ENTITIES];
			FlammableComponents = new FlammableComponent[MAX_ENTITIES];
            InputComponents = new InputComponent[MAX_ENTITIES];
            EnemyAIComponents = new EnemyAIComponent[MAX_ENTITIES];
            MeleeCombatComponents = new MeleeCombatComponent[MAX_ENTITIES];

            for (int i = 0; i < MAX_ENTITIES; i++)
            {
                HealthComponents[i] = new HealthComponent();
                SpatialComponents[i] = new SpatialComponent();
                RegenerationComponents[i] = new RegenerationComponent();
                SpatialComponents[i] = new SpatialComponent();
                PhysicsComponents[i] = new PhysicsComponent();
                CollisionComponents[i] = new TilemapCollisionComponent();
                FlammableComponents[i] = new FlammableComponent();
                InputComponents[i] = new InputComponent();
                EnemyAIComponents[i] = new EnemyAIComponent();
                MeleeCombatComponents[i] = new MeleeCombatComponent();
            }


            HealthSubsystem = new HealthSubsystem(this);
            PhysicsSubsystem = new PhysicsSubsystem(this);
            SpriteRendererSubsystem = new SpriteRendererSubsystem(game.Window, this);
			CollisionSubsystem = new CollisionSubsystem (this);
			StatusEffectsSubsystem = new StatusEffectsSubsystem (this);
            InputSubsystem = new InputSubsystem(this);
            EnvironmentalSubsystem = new EnvironmentalSubsystem(this);
            AISubsystem = new AISubsystem(this);
            MeleeCombatSubsystem = new MeleeCombatSubsystem(this);
        }
        public override void Update(float dt)
        {

            if (InputSubsystem.IsPlayerTurn)
                InputSubsystem.Update(dt);
            else
            {
                AISubsystem.Update(dt);
                InputSubsystem.IsPlayerTurn = true;
            }
            PhysicsSubsystem.Update(dt);
            CollisionSubsystem.Update(dt);
            EnvironmentalSubsystem.Update(dt);
            MeleeCombatSubsystem.Update(dt);
            StatusEffectsSubsystem.Update(dt);
            HealthSubsystem.Update(dt);
            SpriteRendererSubsystem.Update(dt);
        }
        public override void Render()
        {
            SpriteRendererSubsystem.Render();
        }
    }
}
