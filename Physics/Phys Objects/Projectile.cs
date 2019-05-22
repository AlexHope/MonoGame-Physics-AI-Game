using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class Projectile : PhysObject
    {
        public float NumOfBounces { get; set; }
        public float MaxNumberOfBounces { get; set; }

        public enum ProjectileType { Ball, Bullet }
        public ProjectileType Type { get; set; }

        public Projectile()
        {
            Position = new Vector2();

            Elasticity = 0;
            NumOfBounces = 0;
            MaxNumberOfBounces = 6;

            Type = ProjectileType.Ball;
        }

        public Projectile(Vector2 position, Vector2 velocityNormalised, float elasticity, ProjectileType projectileType)
        {
            Position = position;
            RigidBody.Position = Position;
            RigidBody.AffectedByPhysics = false;
            RigidBody.IgnoreGravity = true;

            Elasticity = elasticity;
            NumOfBounces = 0;
            Type = projectileType;


            if (Type == ProjectileType.Ball)
            {
                MaxNumberOfBounces = 6;

                RigidBody.LinearVelocity = velocityNormalised * 800;
                RigidBody.Mass = 7.5f;
                RigidBody.Width = 16;
                RigidBody.Height = 16;
                RigidBody.CollidableObject = new BoxCollider();
                RigidBody.CollidableObject.IsCircle = true;

                // Textures in MonoGame start in the top left corner
                TexturePos = new Vector2(Position.X - RigidBody.Width / 2, Position.Y - RigidBody.Height / 2);
            }
            else if (Type == ProjectileType.Bullet)
            {
                MaxNumberOfBounces = 1;

                RigidBody.LinearVelocity = velocityNormalised * 800;
                RigidBody.Mass = 5.0f;
                RigidBody.Width = 10;
                RigidBody.Height = 10;
                RigidBody.CollidableObject = new BoxCollider();
                RigidBody.CollidableObject.IsCircle = true;

                // Textures in MonoGame start in the top left corner
                TexturePos = new Vector2(Position.X - RigidBody.Width / 2, Position.Y - RigidBody.Height / 2);
            }
        }

        public void Update()
        {
            // If the projectile has collided the same or more than its maximum allowed collisions, 
            // or if it hasn't collided with anything for a long time, mark the projectile for deletion
            if (NumOfBounces >= MaxNumberOfBounces || TimeSinceLastCollision > 3000)
            {
                MarkedForDeletion = true;
            }
        }
    }
}
