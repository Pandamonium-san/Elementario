using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class Enemy:Sprite
    {
        public List<Node> path;
        public Node destination, movingToNode;

        public Vector2 velocity, dir;
        public float speed, radius, currentHealth, maxHealth;
        public float slowAmount, slowDuration;
        public int bounty, level;
        int x, y;

        public bool moving, slowed;
        float moveTime;
        public int step;

        public bool dead = false;

        public float Speed
        {
            get
            {
                return speed * (1 - slowAmount);
            }
        }

        public Node CurrentNode
        {
            get
            {
                x = (int)(pos.X - Grid.offsetX) / Grid.nodeSize;
                y = (int)(pos.Y - Grid.offsetY) / Grid.nodeSize;
                return Game1.grid.nodes[x, y];
            }
        }

        public Enemy(Texture2D tex, Rectangle spriteRec, Node startNode, Node destination, int level)
            : base(tex, startNode.pos, spriteRec)
        {
            this.destination = destination;

            maxHealth = (200f + 10 * level) * (1 + (float)level / 10f);
            currentHealth = maxHealth;
            bounty = (int)(10 + 1 * level);

            speed = 1f + (float)level/50f;
            if (speed > 10f)
                speed = 10f;

            radius = 10f;

            moving = false;
            step = 0;
            UpdatePath();
        }

        public void Update(GameTime gameTime)
        {
            Movement(gameTime);
            if (!moving)
            {
                if (path == null || step < 0)
                    return;
                MoveToNode(path[step]);
            }
        }


        private void MoveToNode(Node n)
        {
            --step;                 //Pathfinder returns path in reverse order
            if (CurrentNode == n)
                return;
            moving = true;
            movingToNode = n;
            UpdateVelocity();
        }

        private void Movement(GameTime gameTime)
        {
            if (slowed)
            {
                color = new Color(105, 105, 255);
                slowDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (slowDuration <= 0)
                {
                    slowed = false;
                    color = Color.White;
                    slowAmount = 0;
                    UpdateVelocity();
                }
            }
            if (moving)
            {
                pos += velocity * 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                moveTime -= 60f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (moveTime <= 0)
                {
                    moving = false;
                }
            }
        }

        private void Slowed(Projectile p)
        {
            if(slowAmount < p.slowAmount)
            slowAmount = p.slowAmount;
            slowDuration = p.slowDuration;
            slowed = true;
            UpdateVelocity();
        }

        private void UpdateVelocity()
        {
            dir = (movingToNode.pos - pos);
            moveTime = dir.Length() / Speed;
            dir.Normalize();
            velocity = dir * Speed;
        }

        public void TakeDamage(Projectile p, float damage)
        {
            currentHealth -= damage;
            if (p.slowAmount > 0)
                Slowed(p);
            if (currentHealth <= 0)
                dead = true;

            if (dead)
                for (int i = 0; i < 50; i++)
                    GenerateDeathParticle();
        }

        protected virtual Color DeathColor
        {
            get
            {
                return new Color(255, Game1.rnd.Next(0, 250), Game1.rnd.Next(0, 50));
            }
        }

        protected virtual void GenerateDeathParticle()
        {
            Vector2 particleVelocity = new Vector2(Game1.rnd.Next(-20, 20) / 20f, Game1.rnd.Next(-20, 20) / 20f);
            particleVelocity.Normalize();
            particleVelocity *= Game1.rnd.Next(20)/20f;
            float lifeTime = lifeTime = 400 + Game1.rnd.Next(800);
            float size = 3f;
            Game1.particleEngine.CreateParticle(Game1.colorTexture, pos, new Rectangle(0,0,1,1), particleVelocity, DeathColor, size, lifeTime);
        }

        public void UpdatePath()
        {
            path = PathFinder.FindPath(CurrentNode, destination);
            if (path == null)
                return;
            step = path.Count() - 1;
        }

        public bool DestinationReached()
        {
            if (CurrentNode == destination)
                return true;
            return false;
        }

        private void DrawHealthBar(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.colorTexture, new Rectangle((int)(pos.X - origin.X / 2) - 1 - 1, (int)pos.Y + 10 - 1, 16, 5), Color.Black);
            spriteBatch.Draw(Game1.colorTexture, new Rectangle((int)(pos.X - origin.X / 2) - 1, (int)pos.Y + 10, 14, 3), Color.Red);
            spriteBatch.Draw(Game1.colorTexture, new Rectangle((int)(pos.X - origin.X / 2) - 1, (int)pos.Y + 10, (int)(14 * currentHealth / maxHealth), 3), Color.Green);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            DrawHealthBar(spriteBatch);
        }
    }
}
