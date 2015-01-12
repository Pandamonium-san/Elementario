using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elementario
{
    public class Grid
    {
        public const int nodeSize = 24;
        public static int offsetX = 48, offsetY = 48;

        public Texture2D tex;
        public Rectangle gridRec;

        public Node[,] nodes;

        public Grid()
        {
            this.tex = Game1.colorTexture;
            this.gridRec = new Rectangle(offsetX,offsetY,840,600);
            CreateNodes();
        }

        private void CreateNodes()
        {
            nodes = new Node[35, 25];
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    nodes[i, j] = new Node(i * nodeSize + gridRec.X, j * nodeSize + gridRec.Y, nodeSize, nodeSize);
                }
            }

            foreach (Node n in nodes)
                FindNeighbors(n);
        }
        
        private void FindNeighbors(Node n)
        {
            n.neighbors = new List<Node>();

            if (n.X + 1 < nodes.GetLength(0))
                n.neighbors.Add(nodes[n.X + 1, n.Y]);

            if (n.X - 1 >= 0)
                n.neighbors.Add(nodes[n.X - 1, n.Y]);

            if (n.Y + 1 < nodes.GetLength(1))
                n.neighbors.Add(nodes[n.X, n.Y + 1]);

            if (n.Y - 1 >= 0)
                n.neighbors.Add(nodes[n.X, n.Y - 1]);
        }

        public void UpdateNodes()
        {
            foreach (Node n in nodes)
            {
                n.blocked = false;
                foreach (Tower t in Game1.towerManager.towers)
                {
                    if (n.hitbox.Intersects(t.hitbox))
                        n.blocked = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, gridRec, new Color(20,50,20));

            if(Game1.showNodes)
            foreach (var n in nodes)  //Draw nodes
            {
                if (n.closed && n.parent != null)
                {
                    spriteBatch.Draw(Game1.colorTexture, n.hitbox, Color.Black);
                }
                if (n.open)
                    spriteBatch.Draw(Game1.colorTexture, n.hitbox, Color.White);
                if (PathFinder.currentNode != null)
                    spriteBatch.Draw(Game1.colorTexture, PathFinder.currentNode.hitbox, Microsoft.Xna.Framework.Color.Red);
            }
        }
    }
}
