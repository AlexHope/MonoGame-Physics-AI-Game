using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Physics
{
    class PhysicsManager
    {
        private General.World World { get; set; }

        public List<PhysObject> StaticObjects { get; set; }
        public List<PhysObject> DynamicObjects { get; set; }
        public List<PhysObject> AllPhysObjects { get; set; }
        public List<PhysObject> DeletionList { get; set; }

        public Vector2 Gravity { get; set; }
        private CollisionHandler CollisionHandler { get; set; }

        public Character Player { get; set; }
        public PhysObject Target { get; set; }

        public float MaximumHorizontalSpeed { get; set; }
        public float MaximumVerticalSpeed { get; set; }
        private const float _portalSpeed = 150;
        private const float _projectileSpeed = 600;
        private const float _maximumMass = 100000;

        // For debugging
        public bool IgnoreCollision { get; set; }

        // Playable area
        private const float _farEdge = 2048; // X coordinate of the edge of the right-most wall
        private const float _nearEdge = 0; // X coordinate of the edge of the left-most wall
        private const float _bottomEdge = 2496; // Y coordinate of the bottom edge of the playable area
        private const float _topEdge = 0; // Y coordinate of the top edge of the playable area

        public PhysicsManager(GraphicsDevice g, General.World world)
        {
            World = world;

            StaticObjects = new List<PhysObject>();
            DynamicObjects = new List<PhysObject>();
            AllPhysObjects = new List<PhysObject>();
            DeletionList = new List<PhysObject>();

            CollisionHandler = new CollisionHandler();

            Gravity = new Vector2(0.0f, 500f);
            IgnoreCollision = false;
            MaximumHorizontalSpeed = 400.0f;
            MaximumVerticalSpeed = 600.0f;

            // Create the game world physics objects
            CreateEnvironment(g);

            CollisionHandler.World = World;
        }

        // Create platforms & environment
        private void CreateEnvironment(GraphicsDevice g)
        {
            #region Create Player & Target
            // First objects to create are the player and the target
            Player = new Character(true);
            Player.Position = new Vector2(_nearEdge + 32, _bottomEdge - 128);
            Player.IsPlayer = true;
            RigidBody playerBody = Player.RigidBody;
            playerBody.Position = Player.Position;
            playerBody.Width = 32;
            playerBody.Height = 32;
            playerBody.CollidableObject = new BoxCollider();
            Player.TexturePos = new Vector2(Player.Position.X - playerBody.Width / 2, Player.Position.Y - playerBody.Height / 2);
            DynamicObjects.Add(Player);
            AllPhysObjects.Add(Player);

            Target = new PhysObject();
            Target.Position = new Vector2(_farEdge / 2, _bottomEdge - 256);
            Target.IsTarget = true;
            Target.Elasticity = 1;
            RigidBody targetBody = Target.RigidBody;
            targetBody.Mass = 100.0f;
            targetBody.Position = Target.Position;
            targetBody.Width = 32;
            targetBody.Height = 32;
            targetBody.CollidableObject = new BoxCollider();
            targetBody.CollidableObject.IsCircle = true;
            targetBody.AffectedByPhysics = false;
            targetBody.IgnoreGravity = true;

            float targetYVelocity = ((float)World.RandomGen.Next(3, 7) / 10.0f);
            if (World.RandomGen.Next(0, 1) == 0) { targetYVelocity *= -1; }
            float targetXVelocity = ((float)World.RandomGen.Next(3, 7) / 10.0f);
            if (World.RandomGen.Next(0, 1) == 0) { targetXVelocity *= -1; }
            targetBody.LinearVelocity = new Vector2(targetXVelocity * _portalSpeed, targetYVelocity * _portalSpeed);

            Target.TexturePos = new Vector2(targetBody.Position.X - targetBody.Width / 2, targetBody.Position.X - targetBody.Height / 2);
            DynamicObjects.Add(Target);
            AllPhysObjects.Add(Target);
            #endregion

            #region Edges
            // EDGES - Create the far left walls
            PhysObject block = new PhysObject();
            block.Position = new Vector2(_nearEdge - 480, _bottomEdge / 2);
            RigidBody body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = _farEdge / 2;
            body.Height = _bottomEdge;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // EDGES - Create the far right wall
            block = new PhysObject();
            block.Position = new Vector2(_farEdge + 480, _bottomEdge / 2);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = _farEdge / 2;
            body.Height = _bottomEdge;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // EDGES - Create the main floor
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2, _bottomEdge + 480);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 4096;
            body.Height = 1024;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // EDGES - Create the roof
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2, _topEdge - 480);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 4096;
            body.Height = 1024;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 0
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 320, _bottomEdge - 32);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 640;
            body.Height = 64;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);


            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 320, _bottomEdge - 32);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 640;
            body.Height = 64;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 1
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 256, _bottomEdge - 240);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Moving Platform
            block = new MovingPlatform(new Vector2(_nearEdge + 256, _bottomEdge - 144),
                                       new Vector2(_nearEdge + 192, _bottomEdge - 144),
                                       new Vector2(_nearEdge + (_farEdge / 2) - 160, _bottomEdge - 144),
                                       new Vector2(100, 0));
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Moving Platform
            block = new MovingPlatform(new Vector2(_farEdge - 256, _bottomEdge - 144),
                                       new Vector2((_farEdge / 2) + 160, _bottomEdge - 144),
                                       new Vector2(_farEdge - 192, _bottomEdge - 144),
                                       new Vector2(-100, 0));
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 256, _bottomEdge - 240);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 2
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 - 448, _bottomEdge - 400);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 128;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 + 448, _bottomEdge - 400);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 128;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 3
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2, _bottomEdge - 496);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 512;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            /*
            // Pendulum (spins, collisions partially broken though, uncomment if you want to see (spawns to the right of the player start))
            block = new Pendulum(new Vector2(_farEdge / 2, _bottomEdge - 496), new Vector2(0, 128));
            DynamicObjects.Add(block);
            AllPhysObjects.Add(block);
            */
            
            #endregion

            #region Platforms - Tier 4
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 256, _bottomEdge - 592);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 512;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 256, _bottomEdge - 592);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 512;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 5
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 128, _bottomEdge - 784);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 - 208, _bottomEdge - 784);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 352;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Thicker block
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 + 80, _bottomEdge - 704);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 96;
            body.Height = 128;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Thicker block
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 - 80, _bottomEdge - 704);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 96;
            body.Height = 128;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 + 208, _bottomEdge - 784);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 352;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 128, _bottomEdge - 784);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 6
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 432, _bottomEdge - 1008);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 288;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Thicker block
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 - 80, _bottomEdge - 1040);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 96;
            body.Height = 160;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Thicker block
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 + 80, _bottomEdge - 1040);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 96;
            body.Height = 160;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 432, _bottomEdge - 1008);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 288;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 7
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 560, _bottomEdge - 1072);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 32;
            body.Height = 96;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 560, _bottomEdge - 1072);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 32;
            body.Height = 96;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 8
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 - 256, _bottomEdge - 1136);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 448;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 + 256, _bottomEdge - 1136);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 448;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 9
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_nearEdge + 128, _bottomEdge - 1424);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 - 288, _bottomEdge - 1424);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2 + 288, _bottomEdge - 1424);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge - 128, _bottomEdge - 1424);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 10
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2, _bottomEdge - 1680);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 640;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 11
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2, _bottomEdge - 1760);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 128;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 12
            // Moving Platform
            block = new MovingPlatform(new Vector2(_nearEdge + 256, _bottomEdge - 2000),
                                       new Vector2(_nearEdge + 160, _bottomEdge - 2000), 
                                       new Vector2(_nearEdge + (_farEdge / 2) - 160, _bottomEdge - 2000),
                                       new Vector2(100, 0));
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);

            // Moving Platform
            block = new MovingPlatform(new Vector2(_farEdge - 256, _bottomEdge - 2000),
                                       new Vector2((_farEdge / 2) + 160, _bottomEdge - 2000),
                                       new Vector2(_farEdge - 160, _bottomEdge - 2000),
                                       new Vector2(-100, 0));
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 256;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion

            #region Platforms - Tier 13
            // Platform
            block = new PhysObject();
            block.Position = new Vector2(_farEdge / 2, _bottomEdge - 2288);
            body = block.RigidBody;
            body.Mass = _maximumMass;
            body.IgnoreGravity = true;
            body.IsDynamic = false;
            body.Position = block.Position;
            body.Width = 512;
            body.Height = 32;
            body.CollidableObject = new BoxCollider();
            StaticObjects.Add(block);
            AllPhysObjects.Add(block);
            #endregion
        }

        // Update world physics
        public void Update(float dt)
        {
            // Update the position of each rigid body & texture
            foreach (PhysObject po in DynamicObjects)
            {
                po.RigidBody.Update(dt, this, po);
                po.TexturePos = new Vector2(po.RigidBody.Position.X - (po.RigidBody.Width / 2), po.RigidBody.Position.Y - (po.RigidBody.Height / 2));
                po.Position = po.RigidBody.Position;

                if (po.IsTarget)
                {
                    if (Math.Abs(po.RigidBody.LinearVelocity.X) != _portalSpeed)
                    {
                        po.RigidBody.LinearVelocity = new Vector2(Math.Sign(po.RigidBody.LinearVelocity.X) * _portalSpeed, po.RigidBody.LinearVelocity.Y);
                    }
                }

                if (po is Projectile)
                {
                    if (Math.Abs(po.RigidBody.LinearVelocity.Length()) != _projectileSpeed)
                    {
                        po.RigidBody.LinearVelocity = Vector2.Normalize(po.RigidBody.LinearVelocity);
                        po.RigidBody.LinearVelocity = new Vector2(po.RigidBody.LinearVelocity.X * _projectileSpeed, po.RigidBody.LinearVelocity.Y * _projectileSpeed);
                    }
                }

                if (po.MarkedForDeletion)
                {
                    DeletionList.Add(po);
                }
            }

            foreach (PhysObject po in StaticObjects)
            {
                if (po is MovingPlatform || po is Pendulum)
                {
                    po.RigidBody.Update(dt, this, po);
                    po.TexturePos = new Vector2(po.RigidBody.Position.X - (po.RigidBody.Width / 2), po.RigidBody.Position.Y - (po.RigidBody.Height / 2));
                    po.Position = po.RigidBody.Position;
                }
            }

            // Delete objects marked for deletion
            for (int i = 0; i < DeletionList.Count; i++)
            {
                DynamicObjects.Remove(DeletionList[i]);
                AllPhysObjects.Remove(DeletionList[i]);

                if (DeletionList[i] is Character)
                {
                    World.Enemies.Remove((Character)DeletionList[i]);
                }

                DeletionList[i] = null;
            }
            DeletionList.Clear();

            CollisionDetection();
        }

        // DEBUG - Toggle collisions between characters
        public void ToggleCharacterCollision()
        {
            if (IgnoreCollision) { IgnoreCollision = false; }
            else { IgnoreCollision = true; }
        }

        // Detect collisions (uses AABB detection)
        private void CollisionDetection()
        {
            for (int i = DynamicObjects.Count - 1; i >= 0; i--)            
            {
                RigidBody bodyA = DynamicObjects[i].RigidBody;
                AABB physObject_A_AABB = bodyA.CollidableObject.CalculateAABB(bodyA);

                for (int j = AllPhysObjects.Count - 1; j >= 0; j--)
                {
                    if (bodyA != AllPhysObjects[j].RigidBody)
                    {
                        RigidBody bodyB = AllPhysObjects[j].RigidBody;
                        AABB physObject_B_AABB = bodyB.CollidableObject.CalculateAABB(bodyB);

                        // Check if the two objects are colliding
                        if (CheckIntersection_AABB(physObject_A_AABB, physObject_B_AABB))
                        {
                            CollisionData collision = new CollisionData();
                            collision.BodyA = DynamicObjects[i];
                            collision.BodyB = AllPhysObjects[j];

                            if (collision.BodyA is Character && collision.BodyB is Character)
                            {
                                collision.IgnoreCollision = IgnoreCollision;
                            }

                            // Vector between the centre of each body
                            Vector2 separator = bodyB.Position - bodyA.Position;

                            // The distance each axis on body A has penetrated into body B
                            float xSeparator = (bodyA.Width / 2 + bodyB.Width / 2) - Math.Abs(separator.X);
                            float ySeparator = (bodyA.Height / 2 + bodyB.Height / 2) - Math.Abs(separator.Y);

                            if (!bodyB.CollidableObject.IsCircle)
                            {
                                if (xSeparator > ySeparator)
                                {
                                    // Body A has collided with body B from above
                                    if (bodyA.Position.Y < bodyB.Position.Y)
                                    {
                                        collision.ContactNormal = new Vector2(0, -1);
                                        collision.ContactPoint = bodyA.Position - new Vector2(0, -0.2f);
                                    }
                                    // Body A has collided with body B from below
                                    else
                                    {
                                        collision.ContactNormal = new Vector2(0, 1);
                                        collision.ContactPoint = bodyA.Position - new Vector2(0, 0.2f);
                                    }

                                    collision.BodyA.TimeSinceLastCollision = 0;
                                    collision.BodyA.TimeSinceLastCollisionWithFloor = 0;

                                    // If it is a dynamic object, set the counter since last collision to 0
                                    if (bodyB.IsDynamic && !collision.BodyA.IsTarget)
                                    {
                                        collision.BodyB.TimeSinceLastCollision = 0;

                                        // If it collided with the floor, set the counter since last collision with floor to 0
                                        collision.BodyB.TimeSinceLastCollisionWithFloor = 0;
                                    }
                                }
                                else
                                {
                                    // Body A has collided with body B from the left
                                    if (bodyA.Position.X < bodyB.Position.X)
                                    {
                                        collision.ContactNormal = new Vector2(-1, 0);
                                        collision.ContactPoint = bodyA.Position - new Vector2(-0.2f, 0);
                                    }
                                    // Body B has collided with body B from the right
                                    else
                                    {
                                        collision.ContactNormal = new Vector2(1, 0);
                                        collision.ContactPoint = bodyA.Position - new Vector2(0.2f, 0);
                                    }
                                }
                            }
                            else
                            {
                                // Normal must be the distance between the two objects, normalised
                                collision.ContactNormal = Vector2.Normalize(separator); 
                                collision.ContactPoint = bodyA.Position + collision.ContactNormal;
                            }

                            CollisionHandler.AddCollision(collision);
                        }
                    }
                }
            }
            CollisionHandler.ResolveAllCollisions();

            
        }

        private bool CheckIntersection_AABB(AABB a, AABB b)
        {
            // If box A is to the left of box B
            if (b.Min.X > a.Max.X)
            {
                return false;
            }

            // If box A is to the right of box B
            if (b.Max.X < a.Min.X)
            {
                return false;
            }

            // If box A is underneath box B
            if (b.Min.Y > a.Max.Y)
            {
                return false;
            }

            // If box A is above box B
            if (b.Max.Y < a.Min.Y)
            {
                return false;
            }

            // If none of the above return false, then objects must be colliding
            return true;
        }
    }
}
