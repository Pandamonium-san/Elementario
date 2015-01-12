using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    class WindTower : Tower
    {
        public int projectilesFired;

        public WindTower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Wind Tower";
            projectiles = new List<Projectile>();
            attackSpeed = 1.2f;
            projectilesFired = 2;
            projectileSpeed = 7f;
            damage = 25f;
            range = 150f;
            cost = 100;
            upgradeCost = 100;

            towerDescription = "Once upgraded, \nshoots bursts of \nhoming projectiles at \nlong range";
        }

        protected override void UpdateProjectiles(GameTime gameTime)
        {
            foreach (Projectile p in projectiles)
            {
                if (p.target.dead || p.target == null)
                    p.UpdateTarget(target);
            }
            base.UpdateProjectiles(gameTime);
        }

        public override void Upgrade()
        {
            base.Upgrade();
            attackSpeed += 0.05f;
            damage += 5 * rank;
            range += 7;
            projectileSpeed += 0.05f;
            if (projectilesFired >= 9)
                damage += 10 * rank;
            if (projectilesFired < 9)
                ++projectilesFired;
            if (rank == 20)
            {
                slowAmount = 0.6f;
            }

            upgradeCost = (int)(upgradeCost + rank * 75);
        }

        protected override void Shoot()
        {
            int projectilesAlreadyFired = 0;
            float lifeTime = 3;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (projectilesAlreadyFired >= projectilesFired)
                        break;
                    projectiles.Add(new Projectile(Game1.spriteSheet, pos, SpriteRegions.WindBullet, null, new Vector2(i, j), projectileSpeed, damage, splashRadius, slowAmount, slowDuration, lifeTime, Color.White, false));
                    ++projectilesAlreadyFired;
                }
            }

            foreach (Projectile p in projectiles)
                p.UpdateTarget(target);

            Game1.soundManager.PlaySound("se_tan00");
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "# of projectiles " + projectilesFired.ToString(), new Vector2(windowX - 150, 90), Color.White);
        }
    }
}
