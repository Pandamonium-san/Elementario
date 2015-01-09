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
        public float radius, speed, damage, lifeTime;
        public float slowAmount, slowDuration, splashRadius;
        public int splashLimit = 5, splashedTargets;
        public Vector2 dir;
        public Enemy target;

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

        public Projectile(Texture2D tex, Vector2 pos, Rectangle spriteRec, Enemy target, float speed, float damage, float splashRadius, float slowAmount, float slowDuration, float lifeTime, Color color)
            :base(tex, pos, spriteRec)
        {
            this.target = target;
            this.speed = speed;
            this.damage = damage;
            this.splashRadius = splashRadius;
            this.slowAmount = slowAmount;
            this.slowDuration = slowDuration;
            this.dir = target.pos - pos;
            this.color = color;
            dir.Normalize();

            radius = 6;
            this.lifeTime = lifeTime;
        }
        public Projectile(Texture2D tex, Vector2 pos, Rectangle spriteRec, Vector2 dir, float speed, float damage, float splashRadius, float lifeTime, Color color)
            : base(tex, pos, spriteRec)
        {
            this.speed = speed;
            this.damage = damage;
            this.splashRadius = splashRadius;
            this.dir = dir;
            this.color = color;
            dir.Normalize();

            radius = 6;
            this.lifeTime = lifeTime;
        }

        public virtual void CollidedWithEnemy(Enemy e)
        {
            if(lifeTime > 0)
            e.TakeDamage(this, damage);
            lifeTime = 0;
        }

        public void SplashedEnemy(Enemy e, float damage)
        {
            e.TakeDamage(this, damage);
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
                rotationAngle = -0.15f;
            else if (crossProductSign < 0)
                rotationAngle = 0.15f;

            dir = Vector2.Transform(dir, Matrix.CreateRotationZ(rotationAngle));
            dir.Normalize();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (target != null && !target.dead)
            UpdateDirection();
            pos += speed * dir * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
