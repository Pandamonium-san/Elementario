using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class Projectile:Sprite
    {
        public float radius, speed, damage, lifeTime, turnSpeed;
        public float slowAmount, slowDuration, splashRadius;
        public int splashLimit = 5, splashedTargets;
        public Vector2 dir;
        public Enemy target;

        public bool targetOnly;

        public bool Slows
        {
            get
            {
                if (slowAmount > 0 && slowDuration > 0)
                    return true;
                else
                    return false;
            }
        }

        public Projectile(Texture2D tex, Vector2 pos, Rectangle spriteRec, Enemy target, Vector2? dir, float speed, float damage, float splashRadius, float slowAmount, float slowDuration, float lifeTime, Color color, bool targetOnly)
            :base(tex, pos, spriteRec)
        {
            this.target = target;
            this.speed = speed;
            this.damage = damage;
            this.splashRadius = splashRadius;
            this.slowAmount = slowAmount;
            this.slowDuration = slowDuration;
            this.color = color;
            this.targetOnly = targetOnly;

            this.turnSpeed = 0.15f;

            if (dir == null)
                this.dir = target.pos - pos;
            else
                this.dir = (Vector2)dir;
            this.dir.Normalize();

            rotation = (float)(Math.Atan2(this.dir.Y, this.dir.X)) + MathHelper.ToRadians(135);
            radius = 6;
            this.lifeTime = lifeTime;
        }

        public virtual void CollidedWithEnemy(Enemy e)
        {
            if(lifeTime > 0 && !e.dead)
            e.TakeDamage(this, damage);
            lifeTime = 0;
        }

        public void Splash(Projectile p, List<Enemy> enemies, float damage, Color splashColor)
        {
            if (p.splashRadius > 0)
            {
                Game1.particleEngine.CreateExplosion(p.pos, p.splashRadius, 300f, splashColor * 0.7f);
                Game1.soundManager.PlaySound("se_damage00");
                foreach (Enemy e in enemies)
                {
                    if (p.splashedTargets > p.splashLimit)
                        break;
                    if ((e.pos - p.pos).Length() <= p.splashRadius)
                    {
                        e.TakeDamage(this, damage/4);
                        ++p.splashedTargets;
                    }
                }
            }
        }

        public void UpdateTarget(Enemy e)
        {
            if(e != null)
            target = e;
        }

        protected virtual void UpdateDirection()
        {
            Vector2 targetDir = target.pos - pos;
            Vector3 targetDir3 = new Vector3(targetDir, 0);
            Vector3 dir3 = new Vector3(dir, 0);

            float crossProductSign = Vector3.Cross(targetDir3, dir3).Z;

            float rotationAngle = 0;
            if (crossProductSign > 0)
                rotationAngle = -turnSpeed;
            else if (crossProductSign < 0)
                rotationAngle = turnSpeed;

            dir = Vector2.Transform(dir, Matrix.CreateRotationZ(rotationAngle));
            dir.Normalize();

            rotation = (float)(Math.Atan2(dir.Y, dir.X)) + MathHelper.ToRadians(135);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (target != null && !target.dead)
                UpdateDirection();
            else
                targetOnly = false;
            pos += speed * dir * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
