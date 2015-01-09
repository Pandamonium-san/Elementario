using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    class EarthTower : Tower
    {

        public EarthTower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Earth Tower";
            projectiles = new List<Projectile>();
            attackSpeed = 0.4f;
            projectileSpeed = 3f;
            damage = 300f;
            range = 150f;
            splashRadius = 35f;
            cost = 500;

            towerDescription = "Fires a piercing \nshot that explodes \nwhen destroyed";
        }

        public override void Upgrade()
        {
            ++rank;
            totalCost += cost;
            damage += 100 * (rank+1);
            splashRadius += 5;
            range += 6;
            attackSpeed += 0.2f;
            projectileSpeed += 0.1f;
            if (rank == 4)
                cost = (int)(cost*1.5f);
            if (rank == 5)
            {
                damage += damage;
                splashRadius += 10;
                cost -= 1000;
            }

            cost = (int)(cost + 800);
        }

        public override void ProjectileCollision(Projectile p, List<Enemy> enemies)
        {
            base.ProjectileCollision(p, enemies);
            if (p.lifeTime <= 0)
                Splash(p, enemies, damage/2, Color.Brown);
        }

        public override Enemy AcquireTarget()
        {
            Enemy closestTarget = null;
            foreach (Enemy e in Game1.enemyManager.enemies)
            {
                if ((e.pos - pos).Length() <= range)
                {
                    if (closestTarget == null || (e.pos - pos).Length() < (closestTarget.pos - pos).Length())
                    closestTarget = e;
                }
            }
            return closestTarget;
        }
        protected override void Shoot()
        {
            float lifeTime = 10;
            projectiles.Add(new Boulder(Game1.spriteSheet, pos, new Rectangle(0, 84, 12, 12), target, projectileSpeed, damage, splashRadius, lifeTime, 2f, Color.Brown));
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "Splash radius " + Math.Round(splashRadius, 1).ToString(), new Vector2(windowX - 150, 90), Color.White);
        }
    }
}
