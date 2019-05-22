using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AI
{
    // This interface was implemented when I used a heap, which I no longer use but may switch back to
    // if the performance drops
    class Node : IHeapItem<Node>
    {
        public Vector2 Position { get; set; }
        public int GridPositionX { get; set; }
        public int GridPositionY { get; set; }
        public Node Parent { get; set; }
        public Dictionary<Node, ConnectionType> Neighbours { get; set; }

        public float GCost { get; set; }
        public float HCost { get; set; }
        public int HeapIndex { get; set; }

        public enum ConnectionType { Move, Jump }
        public ConnectionType Type { get; set; }
        public bool CanTraverse { get; set; }
        public bool IsOnGround { get; set; }

        public Node()
        {
            Position = new Vector2();
            Neighbours = new Dictionary<Node, ConnectionType>();
            Type = ConnectionType.Move;
            CanTraverse = true;
            IsOnGround = false;
        }

        public Node(Vector2 position, int gridPositionX, int gridPositionY)
        {
            Position = position;
            GridPositionX = gridPositionX;
            GridPositionY = gridPositionY;
            Neighbours = new Dictionary<Node, ConnectionType>();
            Type = ConnectionType.Move;
            CanTraverse = true;
            IsOnGround = false;
        }

        public float FCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        // --CURRENTLY UNUSED--
        public int CompareTo(Node n)
        {
            int compare = FCost.CompareTo(n.FCost);

            if (compare == 0)
            {
                compare = HCost.CompareTo(n.HCost);
            }

            // Need to flip the sign otherwise we get the wrong result!
            return compare * -1;
        }
    }
}
