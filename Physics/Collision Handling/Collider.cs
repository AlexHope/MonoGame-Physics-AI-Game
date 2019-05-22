using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    // Taken and modified from the code provided on Course Resources
    abstract class Collider
    {
        protected AABB aabb = null;

        public bool IsCircle { get; set; }

        public abstract AABB CalculateAABB(RigidBody body);
    }
}
