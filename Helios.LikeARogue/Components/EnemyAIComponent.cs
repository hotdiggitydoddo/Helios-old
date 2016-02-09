using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueSharp;
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

    public class EnemyAIComponent
    {
        uint? SeekingEntity { get; set; }
        public Stack<AIStates> States { get; set; }
        public Vector2f? Goal { get; set; }
        public float MoveChance { get; set; }
        public float ElapsedTimeSinceLastMove { get; set; }
        public Path CurrentPath { get; set; }

        public EnemyAIComponent()
        {
            States = new Stack<AIStates>();
        }
    }
}
