using System;
using System.Collections.Generic;
using System.Text;
using General;
using Microsoft.Xna.Framework;

namespace AI
{
    class NodeGrid
    {
        public Node[,] GridNodes { get; set; }
        public Vector2 GridSize { get; set; }

        private const int _gridNodeRadius = 16;
        private const int _gridX = 2048;
        private const int _gridY = 2496;
        private const int _gridXNodeCount = _gridX / (_gridNodeRadius * 2);
        private const int _gridYNodeCount = _gridY / (_gridNodeRadius * 2);

        public NodeGrid()
        {
            GridSize = new Vector2(_gridX, _gridY);
            CreateNodeGrid();
        }

        // Returns a node in the grid closest to the specified vector position
        public Node GetNodeFromVector(Vector2 position)
        {
            float xRatio = position.X / _gridX; // Grab how far along the x axis the node is
            float yRatio = (_gridY - position.Y) / _gridY; // Grab how far along the y axis the node is - (gridY - pos) used due to MonoGame origin in top left

            // These values shouldn't be outside of the range 0->1, but check anyway and clamp
            xRatio = MathHelper.Clamp(xRatio, 0, 1);
            yRatio = MathHelper.Clamp(yRatio, 0, 1);

            int x = (int)Math.Round((_gridXNodeCount - 1) * xRatio);
            int y = (int)Math.Round((_gridYNodeCount - 1) * yRatio);
            return GridNodes[x, y];
        }

        // Finds the 8 nodes surrounding the given node
        // --CURRENTLY UNUSED--
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Ignore self node
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int xNodePos = node.GridPositionX + x;
                    int yNodePos = node.GridPositionY + y;

                    if (xNodePos >= 0 && xNodePos < _gridXNodeCount && yNodePos >= 0 && yNodePos < _gridYNodeCount)
                    {
                        neighbours.Add(GridNodes[xNodePos, yNodePos]);
                    }
                }
            }

            return neighbours;
        }

        // Creates the grid of nodes (64x78)
        public void CreateNodeGrid()
        {
            GridNodes = new Node[_gridXNodeCount, _gridYNodeCount];
            
            for (int x = 0; x < GridXNodeCount; x++)
            {
                for (int y = 0; y < GridYNodeCount; y++)
                {
                    // (Height - y) used due to MonoGame origin in top left
                    GridNodes[x, (_gridYNodeCount - 1) - y] = new Node(new Vector2((x * _gridNodeRadius * 2 + _gridNodeRadius), (y  * _gridNodeRadius * 2 + _gridNodeRadius)), x, (_gridYNodeCount - 1) - y);
                }
            }
        }

        public int GridXNodeCount
        {
            get
            {
                return _gridXNodeCount;
            }
        }

        public int GridYNodeCount
        {
            get
            {
                return _gridYNodeCount;
            }
        }

        public int GridNodeCount
        {
            get
            {
                return GridXNodeCount * GridYNodeCount;
            }
        }
    }
}
