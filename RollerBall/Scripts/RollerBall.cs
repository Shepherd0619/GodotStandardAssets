using Godot;
using System;

namespace Godot.StandardAssets.RollerBall
{
    public class RollerBall : RigidBody
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        private float m_MovePower = 5;
        private bool m_UseTorque = true;
        private float m_MaxAngularVelocity = 25;
        private float m_JumpPower = 10;
        private RayCast rayCast;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            rayCast = (RayCast)GetNode("GroundRayCast");
            rayCast.SetAsToplevel(true);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            rayCast.GlobalTranslation = GlobalTransform.origin;
        }

        public override void _PhysicsProcess(float delta)
        {
            //this.AngularVelocity = new Vector3(Mathf.Min(this.AngularVelocity.x, m_MaxAngularVelocity), Mathf.Min(this.AngularVelocity.y, m_MaxAngularVelocity), Mathf.Min(this.AngularVelocity.z, m_MaxAngularVelocity));
        }

        public void Move(Vector3 moveDirection, bool jump)
        {
            if (m_UseTorque)
            {
                this.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * m_MovePower);
            }
            else
            {
                ApplyCentralImpulse(moveDirection * m_MovePower);
            }

            if (rayCast.IsColliding() && jump)
            {
                ApplyCentralImpulse(Vector3.Up * m_JumpPower);
            }
        }
    }
}
