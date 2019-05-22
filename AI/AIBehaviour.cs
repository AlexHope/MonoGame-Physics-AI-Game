using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Physics;

namespace AI
{
    class AIBehaviour
    {
        public enum State { Move, Jump }
        public State AIState { get; set; }
        public Node CurrentNode { get; set; }
        public Node NextNode { get; set; }
        public Node PreviousNextNode { get; set; }
        public Node TargetNode { get; set; }
        public List<Node> CurrentPath { get; set; }

        private Character _aiCharacter;
        private NodeManager _nodeManager;

        private int _iterator;
        private float _counter;
        private float _timeSinceLastNodeReached;
        private bool _initialPathFound;
        private bool _currentNodesAdded;
        private bool _targetNodesAdded;
        private bool _connectionNodesTargetAdded;

        public AIBehaviour(Character aiCharacter, NodeManager manager)
        {
            AIState = State.Move;
            TargetNode = new Node();
            CurrentPath = new List<Node>();

            _aiCharacter = aiCharacter;
            _nodeManager = manager;

            _iterator = 0;
            _counter = 0;
            _timeSinceLastNodeReached = 1;
            _initialPathFound = false;
            _currentNodesAdded = false;
            _targetNodesAdded = false;
            _connectionNodesTargetAdded = false;
        }

        public void Update(Node target, float dt)
        {
            _counter += dt;
            _timeSinceLastNodeReached += dt;

            // Grab the target and this character's position nodes
            TargetNode = target;
            CurrentNode = _nodeManager.NodeGrid.GetNodeFromVector(_aiCharacter.Position);

            // Grab the closest connection node to this character's position
            Node closestConnectionNodeToCurrent = _nodeManager.GetClosestNodeTo(CurrentNode.Position);

            // Check if the current position node and the closest connection node are neighbours
            if (!CurrentNode.Neighbours.ContainsKey(closestConnectionNodeToCurrent) && closestConnectionNodeToCurrent != CurrentNode && !closestConnectionNodeToCurrent.Neighbours.ContainsKey(CurrentNode))
            {
                // If they aren't, add them as neighbours to each other
                CurrentNode.Neighbours.Add(closestConnectionNodeToCurrent, Node.ConnectionType.Move);
                closestConnectionNodeToCurrent.Neighbours.Add(CurrentNode, Node.ConnectionType.Move);
                _currentNodesAdded = true;
            }

            // Check if the target is already on top of a connection node
            if (!_nodeManager.ConnectionNodes.Contains(TargetNode))
            {
                // If it isn't, add it as a connection node
                _nodeManager.ConnectionNodes.Add(TargetNode);
                _connectionNodesTargetAdded = true;
            }
            
            // Grab the closest connection node to the target's position
            Node closestConnectionNodeToTarget = _nodeManager.GetClosestNodeTo(TargetNode.Position);

            // Check if the target position node and the closest connection node to it are neighbours
            if (!closestConnectionNodeToTarget.Neighbours.ContainsKey(TargetNode) && closestConnectionNodeToTarget != TargetNode)
            {
                // If they aren't, add them as neighbours to each other
                TargetNode.Neighbours.Add(closestConnectionNodeToTarget, Node.ConnectionType.Move);
                closestConnectionNodeToTarget.Neighbours.Add(TargetNode, Node.ConnectionType.Jump);
                _targetNodesAdded = true;
            }
            
            // Get a new path to the target if the character has stopped moving, or every 0.5 seconds
            if (!_initialPathFound || _aiCharacter.RigidBody.LinearVelocity.Length() < 10 || _counter % 0.4 < 0.2)
            {
                GetPath();
                _initialPathFound = true;
            }

            // If the returned path is null, delete the character so we don't crash the game (should never happen anyway)
            if (CurrentPath == null)
            {
                _aiCharacter.MarkedForDeletion = true;
                return;
            }

            // Ensure we stay within the bounds of the current path
            if (_iterator < CurrentPath.Count)
            {
                // Determine the type of connection to the next node and set the state accordingly
                if (CurrentNode.Neighbours.ContainsKey(CurrentPath[_iterator]))
                {
                    foreach (KeyValuePair<Node, Node.ConnectionType> n in CurrentNode.Neighbours)
                    {
                        if (n.Key == CurrentPath[_iterator])
                        {
                            if (n.Value == Node.ConnectionType.Jump)
                            {
                                AIState = State.Jump;
                            }
                            else
                            {
                                AIState = State.Move;
                            }
                        }
                    }
                }

                switch (AIState)
                {
                    case State.Move:
                        // If the previous node is the same as the next node, skip it so we don't end up moving
                        // backwards and forwards between the same two nodes
                        if (PreviousNextNode != null && NextNode == PreviousNextNode && (_iterator + 1) < CurrentPath.Count)
                        {
                            // In case they get stuck, tell them to move back to the previous node
                            if (_timeSinceLastNodeReached > 2)
                            {
                                NextNode = CurrentPath[_iterator];
                            }
                            else
                            {
                                NextNode = CurrentPath[_iterator + 1];
                            }
                        }
                        else
                        {
                            NextNode = CurrentPath[_iterator];
                        }
                        MoveToNodeOnPath(NextNode);
                        break;
                    case State.Jump:
                        CheckPositionY();
                        break;
                }
            }

            // Clean the neighbours out from the current position if any were added to reset state of the pathfinding
            if (_currentNodesAdded)
            {
                closestConnectionNodeToCurrent.Neighbours.Remove(CurrentNode);
                CurrentNode.Neighbours.Remove(closestConnectionNodeToCurrent);
                _currentNodesAdded = false;
            }

            // Clear out the added connection node to reset the state of the pathfinding
            if (_connectionNodesTargetAdded)
            {
                _nodeManager.ConnectionNodes.Remove(TargetNode);
                _connectionNodesTargetAdded = false;
            }

            // Clean the neighbours out from the target position if any were added to reset state of the pathfinding
            if (_targetNodesAdded)
            {
                TargetNode.Neighbours.Remove(closestConnectionNodeToTarget);
                closestConnectionNodeToTarget.Neighbours.Remove(TargetNode);
                _targetNodesAdded = false;
            }
            
            PreviousNextNode = NextNode;
            _aiCharacter.JumpCooldownRemaining -= dt;
        }

