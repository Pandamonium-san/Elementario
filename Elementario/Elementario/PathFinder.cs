using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elementario
{
    public static class PathFinder
    {
        public static Node[,] g = Game1.grid.nodes;
        public static List<Node> openList;
        public static Node currentNode;
        public static Node targetNode;

        public static int I = 875;

        static float G;
        

        public static List<Node> FindPath(Node startNode, Node destinationNode)
        {
            Node[,] g = Game1.grid.nodes;
            openList = new List<Node>();
            foreach (Node n in g)
                n.ResetValues();

            targetNode = destinationNode;
            currentNode = startNode;
            currentNode.open = true;

            for (int i = 0; i < I; i++)
            {
                SearchStep();
                if (currentNode == destinationNode)
                    return Path(startNode, destinationNode);
                if (openList.Count <= 0)
                    return null;
            }
            return null;
        }

        public static void SearchStep()
        {
            AddAdjacentOpenNodes(currentNode);
            currentNode = FindBestNode();
        }

        public static Node FindBestNode()
        {
            Node lowestFNode = currentNode;
            float lowestF = -1;

            foreach (Node n in openList)
            {
                if(n.open)
                if (lowestF < 0 || n.F < lowestF)
                {
                    lowestF = n.F;
                    lowestFNode = n;
                }
            }
            return lowestFNode;
        }

        public static void AddAdjacentOpenNodes(Node n)
        {
            foreach (Node neighbor in n.neighbors)
            {
                if (neighbor.blocked)
                    continue;

                G = n.G + 1;

                if ((!neighbor.closed && !neighbor.open) || neighbor.G >= G)
                {
                    neighbor.G = G;
                    neighbor.H = Heuristic(neighbor);
                    neighbor.parent = n;
                }
                if (!neighbor.closed && !neighbor.open)
                {
                    neighbor.open = true;
                    openList.Add(neighbor);
                }
            }
            CloseNode(n);
        }

        public static float Heuristic(Node n)
        {
            float dx, dy, H;

            H = Math.Abs(targetNode.X - n.X) + Math.Abs(targetNode.Y - n.Y);    //Manhattan distance

            dx = Math.Abs(targetNode.X - n.X)^2;  //tiebreaker Euclidean distance
            dy = Math.Abs(targetNode.Y - n.Y)^2;

            return H + (dx + dy) / 10000; //Avoid sqrt because it's slow
        }

        public static void CloseNode(Node n)
        {
            openList.Remove(n);
            n.open = false;
            n.closed = true;
        }

        public static List<Node> Path(Node startNode, Node destinationNode)
        {
            List<Node> path = new List<Node>();
            Node parentNode = destinationNode;
            for (int i = 0; i < g.Length; i++)
            {
                if (parentNode == startNode)
                    return path;
                path.Add(parentNode);
                parentNode = parentNode.parent;
            }
            return path;
        }
    }
}
