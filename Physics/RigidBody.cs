using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class RigidBody
    {
        public Vector2 Position { get; set; }
        public Vector2 Force { get; set; }
        public Vector2 LinearVelocity { get; set; }

        public Collider CollidableObject { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float Mass { get; set; }
        
        public bool IsDynamic { get; set; }
        public bool IgnoreGravity { get; set; }
        public bool AffectedByPhysics { get; set; }

        public RigidBody()
        {
            Position = new Vector2();
            Force = new Vector2();
            LinearVelocity = new Vector2();

            Mass = 10.0f;

            IgnoreGravity = false;
            IsDynamic = true;
            AffectedByPhysics = true;
        }

        public void Update(float dt, PhysicsManager physManager, PhysObject po)
        {
            // Apply friction if the body is not in the air
            if (!po.IsInTheAir && AffectedByPhysics)
            {
                ApplyFriction(physManager, po);
            }

            Vector2 acceleration = Force * (1 / Mass);

            if (IgnoreGravity || !AffectedByPhysics)
            {
                LinearVelocity += (acceleration * dt);
            }
            else
            {
                LinearVelocity += (physManager.Gravity + acceleration) * dt;
            }

            // If projectile, update it
            if (po is Projectile)
            {
                Projectile projectile = (Projectile)po;

                projectile.Update();
            }

            // If pendulum, update it
            if (po is Pendulum)
            {
                Pendulum pendulum = (Pendulum)po;

                pendulum.Update(physManager, dt);
            }

            // If moving platform, update it
            if (po is MovingPlatform)
            {
                MovingPlatform movingPlatform = (MovingPlatform)po;

                movingPlatform.Update();
            }

            // Update the time since last collision
            // Don't care when moving platform or pendulum last collided
            if (!(po is MovingPlatform) && !(po is Pendulum))
            {
                po.TimeSinceLastCollisionWithFloor++;
                po.TimeSinceLastCollision++;

                // Determine if the body is in the air or not
                if (po.TimeSinceLastCollisionWithFloor < 50 && AffectedByPhysics)
                {
                    po.IsInTheAir = false;
                }
                else
                {
                    po.IsInTheAir = true;
                }
            }

            // Ensure things aren't moving too fast
            MaximumVelocityCheck(physManager, po);

            if ((Math.Abs(LinearVelocity.X) > 0 || Math.Abs(LinearVelocity.Y) > 0) && AffectedByPhysics)
            {            
                // If there's a body on top of the current rigid body
                for (int i = 0; i < po.BodiesOnTop.Count; i++)
                {
                    // Check if the body is still on top
                    if (po.BodiesOnTop[i].TimeSinceLastCollision < 50)
                    {
                        // If this is a moving platform, move the object on top with this body
                        if (po is MovingPlatform)
                        {
                            po.BodiesOnTop[i].RigidBody.Position += (LinearVelocity * dt);

                            // If there is a stack of bodies on top of the moving platform
                            for (int j = 0; j < po.BodiesOnTop[i].BodiesOnTop.Count; j++)
                            {
                                if (!po.BodiesOnTop.Contains(po.BodiesOnTop[i].BodiesOnTop[j]))
                                {
                                    // Add each unique body to the list of bodies on the platform
                                    po.BodiesOnTop.Add(po.BodiesOnTop[i].BodiesOnTop[j]);
                                }
                            }
                        }
                    }
                    else
                    {
                        po.BodiesOnTop.Remove(po.BodiesOnTop[i]);
                    }
                }
            }

            // Reset the force
            Force = new Vector2(0);

            Position += (LinearVelocity * dt);
        }

        // Friction only applied if the body is not in the air
        private void ApplyFriction(PhysicsManager world, PhysObject po)
        {
            Vector2 friction = new Vector2(0);

            if (LinearVelocity.X == 0)
            {
                float staticFrictionCoefficient = 0.7f;
                friction = world.Gravity * staticFrictionCoefficient;
            }
            else
            {
                float kineticFrictionCoefficient = po.FrictionCoefficient;
                friction = -1 * kineticFrictionCoefficient * LinearVelocity;
            }

            Force += new Vector2(friction.X * 200, 0);

            // If the horizontal speed is very small, just set it to 0
            if (Math.Abs(LinearVelocity.X) < 0.01)
            {
                LinearVelocity = new Vector2(0, LinearVelocity.Y);
            }
        }

        // If any objects are moving faster than the maximum allowed speed,
        // sets their speed to the maximum allowed speed
        private void MaximumVelocityCheck(PhysicsManager world, PhysObject po)
        {
            // Not a projectile
            if (!(po is Projectile))
            {
                if (LinearVelocity.X > world.MaximumHorizontalSpeed)
                {
                    LinearVelocity = new Vector2(world.MaximumHorizontalSpeed, LinearVelocity.Y);
                }

                if (LinearVelocity.X < (world.MaximumHorizontalSpeed * -1))
                {
                    LinearVelocity = new Vector2(world.MaximumHorizontalSpeed * -1, LinearVelocity.Y);
                }

                if (LinearVelocity.Y > world.MaximumVerticalSpeed)
                {
                    LinearVelocity = new Vector2(LinearVelocity.X, world.MaximumVerticalSpeed);
                }

                if (LinearVelocity.Y < (world.MaximumVerticalSpeed * -1))
                {
                    LinearVelocity = new Vector2(LinearVelocity.X, world.MaximumVerticalSpeed * -1);
                }
            }
        }
    }
}
