using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helios.LikeARogue.Components;
using Helios.RLToolkit.Generators;
using Helios.RLToolkit.Tiles;
using RogueSharp;
using RogueSharp.DiceNotation;
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
                var spatial = World.SpatialComponents.Single(x => x.Owner == entity);
                var ai = World.EnemyAIComponents.Single(x => x.Owner == entity);

                if (ai.TurnDelay > 0)
                {
                    ai.TurnDelay--;
                    continue;
                }

                //If not already seeking or attacking the player, try to find him and start to chase him if in range.
                if (ai.States.Peek() == AIStates.Resting || ai.States.Peek() == AIStates.Patrolling)
                {
                    var nearbyEntities = World.CurrentLevel.GetEntitesInRadius(spatial.Position, 5);

                    if (nearbyEntities.Any())
                    {
                        foreach (var nearbyEntity in nearbyEntities)
                        {
                            if (nearbyEntity == entity) continue;
                            
                            var collsison = World.CollisionComponents.SingleOrDefault(x => x.Owner == nearbyEntity);
                            if (collsison.Group == CollisionGroup.Player)
                            {
                                var position = World.SpatialComponents.Single(x => x.Owner == nearbyEntity).Position;
                                ai.Goal = position;
                                ai.EntityOfInterest = nearbyEntity;
                                ai.States.Push(AIStates.Seeking);
                            }
                        }
                    }
                }

                switch (ai.States.Peek())
                {
                    case AIStates.Resting:
                        if (Dice.Roll("1d100") < 25)
                            ai.States.Push(AIStates.Patrolling);
                        break;
                    case AIStates.Patrolling:
                        if (!ai.Goal.HasValue)
                        {
                            var currentCell = World.CurrentLevel.GetTile(spatial.Position).Cell;
                            var cell = World.CurrentLevel.GetRandomEmptyTile().Cell;
                            ai.Goal = new Vector2f(cell.X, cell.Y);
                            ai.CurrentPath = World.CurrentLevel.PathFinder.ShortestPath(currentCell, cell);
                        }

                        if (ai.Goal == new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y))
                        {
                            ai.Goal = null;
                            ai.States.Pop();
                            break;
                        }
                        var moveChance = Dice.Roll("1d100");
                        if (moveChance <= ai.MoveChance)
                        {
                            var physics = World.PhysicsComponents.Single(x => x.Owner == entity);
                            var nextStep = new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y);
                            var velocity = nextStep - spatial.Position;
                            ai.CurrentPath.StepForward();
                            physics.Velocity = velocity;
                        }
                        break;
                    case AIStates.Seeking:
                        if (ai.EntityOfInterest == null)
                        {
                            ai.States.Pop();
                            break;
                        }
                        var thisCell = World.CurrentLevel.GetTile(spatial.Position).Cell;
                        var entityTile = World.CurrentLevel.GetTile(World.SpatialComponents[(int)ai.EntityOfInterest].Position);

                        ai.Goal = new Vector2f(entityTile.Cell.X, entityTile.Cell.Y);
                        ai.CurrentPath = World.CurrentLevel.PathFinder.ShortestPath(thisCell, entityTile.Cell);

                        if (ai.Goal == new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y))
                        {
                            ai.Goal = null;
                            ai.States.Pop();
                            FoundEntity(ai);
                            break;
                        }

                        var thisPhysics = World.PhysicsComponents.Single(x => x.Owner == entity);
                        var nextTile = new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y);
                        var thisVelocity = nextTile - spatial.Position;
                        ai.CurrentPath.StepForward();
                        thisPhysics.Velocity = thisVelocity;
                        break;
                }

                ai.TurnDelay = Dice.Roll("1d10");
            }
            base.Update(dt);
        }

        private void FoundEntity(EnemyAIComponent ai)
        {
            ai.States.Push(AIStates.Attacking);
        }
    }
}
