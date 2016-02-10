using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
using RogueSharp.DiceNotation;
using SFML.System;

namespace Helios.LikeARogue.Components
{
    public enum AIStates
    {
        Resting,
        Patrolling,
        Seeking,
        Attacking,
        Retreating
    }

    public class EnemyAIComponent : Component
    {
        uint? SeekingEntity { get; set; }
        public Stack<AIStates> States { get; set; }
        public Vector2f? Goal { get; set; }
        public uint? EntityOfInterest { get; set; }
        public int MoveChance { get; set; }
        public float ElapsedTimeSinceLastMove { get; set; }
        public Path CurrentPath { get; set; }
        public int TurnDelay { get; set; }
        public EnemyAIComponent()
        {
            States = new Stack<AIStates>();
            States.Push(AIStates.Resting);
            TurnDelay = Dice.Roll("1d10");
        }
    }
}
