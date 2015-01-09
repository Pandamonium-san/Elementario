using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class Node  //Used for pathfinding
    {
        public Vector2 pos;
        public Rectangle hitbox;

        public bool blocked;
        public List<Node> neighbors;
        public Node parent;
        public bool closed, open;
        public float G, H;
        public float F
        {
            get { return G + H; }
        }

        public int X
        {
            get { return (int)(pos.X - Grid.offsetX) / Grid.nodeSize; }
        }
        public int Y 
        {
            get { return (int)(pos.Y - Grid.offsetY) / Grid.nodeSize; }
        }

        int offset = 5;

        public Node(int x, int y, int width, int height)
        {
            this.hitbox = new Rectangle(x + offset, y + offset, width - offset*2, height - offset*2);
            pos = new Vector2(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height / 2);
            blocked = false;
        }

        public void ResetValues()
        {
            closed = false;
            open = false;
            G = 0;
            H = 0;
            parent = null;
        }

    }
}
