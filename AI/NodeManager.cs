using System;
using System.Collections.Generic;
using System.Text;
using General;
using Physics;
using Microsoft.Xna.Framework;

namespace AI
{
    class NodeManager
    {
        public NodeGrid NodeGrid { get; set; }
        public List<Node> ConnectionNodes { get; set; }
        private World _world;

        public NodeManager(World world)
        {
            NodeGrid = new NodeGrid();
            ConnectionNodes = new List<Node>();
            _world = world;

            // Mark each node that is inside a static object as impassable
            foreach (Node n in NodeGrid.GridNodes)
            {
                foreach (PhysObject po in _world.PhysicsManager.StaticObjects)
                {
                    if (n.Position.X < po.RigidBody.Position.X + (po.RigidBody.Width / 2) &&
                        n.Position.X > po.RigidBody.Position.X - (po.RigidBody.Width / 2) &&
                        n.Position.Y < po.RigidBody.Position.Y + (po.RigidBody.Height / 2) &&
                        n.Position.Y > po.RigidBody.Position.Y - (po.RigidBody.Height / 2) &&
                        !(po is MovingPlatform))
                    {
                        n.CanTraverse = false;
                    }
                }
            }

            PlaceConnectionNodes();
        }

        // Place all the jump nodes in their correct positions
        public void PlaceConnectionNodes()
        {
            int height;

            #region Tier 0 - Index 0->1
            height = 1;
            NodeGrid.GridNodes[20, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[20, height]);

            NodeGrid.GridNodes[43, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[43, height]);
            #endregion

            #region Tier 1 - Index 2->7
            height = 2;
            NodeGrid.GridNodes[2, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[2, height]);

            NodeGrid.GridNodes[13, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[13, height]);

            NodeGrid.GridNodes[18, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[18, height]);

            NodeGrid.GridNodes[45, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[45, height]);

            NodeGrid.GridNodes[50, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[50, height]);

            NodeGrid.GridNodes[61, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[61, height]);
            #endregion

            #region Tier 2 - Index 8->11
            height = 8;
            NodeGrid.GridNodes[5, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[5, height]);

            NodeGrid.GridNodes[10, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[10, height]);

            NodeGrid.GridNodes[53, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[53, height]);

            NodeGrid.GridNodes[58, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[58, height]);
            #endregion

            #region Tier 3 - Index 12->13
            height = 13;
            NodeGrid.GridNodes[18, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[18, height]);

            NodeGrid.GridNodes[45, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[45, height]);

            #endregion

            #region Tier 4 - Index 14->17
            height = 16;
            NodeGrid.GridNodes[25, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[25, height]);

            NodeGrid.GridNodes[31, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[31, height]);

            NodeGrid.GridNodes[32, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[32, height]);

            NodeGrid.GridNodes[38, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[38, height]);
            #endregion

            #region Tier 5 - Index 18->21
            height = 19;
            NodeGrid.GridNodes[9, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[9, height]);

            NodeGrid.GridNodes[14, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[14, height]);

            NodeGrid.GridNodes[49, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[49, height]);

            NodeGrid.GridNodes[54, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[54, height]);
            #endregion

            #region Tier 6 - Index 22->27
            height = 25;
            NodeGrid.GridNodes[6, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[6, height]);

            NodeGrid.GridNodes[21, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[21, height]);

            NodeGrid.GridNodes[31, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[31, height]);

            NodeGrid.GridNodes[32, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[32, height]);

            NodeGrid.GridNodes[42, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[42, height]);

            NodeGrid.GridNodes[57, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[57, height]);
            #endregion

            #region Tier 7 - Index 28->31
            height = 32;
            NodeGrid.GridNodes[10, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[10, height]);

            NodeGrid.GridNodes[15, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[15, height]);

            NodeGrid.GridNodes[48, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[48, height]);

            NodeGrid.GridNodes[53, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[53, height]);
            #endregion

            #region Tier 8 - Index 32->35
            height = 36;
            NodeGrid.GridNodes[17, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[18, height]);

            NodeGrid.GridNodes[29, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[29, height]);

            NodeGrid.GridNodes[34, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[34, height]);

            NodeGrid.GridNodes[46, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[45, height]);
            #endregion

            #region Tier 9 - Index 36->41
            height = 45;
            NodeGrid.GridNodes[6, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[6, height]);

            NodeGrid.GridNodes[20, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[20, height]);

            NodeGrid.GridNodes[25, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[25, height]);

            NodeGrid.GridNodes[38, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[38, height]);

            NodeGrid.GridNodes[43, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[43, height]);

            NodeGrid.GridNodes[57, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[57, height]);
            #endregion

            #region Tier 10 - Index 42->45
            height = 53;
            NodeGrid.GridNodes[23, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[23, height]);

            NodeGrid.GridNodes[26, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[26, height]);

            NodeGrid.GridNodes[37, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[37, height]);

            NodeGrid.GridNodes[40, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[40, height]);
            #endregion

            #region Tier 11 - Index 46->47
            height = 57;
            NodeGrid.GridNodes[29, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[29, height]);

            NodeGrid.GridNodes[34, height].Type = Node.ConnectionType.Jump;
            ConnectionNodes.Add(NodeGrid.GridNodes[34, height]);
            #endregion

            SetConnectionNodeNeighbours();
        }

