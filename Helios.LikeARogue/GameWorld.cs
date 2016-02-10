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
        //private Game1 _game;
        public static Random RNG = new Random();
        public Level CurrentLevel { get; set; }
        public int CellSize { get; }
        //Components
        public List<HealthComponent> HealthComponents { get; }
        public List<SpatialComponent> SpatialComponents { get; }
        public List<RegenerationComponent> RegenerationComponents { get; }
        public List<SpriteComponent> SpriteComponents { get; }
        public List<PhysicsComponent> PhysicsComponents { get; }
		public List<TilemapCollisionComponent> CollisionComponents { get;}
		public List<FlammableComponent> FlammableComponents { get; }
        public List<InputComponent> InputComponents { get; }
        public List<EnemyAIComponent> EnemyAIComponents { get; }

        //Subsystems
        public HealthSubsystem HealthSubsystem { get; }
        public SpriteRendererSubsystem SpriteRendererSubsystem { get; }
        public PhysicsSubsystem PhysicsSubsystem { get; }
		public CollisionSubsystem CollisionSubsystem { get; }
		public StatusEffectsSubsystem StatusEffectsSubsystem { get; }
        public InputSubsystem InputSubsystem { get; }
        public EnvironmentalSubsystem  EnvironmentalSubsystem { get; }
        public AISubsystem AISubsystem { get; }
        public GameWorld(uint maxEntities, Game game, int cellSize)
        {
            CellSize = cellSize;

            HealthComponents = new List<HealthComponent>();
            SpatialComponents = new List<SpatialComponent>();
            RegenerationComponents = new List<RegenerationComponent>();
            SpriteComponents = new List<SpriteComponent>();
            PhysicsComponents = new List<PhysicsComponent>();
			CollisionComponents = new List<TilemapCollisionComponent>();
			FlammableComponents = new List<FlammableComponent>();
            InputComponents = new List<InputComponent>();
            EnemyAIComponents = new List<EnemyAIComponent>();


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
                InputSubsystem.Update(dt);
            else
            {
                AISubsystem.Update(dt);
                InputSubsystem.IsPlayerTurn = true;
            }
            PhysicsSubsystem.Update(dt);
            CollisionSubsystem.Update(dt);
            EnvironmentalSubsystem.Update(dt);
            StatusEffectsSubsystem.Update(dt);
            HealthSubsystem.Update(dt);
            SpriteRendererSubsystem.Update(dt);

            CurrentLevel.UpdatePlayerFov(SpatialComponents.Single(x => x.Owner == CurrentLevel.Player).Position);

        }
        public override void Render()
        {
            SpriteRendererSubsystem.Render();
        }
    }
}
