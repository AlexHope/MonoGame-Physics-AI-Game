using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Physics
{
    class Character : PhysObject
    {
        public bool IsPlayer { get; set; }
        public int BallAmmo { get; set; }
        public int MaximumBallAmmo { get; set; }
        public AI.AIBehaviour Behaviour { get; set; }
        public float JumpCooldownRemaining { get; set; } // Used for the AI

        private const int _forceMultiplier = 10000;

        public Character(bool isPlayer)
        {
            MaximumBallAmmo = 20;
            BallAmmo = MaximumBallAmmo;
            IsPlayer = isPlayer;
        }

        #region Movement
        public void MoveLeft()
        {
            RigidBody.Force = new Vector2(-20 * _forceMultiplier, RigidBody.Force.Y);
        }

        public void MoveRight()
        {
            RigidBody.Force = new Vector2(20 * _forceMultiplier, RigidBody.Force.Y);
        }

        // Forces the character's horizontal velocity to 0
        public void Stop()
        {
            RigidBody.LinearVelocity = new Vector2(0, RigidBody.LinearVelocity.Y);
        }

        public void Jump()
        {
            if (IsPlayer)
            {
                if (!IsInTheAir)
                {
                    RigidBody.Force = new Vector2(RigidBody.Force.X, -600 * _forceMultiplier);
                }
            }
            else
            {
                // Needed to resolve FSM issues (otherwise the AI can spam jump)
                if (!IsInTheAir && JumpCooldownRemaining <= 0)
                {
                    RigidBody.Force = new Vector2(RigidBody.Force.X, -600 * _forceMultiplier);

                    JumpCooldownRemaining = 0.1f;
                }
            }
        }
        #endregion

        // Fires a projectile toward the cursor position
        public void Attack(PhysicsManager physManager, Vector2 cursorPos, Projectile.ProjectileType projectileType)
        {
            Vector2 directionToFire = new Vector2(cursorPos.X - RigidBody.Position.X, cursorPos.Y - RigidBody.Position.Y);
            directionToFire = Vector2.Normalize(directionToFire);

            if (projectileType == Projectile.ProjectileType.Bullet)
            {
                Projectile projectile = new Projectile(RigidBody.Position, directionToFire, 1, projectileType);
                physManager.DynamicObjects.Add(projectile);
                physManager.AllPhysObjects.Add(projectile);
            }

            if (projectileType == Projectile.ProjectileType.Ball && BallAmmo > 0)
            {
                Projectile projectile = new Projectile(RigidBody.Position, directionToFire, 1, projectileType);
                physManager.DynamicObjects.Add(projectile);
                physManager.AllPhysObjects.Add(projectile);
                BallAmmo--;
            }
        }
    }
}