        // Set all the neighbours for all the connection nodes (WARNING: Many lines of similar code)
        public void SetConnectionNodeNeighbours()
        {
            ConnectionNodes[0].Neighbours.Add(ConnectionNodes[1], Node.ConnectionType.Move);
            ConnectionNodes[0].Neighbours.Add(ConnectionNodes[4], Node.ConnectionType.Jump);

            ConnectionNodes[1].Neighbours.Add(ConnectionNodes[0], Node.ConnectionType.Move);
            ConnectionNodes[1].Neighbours.Add(ConnectionNodes[5], Node.ConnectionType.Jump);

            ConnectionNodes[2].Neighbours.Add(ConnectionNodes[3], Node.ConnectionType.Move);
            ConnectionNodes[2].Neighbours.Add(ConnectionNodes[8], Node.ConnectionType.Jump);

            ConnectionNodes[3].Neighbours.Add(ConnectionNodes[2], Node.ConnectionType.Move);
            ConnectionNodes[3].Neighbours.Add(ConnectionNodes[4], Node.ConnectionType.Move);
            ConnectionNodes[3].Neighbours.Add(ConnectionNodes[9], Node.ConnectionType.Jump);

            ConnectionNodes[4].Neighbours.Add(ConnectionNodes[0], Node.ConnectionType.Move);
            ConnectionNodes[4].Neighbours.Add(ConnectionNodes[3], Node.ConnectionType.Move);

            ConnectionNodes[5].Neighbours.Add(ConnectionNodes[1], Node.ConnectionType.Move);
            ConnectionNodes[5].Neighbours.Add(ConnectionNodes[6], Node.ConnectionType.Move);

            ConnectionNodes[6].Neighbours.Add(ConnectionNodes[5], Node.ConnectionType.Move);
            ConnectionNodes[6].Neighbours.Add(ConnectionNodes[7], Node.ConnectionType.Move);
            ConnectionNodes[6].Neighbours.Add(ConnectionNodes[10], Node.ConnectionType.Jump);

            ConnectionNodes[7].Neighbours.Add(ConnectionNodes[6], Node.ConnectionType.Move);
            ConnectionNodes[7].Neighbours.Add(ConnectionNodes[11], Node.ConnectionType.Jump);

            ConnectionNodes[8].Neighbours.Add(ConnectionNodes[2], Node.ConnectionType.Move);
            ConnectionNodes[8].Neighbours.Add(ConnectionNodes[9], Node.ConnectionType.Move);

            ConnectionNodes[9].Neighbours.Add(ConnectionNodes[3], Node.ConnectionType.Move);
            ConnectionNodes[9].Neighbours.Add(ConnectionNodes[8], Node.ConnectionType.Move);
            ConnectionNodes[9].Neighbours.Add(ConnectionNodes[12], Node.ConnectionType.Jump);

            ConnectionNodes[10].Neighbours.Add(ConnectionNodes[6], Node.ConnectionType.Move);
            ConnectionNodes[10].Neighbours.Add(ConnectionNodes[11], Node.ConnectionType.Move);
            ConnectionNodes[10].Neighbours.Add(ConnectionNodes[13], Node.ConnectionType.Jump);

            ConnectionNodes[11].Neighbours.Add(ConnectionNodes[7], Node.ConnectionType.Move);
            ConnectionNodes[11].Neighbours.Add(ConnectionNodes[10], Node.ConnectionType.Move);
            
            ConnectionNodes[12].Neighbours.Add(ConnectionNodes[8], Node.ConnectionType.Move);
            ConnectionNodes[12].Neighbours.Add(ConnectionNodes[9], Node.ConnectionType.Move);
            ConnectionNodes[12].Neighbours.Add(ConnectionNodes[14], Node.ConnectionType.Jump);
            ConnectionNodes[12].Neighbours.Add(ConnectionNodes[19], Node.ConnectionType.Jump);
            
            ConnectionNodes[13].Neighbours.Add(ConnectionNodes[10], Node.ConnectionType.Move);
            ConnectionNodes[13].Neighbours.Add(ConnectionNodes[11], Node.ConnectionType.Move);
            ConnectionNodes[13].Neighbours.Add(ConnectionNodes[17], Node.ConnectionType.Jump);
            ConnectionNodes[13].Neighbours.Add(ConnectionNodes[20], Node.ConnectionType.Jump);

            ConnectionNodes[14].Neighbours.Add(ConnectionNodes[12], Node.ConnectionType.Move);
            ConnectionNodes[14].Neighbours.Add(ConnectionNodes[15], Node.ConnectionType.Move);
            ConnectionNodes[14].Neighbours.Add(ConnectionNodes[19], Node.ConnectionType.Jump);

            ConnectionNodes[15].Neighbours.Add(ConnectionNodes[14], Node.ConnectionType.Move);
            ConnectionNodes[15].Neighbours.Add(ConnectionNodes[16], Node.ConnectionType.Move);

            ConnectionNodes[16].Neighbours.Add(ConnectionNodes[15], Node.ConnectionType.Move);
            ConnectionNodes[16].Neighbours.Add(ConnectionNodes[17], Node.ConnectionType.Move);

            ConnectionNodes[17].Neighbours.Add(ConnectionNodes[13], Node.ConnectionType.Move);
            ConnectionNodes[17].Neighbours.Add(ConnectionNodes[16], Node.ConnectionType.Move);
            ConnectionNodes[17].Neighbours.Add(ConnectionNodes[20], Node.ConnectionType.Jump);

            ConnectionNodes[18].Neighbours.Add(ConnectionNodes[19], Node.ConnectionType.Move);
            ConnectionNodes[18].Neighbours.Add(ConnectionNodes[22], Node.ConnectionType.Jump);
            ConnectionNodes[18].Neighbours.Add(ConnectionNodes[23], Node.ConnectionType.Jump);

            ConnectionNodes[19].Neighbours.Add(ConnectionNodes[12], Node.ConnectionType.Move);
            ConnectionNodes[19].Neighbours.Add(ConnectionNodes[14], Node.ConnectionType.Move);
            ConnectionNodes[19].Neighbours.Add(ConnectionNodes[18], Node.ConnectionType.Move);
            ConnectionNodes[19].Neighbours.Add(ConnectionNodes[22], Node.ConnectionType.Jump);
            ConnectionNodes[19].Neighbours.Add(ConnectionNodes[23], Node.ConnectionType.Jump);

            ConnectionNodes[20].Neighbours.Add(ConnectionNodes[13], Node.ConnectionType.Move);
            ConnectionNodes[20].Neighbours.Add(ConnectionNodes[17], Node.ConnectionType.Move);
            ConnectionNodes[20].Neighbours.Add(ConnectionNodes[21], Node.ConnectionType.Move);
            ConnectionNodes[20].Neighbours.Add(ConnectionNodes[26], Node.ConnectionType.Jump);
            ConnectionNodes[20].Neighbours.Add(ConnectionNodes[27], Node.ConnectionType.Jump);

            ConnectionNodes[21].Neighbours.Add(ConnectionNodes[20], Node.ConnectionType.Move);
            ConnectionNodes[21].Neighbours.Add(ConnectionNodes[26], Node.ConnectionType.Jump);
            ConnectionNodes[21].Neighbours.Add(ConnectionNodes[27], Node.ConnectionType.Jump);

            ConnectionNodes[22].Neighbours.Add(ConnectionNodes[18], Node.ConnectionType.Move);
            ConnectionNodes[22].Neighbours.Add(ConnectionNodes[19], Node.ConnectionType.Move);
            ConnectionNodes[22].Neighbours.Add(ConnectionNodes[23], Node.ConnectionType.Jump);
            ConnectionNodes[22].Neighbours.Add(ConnectionNodes[28], Node.ConnectionType.Jump);

            ConnectionNodes[23].Neighbours.Add(ConnectionNodes[18], Node.ConnectionType.Move);
            ConnectionNodes[23].Neighbours.Add(ConnectionNodes[19], Node.ConnectionType.Move);
            ConnectionNodes[23].Neighbours.Add(ConnectionNodes[22], Node.ConnectionType.Jump);
            ConnectionNodes[23].Neighbours.Add(ConnectionNodes[24], Node.ConnectionType.Move);

            ConnectionNodes[24].Neighbours.Add(ConnectionNodes[23], Node.ConnectionType.Move);
            ConnectionNodes[24].Neighbours.Add(ConnectionNodes[25], Node.ConnectionType.Jump);

            ConnectionNodes[25].Neighbours.Add(ConnectionNodes[24], Node.ConnectionType.Jump);
            ConnectionNodes[25].Neighbours.Add(ConnectionNodes[26], Node.ConnectionType.Move);

            ConnectionNodes[26].Neighbours.Add(ConnectionNodes[20], Node.ConnectionType.Move);
            ConnectionNodes[26].Neighbours.Add(ConnectionNodes[21], Node.ConnectionType.Move);
            ConnectionNodes[26].Neighbours.Add(ConnectionNodes[25], Node.ConnectionType.Move);
            ConnectionNodes[26].Neighbours.Add(ConnectionNodes[27], Node.ConnectionType.Jump);

            ConnectionNodes[27].Neighbours.Add(ConnectionNodes[20], Node.ConnectionType.Move);
            ConnectionNodes[27].Neighbours.Add(ConnectionNodes[21], Node.ConnectionType.Move);
            ConnectionNodes[27].Neighbours.Add(ConnectionNodes[26], Node.ConnectionType.Jump);
            ConnectionNodes[27].Neighbours.Add(ConnectionNodes[31], Node.ConnectionType.Jump);

            ConnectionNodes[28].Neighbours.Add(ConnectionNodes[22], Node.ConnectionType.Move);
            ConnectionNodes[28].Neighbours.Add(ConnectionNodes[29], Node.ConnectionType.Move);
            ConnectionNodes[28].Neighbours.Add(ConnectionNodes[32], Node.ConnectionType.Jump);

            ConnectionNodes[29].Neighbours.Add(ConnectionNodes[28], Node.ConnectionType.Move);
            ConnectionNodes[29].Neighbours.Add(ConnectionNodes[32], Node.ConnectionType.Jump);

            ConnectionNodes[30].Neighbours.Add(ConnectionNodes[31], Node.ConnectionType.Move);
            ConnectionNodes[30].Neighbours.Add(ConnectionNodes[35], Node.ConnectionType.Jump);

            ConnectionNodes[31].Neighbours.Add(ConnectionNodes[27], Node.ConnectionType.Move);
            ConnectionNodes[31].Neighbours.Add(ConnectionNodes[30], Node.ConnectionType.Move);
            ConnectionNodes[31].Neighbours.Add(ConnectionNodes[35], Node.ConnectionType.Jump);

            ConnectionNodes[32].Neighbours.Add(ConnectionNodes[28], Node.ConnectionType.Move);
            ConnectionNodes[32].Neighbours.Add(ConnectionNodes[29], Node.ConnectionType.Move);
            ConnectionNodes[32].Neighbours.Add(ConnectionNodes[33], Node.ConnectionType.Move);
            ConnectionNodes[32].Neighbours.Add(ConnectionNodes[36], Node.ConnectionType.Jump);
            ConnectionNodes[32].Neighbours.Add(ConnectionNodes[37], Node.ConnectionType.Jump);

            ConnectionNodes[33].Neighbours.Add(ConnectionNodes[36], Node.ConnectionType.Move);
            ConnectionNodes[33].Neighbours.Add(ConnectionNodes[34], Node.ConnectionType.Jump);
            ConnectionNodes[33].Neighbours.Add(ConnectionNodes[38], Node.ConnectionType.Jump);
            ConnectionNodes[33].Neighbours.Add(ConnectionNodes[39], Node.ConnectionType.Jump);

            ConnectionNodes[34].Neighbours.Add(ConnectionNodes[33], Node.ConnectionType.Jump);
            ConnectionNodes[34].Neighbours.Add(ConnectionNodes[35], Node.ConnectionType.Move);
            ConnectionNodes[34].Neighbours.Add(ConnectionNodes[38], Node.ConnectionType.Jump);
            ConnectionNodes[34].Neighbours.Add(ConnectionNodes[39], Node.ConnectionType.Jump);

            ConnectionNodes[35].Neighbours.Add(ConnectionNodes[30], Node.ConnectionType.Move);
            ConnectionNodes[35].Neighbours.Add(ConnectionNodes[31], Node.ConnectionType.Move);
            ConnectionNodes[35].Neighbours.Add(ConnectionNodes[34], Node.ConnectionType.Move);
            ConnectionNodes[35].Neighbours.Add(ConnectionNodes[40], Node.ConnectionType.Jump);
            ConnectionNodes[35].Neighbours.Add(ConnectionNodes[41], Node.ConnectionType.Jump);

            ConnectionNodes[36].Neighbours.Add(ConnectionNodes[36], Node.ConnectionType.Move);
            ConnectionNodes[36].Neighbours.Add(ConnectionNodes[37], Node.ConnectionType.Jump);
            ConnectionNodes[36].Neighbours.Add(ConnectionNodes[42], Node.ConnectionType.Jump);

            ConnectionNodes[37].Neighbours.Add(ConnectionNodes[32], Node.ConnectionType.Move);
            ConnectionNodes[37].Neighbours.Add(ConnectionNodes[36], Node.ConnectionType.Jump);
            ConnectionNodes[37].Neighbours.Add(ConnectionNodes[38], Node.ConnectionType.Move);
            ConnectionNodes[37].Neighbours.Add(ConnectionNodes[42], Node.ConnectionType.Jump);

            ConnectionNodes[38].Neighbours.Add(ConnectionNodes[33], Node.ConnectionType.Move);
            ConnectionNodes[38].Neighbours.Add(ConnectionNodes[34], Node.ConnectionType.Move);
            ConnectionNodes[38].Neighbours.Add(ConnectionNodes[37], Node.ConnectionType.Move);
            ConnectionNodes[38].Neighbours.Add(ConnectionNodes[39], Node.ConnectionType.Jump);

            ConnectionNodes[39].Neighbours.Add(ConnectionNodes[33], Node.ConnectionType.Move);
            ConnectionNodes[39].Neighbours.Add(ConnectionNodes[34], Node.ConnectionType.Move);
            ConnectionNodes[39].Neighbours.Add(ConnectionNodes[38], Node.ConnectionType.Jump);
            ConnectionNodes[39].Neighbours.Add(ConnectionNodes[40], Node.ConnectionType.Move);

            ConnectionNodes[40].Neighbours.Add(ConnectionNodes[35], Node.ConnectionType.Move);
            ConnectionNodes[40].Neighbours.Add(ConnectionNodes[39], Node.ConnectionType.Move);
            ConnectionNodes[40].Neighbours.Add(ConnectionNodes[41], Node.ConnectionType.Jump);
            ConnectionNodes[40].Neighbours.Add(ConnectionNodes[45], Node.ConnectionType.Jump);

            ConnectionNodes[41].Neighbours.Add(ConnectionNodes[35], Node.ConnectionType.Move);
            ConnectionNodes[41].Neighbours.Add(ConnectionNodes[40], Node.ConnectionType.Jump);
            ConnectionNodes[41].Neighbours.Add(ConnectionNodes[45], Node.ConnectionType.Jump);

            ConnectionNodes[42].Neighbours.Add(ConnectionNodes[36], Node.ConnectionType.Move);
            ConnectionNodes[42].Neighbours.Add(ConnectionNodes[37], Node.ConnectionType.Move);
            ConnectionNodes[42].Neighbours.Add(ConnectionNodes[43], Node.ConnectionType.Move);
            ConnectionNodes[42].Neighbours.Add(ConnectionNodes[46], Node.ConnectionType.Jump);

            ConnectionNodes[43].Neighbours.Add(ConnectionNodes[42], Node.ConnectionType.Move);
            ConnectionNodes[43].Neighbours.Add(ConnectionNodes[46], Node.ConnectionType.Jump);

            ConnectionNodes[44].Neighbours.Add(ConnectionNodes[45], Node.ConnectionType.Move);
            ConnectionNodes[44].Neighbours.Add(ConnectionNodes[47], Node.ConnectionType.Jump);

            ConnectionNodes[45].Neighbours.Add(ConnectionNodes[40], Node.ConnectionType.Move);
            ConnectionNodes[45].Neighbours.Add(ConnectionNodes[41], Node.ConnectionType.Move);
            ConnectionNodes[45].Neighbours.Add(ConnectionNodes[44], Node.ConnectionType.Move);
            ConnectionNodes[45].Neighbours.Add(ConnectionNodes[47], Node.ConnectionType.Jump);

            ConnectionNodes[46].Neighbours.Add(ConnectionNodes[42], Node.ConnectionType.Move);
            ConnectionNodes[46].Neighbours.Add(ConnectionNodes[43], Node.ConnectionType.Move);
            ConnectionNodes[46].Neighbours.Add(ConnectionNodes[47], Node.ConnectionType.Move);

            ConnectionNodes[47].Neighbours.Add(ConnectionNodes[44], Node.ConnectionType.Move);
            ConnectionNodes[47].Neighbours.Add(ConnectionNodes[45], Node.ConnectionType.Move);
            ConnectionNodes[47].Neighbours.Add(ConnectionNodes[46], Node.ConnectionType.Move);
        }

