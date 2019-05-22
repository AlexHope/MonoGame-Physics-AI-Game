using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Physics
{
    class Pendulum : PhysObject
    {
        public Vector2 AnchorPosition { get; set; }
        public Vector2 Length { get; set; }

        private float _angularVelocity;

        public Pendulum(Vector2 anchorPosition, Vector2 length)
        {
            Position = anchorPosition + length;
            RigidBody.Position = Position;
            AnchorPosition = anchorPosition;
            _angularVelocity = 0;

            Elasticity = 1.0f;
            Length = length;

            RigidBody.Mass = 100.0f;
            RigidBody.Width = 32.0f;
            RigidBody.Height = 32.0f;
            RigidBody.CollidableObject = new BoxCollider();
            RigidBody.CollidableObject.IsCircle = true;

            TexturePos = new Vector2(Position.X - RigidBody.Width / 2, Position.Y - RigidBody.Height / 2);
        }

        public void Update(PhysicsManager physManager, float dt)
        {
            // Vector from cart to pendulum
            Vector3 pendulumRod = new Vector3(AnchorPosition - RigidBody.Position, 0);

            // Torque = r X F = rod length X gravity
            float torque = Vector3.Cross(pendulumRod, new Vector3(physManager.Gravity, 0)).Length();

            // Angular Acceleration = torque / inertia (approximated to mass here)
            float angularAcceleration = torque / RigidBody.Mass;

            // Velocity = Acceleration x Time
            _angularVelocity += angularAcceleration * dt;

            // Don't let it spin too fast
            if (_angularVelocity > 0.4)
            {
                _angularVelocity = 0.4f;
            }

            // Angle of rotation
            float dtheta = _angularVelocity / pendulumRod.Length();

            // Set the rotation
            General.Matrix2 rotation = new General.Matrix2();
            rotation.SetRotation(dtheta);

            // Adjust positions
            Length = rotation * Length;
            RigidBody.Position = AnchorPosition + Length;
            Position = RigidBody.Position;
        }
    }
}
