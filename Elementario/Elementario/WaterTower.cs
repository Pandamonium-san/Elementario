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
            cost = 100;

            slowAmount = 0.2f;
            slowDuration = 1f;

            projectileSpeed = 7f;

            towerDescription = "Targets multiple \nenemies at once and \nslows them";
        }

        public override void Upgrade()
        {
            ++rank;
            totalCost += cost;
            damage += rank * 10f;
            attackSpeed += 0.1f;
            range += 5;
            cost = (int)(cost + 500);

            slowAmount += 0.05f;
            if (slowAmount > 0.8f)
            {
                damage += 15 * rank;
                slowAmount = 0.8f;
            }
            if(rank%2==0)
                ++maxTargets;
        }

        public override void ProjectileCollision(Projectile p, List<Enemy> enemies)
        {
            if ((p.target.pos - p.pos).Length() <= p.target.radius + p.radius)
            {
                p.CollidedWithEnemy(p.target);
            }
        }

        private Enemy FirstPlaceEnemy(List<Enemy> enemies)
        {
            Enemy inFirst = null;
            for (int j = 0; j < enemies.Count(); j++)
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
                projectiles.Add(new Projectile(Game1.spriteSheet, pos, new Rectangle(0, 84, 12, 12), e, projectileSpeed, damage, 0f, slowAmount, slowDuration, lifeTime, Color.CornflowerBlue));
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
