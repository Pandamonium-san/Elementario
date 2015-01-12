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
        int nrOfProjectiles = 1;

        public FireTower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Fire Tower";
            projectiles = new List<Projectile>();
            attackSpeed = 3f;
            projectileSpeed = 3f;
            damage = 80f;
            range = 70f;
            splashRadius = 50f;
            cost = 150;
            upgradeCost = 150;

            splashColor = Color.Orange*0.5f;

            towerDescription = "Rapidly shoots \nexploding fireballs \nin random directions";
        }

        public override void Upgrade()
        {
            base.Upgrade();
            damage += (rank+1)*15;
            range += 3;
            splashRadius += 0.4f;
            attackSpeed += 0.3f;
            if (rank == 15)
                ++nrOfProjectiles;
            if (rank == 30)
            {
                nrOfProjectiles += 2;
                splashColor = Color.Orange * 0.1f;
            }
            if (rank > 5)
                damage += 5 * rank;

            upgradeCost = (int)(upgradeCost + 200 + rank * 50);
        }


        protected override void Shoot()
        {
            for (int i = 0; i < nrOfProjectiles; i++)
            {
                float lifeTime = ((range / projectileSpeed) / 60) * (float)Game1.rnd.NextDouble();
                Vector2 dir = new Vector2(Game1.rnd.Next(-20, 20) / 20f, Game1.rnd.Next(-20, 20) / 20f);
                dir.Normalize();
                projectiles.Add(new Projectile(Game1.spriteSheet, pos, SpriteRegions.FireBullet, null, dir, projectileSpeed, damage, splashRadius, slowAmount, slowDuration, lifeTime, Color.White, false));
            }

            Game1.soundManager.PlaySound("se_damage01");
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "Splash radius " + splashRadius.ToString(), new Vector2(windowX - 150, 90), Color.White);
        }
    }
}
