using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class CollisionData
    {
        public PhysObject BodyA { get; set; }
        public PhysObject BodyB { get; set; }
        public Vector2 ContactNormal { get; set; }
        public Vector2 ContactPoint { get; set; }
        public float Elasticity { get; set; }

        public bool IgnoreCollision { get; set; }

        public CollisionData()
        {
            BodyA = null;
            BodyB = null;

            ContactNormal = new Vector2();
            ContactPoint = new Vector2(); 

            Elasticity = 0f;        // How 'bouncy' or 'elastic' the collision is

            IgnoreCollision = false;
        }
    }
}
