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
        public int cost, totalCost, rank;
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
        protected Enemy target;

        public Tower(Texture2D tex, Vector2 pos, Rectangle spriteRec)
            : base(tex, pos, spriteRec)
        {
            name = "Wall Tower";
            projectiles = new List<Projectile>();
            attackSpeed = 1f;
            projectileSpeed = 4f;
            damage = 5f;
            range = 100f;
            cost = 10;

            cooldown = 0f;
            towerDescription = "Cheap tower used \nmainly for pathing";
        }

        public virtual void Upgrade()
        {
            ++rank;
            totalCost += cost;
            attackSpeed += 0.1f;
            damage += 3+1f*rank;
            range += 4;

            cost = (int)(cost + 20);
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
                if (projectiles[i].lifeTime <= 0)
                {
                    projectiles.Remove(projectiles[i]);
                    --i;
                }
            }
        }

        public virtual void ProjectileCollision(Projectile p, List<Enemy> enemies)
        {
            foreach (Enemy e in enemies)
            {
                if ((e.pos - p.pos).Length() <= e.radius + p.radius)
                {
                    p.CollidedWithEnemy(e);
                }
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

        public void Splash(Projectile p, List<Enemy> enemies, float damage, Color splashColor)
        {
            if (p.splashRadius > 0)
            {
                Game1.particleEngine.CreateExplosion(p.pos, p.splashRadius, 300f, splashColor * 0.7f);
                foreach (Enemy E in enemies)
                    if ((E.pos - p.pos).Length() <= p.splashRadius)
                    {
                        if (p.splashedTargets > p.splashLimit)
                            break;
                        p.SplashedEnemy(E, damage);
                        ++p.splashedTargets;
                    }
            }
        }
        protected virtual void Shoot()
        {
            projectiles.Add(new Projectile(Game1.spriteSheet, pos, new Rectangle(0, 84, 12, 12), target, projectileSpeed, damage, 0f, 0f, 0f, 10f, Color.Gray));
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
