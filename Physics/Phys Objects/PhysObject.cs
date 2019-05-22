using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Physics
{
    class PhysObject
    {
        public Vector2 Position { get; set; }
        public RigidBody RigidBody { get; set; }
        public List<PhysObject> BodiesOnTop { get; set; }

        public Texture2D Texture { get; set; }
        public Vector2 TexturePos { get; set; }
        
        public float TimeSinceLastCollisionWithFloor { get; set; }
        public float TimeSinceLastCollision { get; set; }
        public float Elasticity { get; set; }
        public float FrictionCoefficient { get; set; }
        public bool IsTarget { get; set; }
        public bool IsInTheAir { get; set; }
        public bool MarkedForDeletion { get; set; }

        public PhysObject()
        {
            Position = new Vector2();
            RigidBody = new RigidBody();
            BodiesOnTop = new List<PhysObject>();

            TexturePos = new Vector2();

            TimeSinceLastCollisionWithFloor = 0;
            TimeSinceLastCollision = 0;
            Elasticity = 0;
            FrictionCoefficient = 0.2f;

            IsTarget = false;
            IsInTheAir = true;
            MarkedForDeletion = false;
        }
    }
}
