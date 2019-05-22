using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class MovingPlatform : PhysObject
    {
        public Vector2 StartPoint { get; set; }
        public Vector2 EndPoint { get; set; }

        public MovingPlatform(Vector2 position, Vector2 startPoint, Vector2 endPoint, Vector2 velocity)
        {
            Position = position;
            RigidBody.Position = Position;
            RigidBody.LinearVelocity = velocity;
            RigidBody.IsDynamic = false;       // Technically it is dynamic, but it needs to behave as static
            RigidBody.IgnoreGravity = true;

            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public void Update()
        {
            // Check moving platforms position & invert speed if necessary
            if (RigidBody.Position.X < StartPoint.X ||
                RigidBody.Position.X > EndPoint.X)
            {
                RigidBody.LinearVelocity = new Vector2(RigidBody.LinearVelocity.X * -1, RigidBody.LinearVelocity.Y);
            }
        }
    }
}
