using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    class WaterTower:Tower
    {
        List<Enemy> targets;
        public int maxTargets;

        public WaterTower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Water Tower";
            projectiles = new List<Projectile>();
            targets = new List<Enemy>();
            maxTargets = 3;
    
            damage = 10f;
            attackSpeed = 1.5f;
            range = 100f;
            slowAmount = 0.2f;
            slowDuration = 1f;

            cost = 100;
            upgradeCost = 100;

            splashColor = Color.CornflowerBlue;
            projectileSpeed = 5f;

            towerDescription = "Targets multiple \nenemies at once and \nslows them";
        }

        public override void Upgrade()
        {
            base.Upgrade();
            damage += 5 + rank * 10f;
            attackSpeed += 0.05f;
            range += 5;
            slowAmount += 0.05f;
            projectileSpeed += 0.07f;
            if (slowAmount > 0.8f)
            {
                damage += 15 * rank;
                slowAmount = 0.8f;
            }
            if(rank%2==0)
                ++maxTargets;
            if(rank == 20)
            {
                splashRadius += 30f;
                slowDuration += 1f;
            }

            upgradeCost = (int)(upgradeCost + 200 + rank * 90);
        }

        private Enemy FirstPlaceEnemy(List<Enemy> enemies)
        {
            Enemy inFirst = null;
            for ( int j = 0; j < enemies.Count(); j++)
            {
                if (inFirst == null || (enemies[j].step < inFirst.step))
                {
                    inFirst = enemies[j];
                }
            }
            return inFirst;
        }

        public override Enemy AcquireTarget()   //Prioritizes the non-slowed enemy closest to the goal
        {
            List<Enemy> inRangeSlowed = new List<Enemy>();
            List<Enemy> inRangeNotSlowed = new List<Enemy>();
            targets = new List<Enemy>();

            foreach (Enemy e in Game1.enemyManager.enemies)
            {
                if (targets.Count >= maxTargets)
                    return targets[0];
                if ((e.pos - pos).Length() <= range)
                {
                    if (e.slowed)
                        inRangeSlowed.Add(e);
                    else if (!e.slowed)
                        inRangeNotSlowed.Add(e);
                }
            }

            for (int i = 0; i < maxTargets; i++)
            {
                Enemy inFirst = FirstPlaceEnemy(inRangeNotSlowed);
                if (inFirst != null)
                {
                    targets.Add(inFirst);
                    inRangeNotSlowed.Remove(inFirst);
                }
            }
            if(targets.Count() < maxTargets)
                for (int i = 0; i < maxTargets - targets.Count(); i++)
                {
                    Enemy inFirst = FirstPlaceEnemy(inRangeSlowed);
                    if (inFirst != null)
                    {
                        targets.Add(inFirst);
                        inRangeSlowed.Remove(inFirst);
                    }
                }

            if (targets.Count > 0)
                return targets[0];
            return null;
        }

        protected override void Shoot()
        {
            float lifeTime = 10;
            foreach (Enemy e in targets)
            {
                projectiles.Add(new Projectile(Game1.spriteSheet, pos, SpriteRegions.Bullet, e, null, projectileSpeed, damage, splashRadius, slowAmount, slowDuration, lifeTime, Color.CornflowerBlue, true));
            }
        }

        public override void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            base.DrawTowerInfo(spriteBatch, windowX, windowY);
            spriteBatch.DrawString(Game1.font3, "Targets " + maxTargets.ToString(), new Vector2(windowX - 150, 90), Color.White);
            spriteBatch.DrawString(Game1.font3, "Slow% " + Math.Round((100 * slowAmount), 0).ToString(), new Vector2(windowX - 150, 110), Color.White);
        }
    }
}
