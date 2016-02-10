using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Helios.LikeARogue.Subsystems
{
    public class HealthSubsystem : GameSubsystem
    {
        public HealthSubsystem(GameWorld world) : base(world)
        {
            ComponentMask.SetBit(XnaGameComponentType.Health);
            ComponentMask.SetBit(XnaGameComponentType.Regeneration);
            ComponentMask.SetBit(XnaGameComponentType.Sprite);
        }

        private float lerp(float a, float b, float t)
        {
            return a * (1 - t) + b * t;
        }

        public override void Update(float dt)
        {
            foreach (var entity in RelevantEntities)
            {
                var health = World.HealthComponents[entity];
                var regen = World.RegenerationComponents[entity];
                var sprite = World.SpriteComponents[entity];


                var flammable = World.FlammableComponents[entity];

                if (regen.Frequency > 0)
                {
                    if (regen.ElapsedTime >= regen.Frequency && health.CurrentHealth != health.MaxHealth)
                    {
                        health.CurrentHealth += regen.AmountToHeal;
                        regen.ElapsedTime = 0;
                        //renderer.Messages.Add(string.Format("healed for {0} points.  Health: {1}/{2} ({3})",
                        //  regen.AmountToHeal, health.CurrentHealth, health.MaxHealth, health.IsAlive ? "ALIVE" : "DEAD"));
                        sprite.Sprite.Color = new Color(Color.Green);
                    }
                    else if (health.CurrentHealth != health.MaxHealth)
                    {
                        regen.ElapsedTime += dt;
                        var r = lerp(Color.Green.R, Color.White.R, regen.ElapsedTime);
                        var g = lerp(Color.Green.G, Color.White.G, regen.ElapsedTime);
                        var b = lerp(Color.Green.B, Color.White.B, regen.ElapsedTime);
                        sprite.Sprite.Color = new Color((byte) r, (byte) g, (byte) b);
                    }
                    else
                        sprite.Sprite.Color = Color.White;
                }

                if (flammable != null && flammable.Damage > 0)
                    health.Damage += flammable.Damage;

                if (health.Damage == 0) continue;

                health.CurrentHealth -= health.Damage;
                //renderer.Messages.Add(string.Format("damaged for {0} points.  Health: {1}/{2} ({3})",
                //            health.Damage, health.CurrentHealth, health.MaxHealth, health.IsAlive ? "ALIVE" : "DEAD"));
                health.Damage = 0;
                regen.ElapsedTime = 0f;
                sprite.Sprite.Color = Color.Red;
            }
            base.Update(dt);
        }
    }
}
