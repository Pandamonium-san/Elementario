using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class Tower : Sprite
    {
        public string name, towerDescription = "";
        public int cost, upgradeCost, totalCost, rank;
        public float range;
        public Rectangle hitbox
        {
            get
            {
                return new Rectangle((int)(pos.X - origin.X),
                    (int)(pos.Y - origin.Y),
                    spriteRec.Width,
                    spriteRec.Height);
            }
        }
        public List<Projectile> projectiles;
        public float projectileSpeed, damage, attackSpeed, cooldown;
        public float splashRadius;
        public float slowAmount, slowDuration;
        protected Color splashColor = Color.White;
        protected Enemy target;

        public Tower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            projectiles = new List<Projectile>();
            cooldown = 0f;
        }

        public virtual void Upgrade()
        {
            ++rank;
            totalCost += upgradeCost;
        }

        public virtual void Update(GameTime gameTime)
        {
            cooldown -= attackSpeed * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            target = AcquireTarget();
            UpdateProjectiles(gameTime);
            if (cooldown <= 0 && target != null)
            {
                Shoot();
                cooldown = 60 * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        protected virtual void UpdateProjectiles(GameTime gameTime)
        {
            for (int i = 0; i < projectiles.Count(); i++)
            {
                projectiles[i].Update(gameTime);
                ProjectileCollision(projectiles[i], Game1.enemyManager.enemies);
                if (projectiles[i].lifeTime <= 0 || !Game1.windowRec.Contains((int)projectiles[i].pos.X, (int)projectiles[i].pos.Y))
                {
                    projectiles.Remove(projectiles[i]);
                    --i;
                }
            }
        }

        public void ProjectileCollision(Projectile p, List<Enemy> enemies)
        {
            if (p.targetOnly)
                CollidesWithTarget(p);
            else
                CollidesWithFirstEnemy(p, enemies);

            if (p.splashRadius > 0)
                if (p.lifeTime <= 0)
                    p.Splash(p, enemies, damage, splashColor);
        }

        protected void CollidesWithTarget(Projectile p)
        {
            if (p.target == null)
            {
                p.lifeTime = 0;
                return;
            }
            if ((p.target.pos - p.pos).Length() <= p.target.radius + p.radius)
            {
                p.CollidedWithEnemy(p.target);
            }
        }

        protected void CollidesWithFirstEnemy(Projectile p, List<Enemy> enemies)
        {
            foreach (Enemy e in enemies)
            {
                if ((e.pos - p.pos).Length() < e.radius + p.radius)
                    p.CollidedWithEnemy(e);
            }
        }

        public virtual Enemy AcquireTarget()
        {
            foreach (Enemy e in Game1.enemyManager.enemies)
            {
                if ((e.pos - pos).Length() <= range)
                    return e;
            }
            return null;
        }

        protected virtual void Shoot()
        {
        }

        public virtual void DrawTowerInfo(SpriteBatch spriteBatch, int windowX, int windowY)
        {
            spriteBatch.DrawString(Game1.font3, name, new Vector2(windowX - 150, 10), Color.White);
            spriteBatch.DrawString(Game1.font3, "Rank " + rank.ToString(), new Vector2(windowX - 150, 30), Color.White);
            spriteBatch.DrawString(Game1.font3, "Damage " + Math.Round(damage, 1).ToString(), new Vector2(windowX - 150, 50), Color.White);
            spriteBatch.DrawString(Game1.font3, "Fire Rate " + Math.Round(attackSpeed, 1).ToString(), new Vector2(windowX - 150, 70), Color.White);
            spriteBatch.DrawString(Game1.font3, towerDescription, new Vector2(windowX - 150, 155), Color.White);

            if (this == Game1.towerManager.ghostTower)
                spriteBatch.DrawString(Game1.font3, "Cost " + cost.ToString(), new Vector2(windowX - 150, 130), Color.White);
            else
                spriteBatch.DrawString(Game1.font3, "Value " + totalCost.ToString(), new Vector2(windowX - 150, 130), Color.White);
        }

        public virtual void DrawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (var p in projectiles)
                p.Draw(spriteBatch);
        }
    }
}
