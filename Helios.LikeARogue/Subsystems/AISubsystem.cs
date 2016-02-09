using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.LikeARogue.Components;
using Helios.RLToolkit.Tiles;
using RogueSharp;
using SFML.System;

namespace Helios.LikeARogue.Subsystems
{
    public class AISubsystem : GameSubsystem
    {
        Random RNG = new Random();
        public AISubsystem(GameWorld theWorld) : base(theWorld)
        {
            ComponentMask.SetBit(XnaGameComponentType.EnemyAI);
            ComponentMask.SetBit(XnaGameComponentType.Collision);
            ComponentMask.SetBit(XnaGameComponentType.Spatial);
            ComponentMask.SetBit(XnaGameComponentType.Physics);
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var spatial = World.SpatialComponents[entity];
                var ai = World.EnemyAIComponents[entity];

                if (ai.States.Peek() == AIStates.Patrolling)
                {
                    if (!ai.Goal.HasValue)
                    {
                        var currentCell = World.CurrentLevel.GetTile(spatial.Position).Cell;
                        var cell = World.CurrentLevel.GetRandomEmptyTile().Cell;
                        ai.Goal = new Vector2f(cell.X, cell.Y);
                        ai.CurrentPath = World.CurrentLevel.PathFinder.ShortestPath(currentCell, cell);
                    }

                    var moveChance = RNG.NextDouble() * 100f;
                    if (moveChance <= ai.MoveChance)
                    {
                        var physics = World.PhysicsComponents[entity];
                        var nextStep = new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y);
                        var velocity = nextStep - spatial.Position;
                        ai.CurrentPath.StepForward();
                        //var tiles = World.CurrentLevel.GetSurroundingTiles(spatial.Position);

                        //var walkables = tiles.Where(x => x.Cell.IsWalkable).ToArray();
                        //var index = RNG.Next(0, walkables.Length - 1);

                        //var velocity = new Vector2f(walkables[index].Cell.X, walkables[index].Cell.Y) - spatial.Position;
                        physics.Velocity = velocity;
                        break;
                    }

                }
            }
            base.Update(dt);
        }
    }
}
