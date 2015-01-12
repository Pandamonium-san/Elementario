using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    class FastEnemy:Enemy
    {
        public FastEnemy(Texture2D tex, Rectangle spriteRec, Node startNode, Node destination, int level)
            : base(tex, spriteRec, startNode, destination, level)
        {
            speed *= 1.5f;
            if (speed > 15f)
                speed = 15f;
        }
    }

    class SmallEnemy : Enemy
    {
        public SmallEnemy(Texture2D tex, Rectangle spriteRec, Node startNode, Node destination, int level)
            : base(tex, spriteRec, startNode, destination, level)
        {
            maxHealth *= 0.6f;
            currentHealth = maxHealth;
            bounty = (int)(bounty*0.5);

            radius = 8;
        }
    }

    class BigEnemy : Enemy
    {
        public BigEnemy(Texture2D tex, Rectangle spriteRec, Node startNode, Node destination, int level)
            : base(tex, spriteRec, startNode, destination, level)
        {
            maxHealth *= 4f;
            currentHealth = maxHealth;
            bounty *= 3;
            speed *= 0.7f;

            if (speed > 7f)
                speed = 7f;
        }
    }

    class BossEnemy : Enemy
    {
        float animationTime, animationInterval;
        bool rightFoot;
        public BossEnemy(Texture2D tex, Rectangle spriteRec, Node startNode, Node destination, int level)
            : base(tex, spriteRec, startNode, destination, level)
        {
            maxHealth *= 25f;
            currentHealth = maxHealth;
            bounty *= 30;
            speed *= 0.5f;
            if (speed > 7f)
                speed = 7f;

            rightFoot = false;
            animationInterval = 200 / speed;
            animationTime = animationInterval;
        }

        private void Animate(GameTime gameTime)
        {
            animationTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationTime <= 0)
            {
                animationTime = animationInterval;
                rightFoot = !rightFoot;
                if (rightFoot)
                    spriteRec.Y += 24;
                else
                    spriteRec.Y -= 24;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Animate(gameTime);
            base.Update(gameTime);
        }

    }
}
