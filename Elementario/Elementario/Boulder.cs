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

        public Boulder(Texture2D tex, Vector2 pos, Rectangle spriteRec, Enemy target, float speed, float damage, float splashRadius, float lifeTime, float scale, Color color)
            :base(tex, pos, spriteRec, target, speed, damage, splashRadius, 0f, 0f, lifeTime, color)
        {
            this.scale = scale;
            radius = 12;
        }

        public override void CollidedWithEnemy(Enemy e)
        {
            if (e.currentHealth > damage)
                base.CollidedWithEnemy(e);
            else
            {
                float enemyHealth = e.currentHealth;
                e.TakeDamage(this, damage);
                this.damage -= enemyHealth*0.75f;
            }
        }

        protected override void UpdateDirection()
        {
        }
    }
}
