using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    class WallTower:Tower
    {
        public int bounces;

        public WallTower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Wall Tower";
            projectiles = new List<Projectile>();
            attackSpeed = 1f;
            projectileSpeed = 2f;
            damage = 5f;
            range = 100f;
            bounces = 0;
            cost = 10;
            upgradeCost = 100;

            cooldown = 0f;
            towerDescription = "Used mainly for \npathing. Upgrades \ninto Blade Tower";
        }

        public override void Upgrade()
        {
            base.Upgrade();
            attackSpeed += 0.03f;
            damage += 5 + 2f * rank;
            range += 2;
            bounces = rank / 10 + 3;
            projectileSpeed += 0.02f;
            if(rank == 1)
            {
                name = "Blade Tower";
                towerDescription = "Fires a spinning \nblade that bounces \nfrom enemy to enemy";
                spriteRec = SpriteRegions.BladeTower;
                damage += 10;
                projectileSpeed = 3f;
            }
            if (rank == 50)
            {
                splashRadius = 30;
                projectileSpeed = 7f;
            }
            upgradeCost = (int)(upgradeCost + 20 + rank);
        }

        protected override void Shoot()
        {
            Color color = Color.White;
            if (rank == 0)
                color = Color.White * 0.2f;
            projectiles.Add(new RicochetP(Game1.spriteSheet, pos, SpriteRegions.Shuriken, target, projectileSpeed, damage, splashRadius, slowAmount, slowDuration, bounces, 5f, color, false));
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "Bounces " + bounces.ToString(), new Vector2(windowX - 150, 90), Color.White);
        }
    }
}
