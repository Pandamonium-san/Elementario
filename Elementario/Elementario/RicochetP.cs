using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    class RicochetP : Projectile
    {
        List<Enemy> alreadyHit;
        float bounceRange, currentDamage;
        int bounces, maxBounces;

        public RicochetP(Texture2D tex, Vector2 pos, Rectangle spriteRec, Enemy target, float speed, float damage, float splashRadius, float slowAmount, float slowDuration, int maxBounces, float lifeTime, Color color, bool targetOnly)
            : base(tex, pos, spriteRec, target, null, speed, damage, splashRadius, slowAmount, slowDuration, lifeTime, color, targetOnly)
        {
            alreadyHit = new List<Enemy>();
            currentDamage = damage;
            bounceRange = 200;
            bounces = 0;
            this.maxBounces = maxBounces;
            radius = 1;
        }

        public override void CollidedWithEnemy(Enemy e)
        {
            targetOnly = true;
            if (bounces >= maxBounces)
            {
                e.TakeDamage(this, currentDamage);
                lifeTime = 0;
            }
            else
            {
                e.TakeDamage(this, currentDamage);
                Splash(this, Game1.enemyManager.enemies, currentDamage, Color.White);
                alreadyHit.Add(e);
                if (target != null)
                    AcquireNewTarget();
                UpdateDirection();
                ++bounces;

                float bounceDamageModifier = (float)(10 - bounces) / 10f;
                if (bounceDamageModifier < 0.2)
                    bounceDamageModifier = 0.2f;
                currentDamage = damage * bounceDamageModifier;
            }
        }

        private void AcquireNewTarget()
        {
            target = TargetClosestNotHitEnemy();
            if (target == null)
            {
                alreadyHit = new List<Enemy>();
                target = TargetClosestNotHitEnemy();
            }
            Game1.soundManager.PlaySound("se_focusfix2");
        }

        private Enemy TargetClosestNotHitEnemy()
        {
            Enemy closestTarget = null;
            foreach (Enemy e in Game1.enemyManager.enemies)
            {
                if ((e.pos - pos).Length() <= bounceRange && e != target && !alreadyHit.Any(x => x == e))
                {
                    if (closestTarget == null || (e.pos - pos).Length() < (closestTarget.pos - pos).Length())
                        closestTarget = e;
                }
            }
            return closestTarget;
        }

        public override void Update(GameTime gameTime)
        {
            rotation += 0.2f;
            base.Update(gameTime);
        }

        protected override void UpdateDirection()
        {
            if (target != null)
            {
                dir = target.pos - pos;
                dir.Normalize();
            }
        }
    }
}
