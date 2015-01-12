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
            attackSpeed = 0.7f;
            projectileSpeed = 3f;
            damage = 300f;
            range = 150f;
            splashRadius = 35f;
            cost = 500;
            upgradeCost = 300;

            splashColor = Color.Brown;

            towerDescription = "Fires a piercing \nshot that explodes \nwhen destroyed";
        }

        public override void Upgrade()
        {
            base.Upgrade();
            damage += 150 * (rank+1);
            splashRadius += 1;
            range += 6;
            attackSpeed += 0.05f;
            projectileSpeed += 0.1f;
            if (rank == 4)
                upgradeCost = (int)(upgradeCost*1.5f);
            if (rank == 5)
            {
                damage += damage;
                splashRadius += 10;
                upgradeCost -= 1000;
            }
            upgradeCost = (int)(upgradeCost + 350 + rank * 50);
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
            Boulder b;
            float lifeTime = 10;
            projectiles.Add(b = new Boulder(Game1.spriteSheet, pos, SpriteRegions.Boulder, target, null, projectileSpeed, damage, splashRadius, lifeTime, 1.5f, Color.White, false));

            if(rank >= 10)
            {
                Vector2 dir = Vector2.Transform(b.dir,Matrix.CreateRotationZ(MathHelper.ToRadians(15)));
                projectiles.Add(new Boulder(Game1.spriteSheet, pos, SpriteRegions.Boulder, null, dir, projectileSpeed, damage / 2, splashRadius / 2, lifeTime, 0.9f, Color.White, false));
                dir = Vector2.Transform(b.dir, Matrix.CreateRotationZ(MathHelper.ToRadians(-15)));
                projectiles.Add(new Boulder(Game1.spriteSheet, pos, SpriteRegions.Boulder, null, dir, projectileSpeed, damage / 2, splashRadius / 2, lifeTime, 0.9f, Color.White, false));
            }

            Game1.soundManager.PlaySound("se_enep00");
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "Splash radius " + Math.Round(splashRadius, 1).ToString(), new Vector2(windowX - 150, 90), Color.White);
        }
    }
}
