using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Physics
{
    class CollisionHandler
    {
        private List<CollisionData> _collisionData;
        public General.World World { get; set; }

        public CollisionHandler()
        {
            _collisionData = new List<CollisionData>();
        }

        public void AddCollision(CollisionData collision)
        {
            RigidBody bodyA = collision.BodyA.RigidBody;
            RigidBody bodyB = collision.BodyB.RigidBody;

            // Check that the colliding objects are not one and the same
            foreach (CollisionData c in _collisionData)
            {
                if ((Object.ReferenceEquals(bodyA, c.BodyA.RigidBody) && Object.ReferenceEquals(bodyB, c.BodyB.RigidBody)) ||
                    (Object.ReferenceEquals(bodyA, c.BodyB.RigidBody) && Object.ReferenceEquals(bodyB, c.BodyA.RigidBody)))
                {
                    return;
                }
            }
            _collisionData.Add(collision);
        }

        public void ResolveCollision(CollisionData collision)
        {
            // Projectiles shouldn't collide with eachother
            if (collision.BodyA is Projectile && collision.BodyB is Projectile)
            {
                return;
            }

            // Characters shouldn't collide with eachother
            if (collision.BodyA is Character && collision.BodyB is Character && collision.IgnoreCollision)
            {
                return;
            }

            // Check what has collided with the target
            if (collision.BodyB.IsTarget || (collision.BodyA.IsTarget && collision.BodyB.RigidBody.IsDynamic))
            {
                if (collision.BodyB is Character)
                {
                    Character c = (Character)collision.BodyB;

                    // If not a player, then must be an AI hitting the target, so delete the AI and lower the portal health
                    if (!c.IsPlayer)
                    {
                        collision.BodyB.MarkedForDeletion = true;
                        World.PortalHealth -= 10;
                    }
                }
                else if (collision.BodyA is Character)
                {
                    Character c = (Character)collision.BodyA;

                    // If not a player, then must be an AI hitting the target, so delete the AI and lower the portal health
                    if (!c.IsPlayer)
                    {
                        collision.BodyA.MarkedForDeletion = true;
                        World.PortalHealth -= 10;
                    }
                }
                return;
            }

            collision.ContactNormal = Vector2.Normalize(collision.ContactNormal);
            RigidBody rb_A = collision.BodyA.RigidBody;
            RigidBody rb_B = collision.BodyB.RigidBody;

            // Determine if the collision involves a projectile
            if (collision.BodyA is Projectile)
            {
                if (collision.BodyB is Character)
                {
                    Character c = (Character)collision.BodyB;

                    // Check if the target hit is not the player
                    if (!c.IsPlayer)
                    {
                        World.Player.BallAmmo++;

                        if (World.Player.BallAmmo > World.Player.MaximumBallAmmo)
                        {
                            World.Player.BallAmmo = World.Player.MaximumBallAmmo;
                        }
                    }
                    else
                    {
                        return;
                    }

                    if (!World.ProjectileCharacterCollision)
                    {
                        if (!c.IsPlayer)
                        {
                            collision.BodyB.MarkedForDeletion = true;
                        }
                        return;
                    }
                }
                collision.Elasticity = collision.BodyA.Elasticity;

                Projectile projectile = (Projectile)collision.BodyA;
                projectile.NumOfBounces++;
            }
            else if (collision.BodyB is Projectile)
            {
                if (collision.BodyA is Character)
                {
                    Character c = (Character)collision.BodyA;

                    // Check if the target hit is not the player
                    if (!c.IsPlayer)
                    {
                        World.Player.BallAmmo++;

                        if (World.Player.BallAmmo > World.Player.MaximumBallAmmo)
                        {
                            World.Player.BallAmmo = World.Player.MaximumBallAmmo;
                        }
                    }
                    else
                    {
                        return;
                    }

                    if (!World.ProjectileCharacterCollision)
                    {
                        if (!c.IsPlayer)
                        {
                            collision.BodyA.MarkedForDeletion = true;
                        }
                        return;
                    }
                }
                collision.Elasticity = collision.BodyB.Elasticity;

                Projectile projectile = (Projectile)collision.BodyB;
                projectile.NumOfBounces++;
            }
            else if (collision.BodyA.IsTarget)
            {
                collision.Elasticity = collision.BodyA.Elasticity;
            }
            else
            {
                collision.Elasticity = collision.BodyB.Elasticity;
            }

            // If body A is sitting on top of body B
            if (collision.BodyA is Character && collision.ContactNormal.Y == -1)
            {
                if (!collision.BodyB.BodiesOnTop.Contains(collision.BodyA))
                {
                    collision.BodyB.BodiesOnTop.Add(collision.BodyA);
                }
            }

            #region Displacement Method

            Vector2 displacement = new Vector2();
            Vector2 queryPoint = new Vector2();
            queryPoint = rb_A.Position;

            float d = -(Vector2.Dot(collision.ContactNormal, collision.ContactPoint));
            float penetrationDepth = Vector2.Dot(queryPoint, collision.ContactNormal) + d;
            displacement = (collision.ContactNormal * penetrationDepth) * -1;

            if (rb_A.IsDynamic)
            {
                rb_A.Position -= displacement;
            }
            if (rb_B.IsDynamic)
            {
                rb_B.Position += displacement;
            }
            #endregion
            
            #region Impulse Method

            Vector2 relativeVelocity = rb_A.LinearVelocity - rb_B.LinearVelocity;
            Vector2 bodyAInitialVelocity = new Vector2(rb_A.LinearVelocity.X, rb_A.LinearVelocity.Y);
            Vector2 bodyBInitialVelocity = new Vector2(rb_B.LinearVelocity.X, rb_B.LinearVelocity.Y);
            // 1 for perfectly elastic, 0 for perfectly inelastic
            float impulse;

            if (!rb_A.IsDynamic)
            {
                impulse = ((-(1 + collision.Elasticity)) * (Vector2.Dot(relativeVelocity, collision.ContactNormal))) / ((1 / rb_B.Mass));

                if (!(collision.BodyA is MovingPlatform))
                {
                    rb_A.LinearVelocity = new Vector2(0, 0);
                }

                rb_B.LinearVelocity = bodyBInitialVelocity - collision.ContactNormal * (impulse * (1 / rb_B.Mass));
            }
            else if (!rb_B.IsDynamic)
            {
                impulse = ((-(1 + collision.Elasticity)) * (Vector2.Dot(relativeVelocity, collision.ContactNormal))) / ((1 / rb_A.Mass));

                if (!(collision.BodyB is MovingPlatform))
                {
                    rb_B.LinearVelocity = new Vector2(0, 0);
                }

                rb_A.LinearVelocity = bodyAInitialVelocity + collision.ContactNormal * (impulse * (1 / rb_A.Mass));
            }
            else
            {
                impulse = ((-(1 + collision.Elasticity)) * (Vector2.Dot(relativeVelocity, collision.ContactNormal))) / ((1 / rb_A.Mass) + (1 / rb_B.Mass));
                rb_A.LinearVelocity = bodyAInitialVelocity + collision.ContactNormal * (impulse * (1 / rb_A.Mass));
                rb_B.LinearVelocity = bodyBInitialVelocity - collision.ContactNormal * (impulse * (1 / rb_B.Mass));
            }
            #endregion
        }

        public void ResolveAllCollisions()
        {
            foreach (CollisionData c in _collisionData)
            {
                ResolveCollision(c);
            }
            _collisionData.Clear();
        }
    }
}
