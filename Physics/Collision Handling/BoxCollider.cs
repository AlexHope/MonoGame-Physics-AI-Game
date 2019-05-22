using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class BoxCollider : Collider
    {
        public override AABB CalculateAABB(RigidBody body)
        {
            aabb = new AABB();

            aabb.Min = new Vector2(body.Position.X - (body.Width / 2), body.Position.Y - (body.Height / 2));    // Top left
            aabb.Max = new Vector2(body.Position.X + (body.Width / 2), body.Position.Y + (body.Height / 2));    // Bottom right

            return aabb;
        }        
    }
}
