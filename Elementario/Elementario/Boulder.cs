using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    class Boulder:Projectile      
    {
        float currentDamage;

        public Boulder(Texture2D tex, Vector2 pos, Rectangle spriteRec, Enemy target, Vector2? dir, float speed, float damage, float splashRadius, float lifeTime, float scale, Color color, bool targetOnly)
            :base(tex, pos, spriteRec, target, dir, speed, damage, splashRadius, 0f, 0f, lifeTime, color, targetOnly)
        {
            currentDamage = damage;
            this.scale = scale;
            radius = 17;
        }

        public override void CollidedWithEnemy(Enemy e)
        {
            if (e.currentHealth > currentDamage)
            {
                e.TakeDamage(this, currentDamage);
                lifeTime = 0;
            }
            else
            {
                float enemyHealth = e.currentHealth;
                e.TakeDamage(this, currentDamage);
                currentDamage-= enemyHealth * 0.75f;
            }
        }

        protected override void UpdateDirection()
        {
        }
    }
}
