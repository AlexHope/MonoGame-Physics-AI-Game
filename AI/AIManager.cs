using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using General;
using Physics;

namespace AI
{
    class AIManager
    {
        public NodeManager NodeManager { get; set; }
        public List<Node> DebugNodeDisplay { get; set; }
        public List<Node> DebugPathCheck { get; set; }

        private World _world;
        private Vector2 _targetPosition;
        private Node _targetNode;
        private List<Character> _enemies;
        private List<PhysObject> _movingPlatforms;
        private List<Node> _switchedNodes;

        public AIManager(World world)
        {
            _world = world;
            NodeManager = new NodeManager(_world);
            DebugNodeDisplay = new List<Node>();
            DebugPathCheck = new List<Node>();
            _enemies = _world.Enemies;
            _movingPlatforms = new List<PhysObject>();
            _switchedNodes = new List<Node>();
        }

        public void Update(World world, float dt)
        {
            // Update the list of enemies to match the world
            _enemies = world.Enemies;
            // Grab the position of the target
            _targetPosition = _world.Target.RigidBody.Position;
            _targetNode = NodeManager.NodeGrid.GetNodeFromVector(_targetPosition);

            // Mark the nodes covered by the moving platforms as impassable
            //MarkMovingPlatformNodes();

            DebugNodeDisplay = NodeManager.ConnectionNodes;

            // Debug check, toggles enemy AI
            if (_world.AllowAIBehaviour)
            {
                // Update each of the enemy characters AI behaviour
                for (int i = 0; i < _enemies.Count; i++)
                {
                    _enemies[i].Behaviour.Update(_targetNode, dt);
                }

                // Used to display the current path of the first enemy
                if (_enemies.Count > 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        if (_enemies[i].Behaviour.CurrentPath != null)
                        {
                            DebugPathCheck = _enemies[i].Behaviour.CurrentPath;
                        }
                    }
                }
            }
        }

        // Marks the nodes covered by moving platforms as impassable
        public void MarkMovingPlatformNodes()
        {
            foreach (Node n in _switchedNodes)
            {
                n.CanTraverse = true;
            }

            _switchedNodes.Clear();
            _movingPlatforms.Clear();

            foreach (PhysObject po in _world.PhysicsManager.StaticObjects)
            {
                if (po is MovingPlatform)
                {
                    _movingPlatforms.Add(po);
                }
            }

            foreach (Node n in NodeManager.NodeGrid.GridNodes)
            {
                foreach (PhysObject po in _movingPlatforms)
                {
                    if (n.Position.X < po.RigidBody.Position.X + (po.RigidBody.Width / 2) &&
                        n.Position.X > po.RigidBody.Position.X - (po.RigidBody.Width / 2) &&
                        n.Position.Y < po.RigidBody.Position.Y + (po.RigidBody.Height / 2) &&
                        n.Position.Y > po.RigidBody.Position.Y - (po.RigidBody.Height / 2))
                    {
                        n.CanTraverse = false;
                        _switchedNodes.Add(n);
                    }
                }
            }
        }
    }
}
