using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    class FireTower:Tower
    {

        public FireTower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Fire Tower";
            projectiles = new List<Projectile>();
            attackSpeed = 5f;
            projectileSpeed = 5f;
            damage = 35f;
            range = 70f;
            cost = 150;
            splashRadius = 45f;

            towerDescription = "Rapidly shoots \nexploding fireballs \nin random directions";
        }

        public override void ProjectileCollision(Projectile p, List<Enemy> enemies)
        {
            base.ProjectileCollision(p, enemies);
            if(p.lifeTime <= 0)
            Splash(p, enemies, damage/4, Color.Orange*0.7f);
        }

        protected override void Shoot()
        {
            float lifeTime = ((range/projectileSpeed)/60)*(float)Game1.rnd.NextDouble();
            Vector2 dir = new Vector2(Game1.rnd.Next(-20, 20) / 20f, Game1.rnd.Next(-20, 20) / 20f);
            dir.Normalize();
            projectiles.Add(new Projectile(Game1.spriteSheet, pos, new Rectangle(0, 84, 12, 12), dir, projectileSpeed, damage, splashRadius, lifeTime, Color.Red));
        }

        public override void Upgrade()
        {
            ++rank;
            totalCost += cost;
            damage += (rank+1)*15;
            range += 5;
            splashRadius += 2;
            attackSpeed += 1f;
            if (rank == 4)
                cost += 500;
            if (rank == 5)
            {
                attackSpeed *= 2f;
                cost -= 500;
            }
            if (rank > 5)
                damage += 5 * rank;

            cost = (int)(cost + 400);
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "Splash radius " + splashRadius.ToString(), new Vector2(windowX - 150, 90), Color.White);
        }
    }
}
