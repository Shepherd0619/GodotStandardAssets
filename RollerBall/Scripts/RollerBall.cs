using Godot;
using System;

namespace Godot.StandardAssets.RollerBall
{
	public partial class RollerBall : RigidBody3D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		private float m_MovePower = 5;
		private bool m_UseTorque = true;
		private float m_MaxAngularVelocity = 25;
		private float m_JumpPower = 10;
		private RayCast3D rayCast;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// The way to set up Raycast is different here.
			// You need to assign a Node called RayCast in the Scene View.
			rayCast = (RayCast3D)GetNode("GroundRayCast");
			rayCast.TopLevel = true;

			// In the original Standard Assets, Unity use this to set the maximum angular velocity.
			// GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
			// Unfortunately, there is no maxAngularVelocity in Godot.
			// Maybe someone know another workaround?
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			rayCast.GlobalPosition = GlobalTransform.Origin;
		}

		public override void _PhysicsProcess(double delta)
		{
			//this.AngularVelocity = new Vector3(Mathf.Min(this.AngularVelocity.X, m_MaxAngularVelocity), Mathf.Min(this.AngularVelocity.Y, m_MaxAngularVelocity), Mathf.Min(this.AngularVelocity.Z, m_MaxAngularVelocity));
			Vector3 angularVel = AngularVelocity;
			if(AngularVelocity.Length() > m_MaxAngularVelocity){
				AngularVelocity = AngularVelocity.Normalized() * m_MaxAngularVelocity;
			}

			//AngularVelocity = angularVel;
			
		}

		public void Move(Vector3 moveDirection, bool jump)
		{
			// If using torque to rotate the ball...
			if (m_UseTorque)
			{
				// ... add torque around the axis defined by the move direction.
				ApplyTorque(new Vector3(moveDirection.Z, 0, -moveDirection.X) * m_MovePower);
			}
			else
			{
				// Otherwise add force in the move direction.
				ApplyCentralImpulse(moveDirection * m_MovePower);
			}

			// If on the ground and jump is pressed...
			if (rayCast.IsColliding() && jump)
			{
				// ... add force in upwards.
				ApplyCentralImpulse(Vector3.Up * m_JumpPower);
			}
		}
	}
}