        // Returs a float of the distance between 2 nodes
        public float DistanceToNode(Node nodeA, Node nodeB)
        {
            return Vector2.Distance(nodeA.Position, nodeB.Position);
        }

        // A* pathfinding, uses pre-allocated connection nodes as the nodes
        public List<Node> FindPath(Node startNode, Node endNode)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost ||
                        (openSet[i].FCost == currentNode.FCost &&
                        openSet[i].HCost < currentNode.HCost))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    return CreatePath(startNode, currentNode);
                }

                foreach (KeyValuePair<Node, Node.ConnectionType> n in currentNode.Neighbours)
                {
                    if (closedSet.Contains(n.Key) || !n.Key.CanTraverse)
                    {
                        continue;
                    }

                    float newMovementCost = currentNode.GCost + DistanceToNode(currentNode, n.Key);

                    if (!openSet.Contains(n.Key) || newMovementCost < n.Key.GCost)
                    {
                        n.Key.GCost = newMovementCost;
                        n.Key.HCost = DistanceToNode(n.Key, endNode);
                        n.Key.Parent = currentNode;

                        if (!openSet.Contains(n.Key))
                        {
                            openSet.Add(n.Key);
                        }
                    }
                }
            }

            // Failed to find path
            return null;
        }

        // Returns the full path from the passed in list of nodes from the above function
        public List<Node> CreatePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            // Path is in reverse order, so reverse it to get it in the correct order
            if (path.Count != 0) { path.Reverse(); }

            return path;
        }

        // Finds the closest connection node to a specified vector position
        public Node GetClosestNodeTo(Vector2 position)
        {
            float distance = 100000000;
            Node returnNode = new Node();

            foreach (Node n in ConnectionNodes)
            {
                if (!n.CanTraverse)
                {
                    continue;
                }

                if (Vector2.Distance(position, n.Position) < distance && n.Position != position)
                {
                    distance = Vector2.Distance(position, n.Position);
                    returnNode = n;
                }
            }

            return returnNode;
        }
    }
}