        // Gets the path from the current position node to the target node
        public void GetPath()
        {
            CurrentPath = _nodeManager.FindPath(CurrentNode, TargetNode);
            _iterator = 0;
        }

        // Moves toward the given node
        public void MoveToNodeOnPath(Node n)
        {
            if (n.CanTraverse)
            {
                // Determine which side the given node is relative to the character
                if (n.GridPositionX > CurrentNode.GridPositionX)
                {
                    _aiCharacter.MoveRight();
                }
                else if (n.GridPositionX < CurrentNode.GridPositionX)
                {
                    _aiCharacter.MoveLeft();
                }
            }

            // Check if this character is very close to the given node & tell the character to move on to the next node if true
            if (Math.Abs(n.Position.X - CurrentNode.Position.X) < 16 &&
                Math.Abs(n.Position.Y - CurrentNode.Position.Y) < 16)
            {
                _iterator++;
                _timeSinceLastNodeReached = 0;
            }
        }

        // Determine if the character should be jumping or not
        public void CheckPositionY()
        {
            if (CurrentPath.Count > 0)
            {
                // If this character is not in the air, and the next node is above the character's current position, then jump
                if (_aiCharacter.Position.Y > CurrentPath[0 + _iterator].Position.Y && !_aiCharacter.IsInTheAir)
                {
                    _aiCharacter.Jump();
                }
                // Otherwise if the next node is on the same level and we're not in the air, move toward the next node
                // Or if we've in the air and above the next node, move toward the next node
                else if ((_nodeManager.NodeGrid.GetNodeFromVector(_aiCharacter.Position).GridPositionY == CurrentPath[0 + _iterator].GridPositionY && !_aiCharacter.IsInTheAir) ||
                         _aiCharacter.Position.Y < CurrentPath[_iterator].Position.Y)
                {
                    MoveToNodeOnPath(CurrentPath[_iterator]);
                }
            }
        }
    }
}
