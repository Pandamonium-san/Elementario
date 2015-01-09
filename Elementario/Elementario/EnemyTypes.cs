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

        protected override Color DeathColor
        {
            get
            {
                return new Color(0, 255, Game1.rnd.Next(0,200));
            }
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

        protected override Color DeathColor
        {
            get
            {
                return new Color(0, Game1.rnd.Next(0, 200), 255);
            }
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

        protected override Color DeathColor
        {
            get
            {
                return new Color(Game1.rnd.Next(100,200), Game1.rnd.Next(50,100),Game1.rnd.Next(0,80));
            }
        }
    }

    class BossEnemy : Enemy
    {
        public BossEnemy(Texture2D tex, Rectangle spriteRec, Node startNode, Node destination, int level)
            : base(tex, spriteRec, startNode, destination, level)
        {
            maxHealth *= 25f;
            currentHealth = maxHealth;
            bounty *= 30;
            speed *= 0.5f;
            if (speed > 7f)
                speed = 7f;
        }

        protected override Color DeathColor
        {
            get
            {
                return new Color(Game1.rnd.Next(0, 255), Game1.rnd.Next(0, 255), Game1.rnd.Next(0, 255));
            }
        }
    }
}
