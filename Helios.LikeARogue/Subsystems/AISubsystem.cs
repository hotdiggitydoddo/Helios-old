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
                var spatial = World.SpatialComponents[entity];
                var ai = World.EnemyAIComponents[entity];

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

                            var collsison = World.CollisionComponents[nearbyEntity];
                            if (collsison.Group == CollisionGroup.Player)
                            {
                                var position = World.SpatialComponents[nearbyEntity].Position;
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
                            var physics = World.PhysicsComponents[entity];
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
                        var entityPos = World.SpatialComponents[(int)ai.EntityOfInterest].Position;
                        if (entityPos != ai.Goal || ai.CurrentPath == null)
                        {
                            var thisCell = World.CurrentLevel.GetTile(spatial.Position).Cell;
                            var entityTile = World.CurrentLevel.GetTile(entityPos);

                            ai.Goal = new Vector2f(entityTile.Cell.X, entityTile.Cell.Y);
                            ai.CurrentPath = World.CurrentLevel.PathFinder.ShortestPath(thisCell, entityTile.Cell);
                        }

                        if (ai.Goal == new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y))
                        {
                            ai.Goal = null;
                            ai.States.Pop();
                            FoundEntity(ai);
                            break;
                        }

                        var thisPhysics = World.PhysicsComponents[entity];
                        var nextTile = new Vector2f(ai.CurrentPath.CurrentStep.X, ai.CurrentPath.CurrentStep.Y);
                        var thisVelocity = nextTile - spatial.Position;
                        ai.CurrentPath.StepForward();
                        thisPhysics.Velocity = thisVelocity;
                        break;

                    case AIStates.InCombat:
                        var entitySpatial = World.SpatialComponents[ai.EntityOfInterest.Value];
                        if (!InRange(ai.EntityOfInterest.Value, spatial.Position, entitySpatial.Position))
                        {
                            ai.States.Pop();
                            var position = World.SpatialComponents[ai.EntityOfInterest.Value].Position;
                            ai.Goal = position;
                            ai.States.Push(AIStates.Seeking);
                            break;
                        }
                        if (Dice.Roll("1d100") < 50)
                            ai.States.Push(AIStates.Attacking);
                        break;
                }
                ai.TurnDelay = Dice.Roll("1d10");
            }
            base.Update(dt);
        }

        private bool InRange(uint entity, Vector2f AIpos, Vector2f entityPos)
        {
            var tiles = new Tile[4]
            {
                World.CurrentLevel.GetTile(AIpos + new Vector2f(1, 0)),
                World.CurrentLevel.GetTile(AIpos + new Vector2f(-1, 0)),
                World.CurrentLevel.GetTile(AIpos + new Vector2f(0, 1)),
                World.CurrentLevel.GetTile(AIpos + new Vector2f(0, -1))
            };

            for (var i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].Entity != null && tiles[i].Entity.Value == entity)
                    return true;
            }
            return false;
        }

        private void FoundEntity(EnemyAIComponent ai)
        {
            ai.States.Push(AIStates.InCombat);
        }
    }
}
