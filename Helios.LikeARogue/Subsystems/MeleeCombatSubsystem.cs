using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Helios.LikeARogue.Components;
using RogueSharp.DiceNotation;

namespace Helios.LikeARogue.Subsystems
{
    public class MeleeCombatSubsystem : GameSubsystem
    {
        public MeleeCombatSubsystem(GameWorld theWorld) : base(theWorld)
        {
            ComponentMask.SetBit(XnaGameComponentType.Collision);
            ComponentMask.SetBit(XnaGameComponentType.Spatial);
            ComponentMask.SetBit(XnaGameComponentType.MeleeCombat);
            ComponentMask.SetBit(XnaGameComponentType.Health);
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var combat = World.MeleeCombatComponents[entity];
                var collision = World.CollisionComponents[entity];

                combat.Damage = 0;

                if (collision.Group == CollisionGroup.Enemy)
                {
                    var ai = World.EnemyAIComponents[entity];
                    if (ai.States.Peek() == AIStates.Attacking && ai.EntityOfInterest.HasValue)
                    {
                        combat.Damage = Dice.Roll("1d6");
                        if (World.HealthSubsystem.HasEntity(ai.EntityOfInterest.Value))
                        {
                            var otherHealth = World.HealthComponents[ai.EntityOfInterest.Value];
                            otherHealth.Damage += combat.Damage;
                        }
                        ai.States.Pop();
                    }
                }
                else if (collision.Group == CollisionGroup.Player && collision.CollidedWithEntity.HasValue)
                {
                    var enemy = collision.CollidedWithEntity;
                    combat.Damage = Dice.Roll("1d4");
                    if (World.HealthSubsystem.HasEntity(enemy.Value))
                    {
                        var otherHealth = World.HealthComponents[enemy.Value];
                        otherHealth.Damage += combat.Damage;
                    }
                }
            }
            base.Update(dt);
        }
    }
}
