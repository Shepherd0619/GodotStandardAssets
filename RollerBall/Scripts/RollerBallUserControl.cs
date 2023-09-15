using Godot;
using System;

namespace Godot.StandardAssets.RollerBall
{
    public class RollerBallUserControl : Node
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        private RollerBall ball;
        private Vector3 move;

        private Camera cam;
        private Vector3 camForward;
        private bool jump;

        private Vector3 StartPosition;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // The way we assign variable is different here.
            // In Unity, we assign by drag n' drop.
            // In Godot, we need to assign by coding.
            // But don't worry, GetNode is not like GameObject.Find or something.
            // It is optimized so no need to worry about performance.
            // You can use path or just the name of the object to locate the node.

            ball = GetNode("/root/Root/Ball") as RollerBall;
            cam = GetNode("/root/Root/Camera") as Camera;

            if(ball != null){
                StartPosition = ball.GlobalTransform.origin;
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if(Input.IsActionJustPressed("reset")){
                ball.GlobalTranslation = StartPosition;
                ball.AngularVelocity = Vector3.Zero;
                ball.LinearVelocity = Vector3.Zero;
            }

            // First of all, there is no Horizontal or Vertical Axis in Godot.
            // To be specific, it works like Unreal Engine.
            // So we need to set up couple Actions in Input Settings and make our own axis float.
            float h = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
            float v = Input.GetActionStrength("move_back") - Input.GetActionStrength("move_forward");
            jump = Input.IsActionJustPressed("jump");

            if (cam != null)
            {
                // The way we get a transform and its position, rotation is different here.
                // GlobalTransform means the transform in world space.
                // Transform means the transform in local space.
                // origin represents the position.
                // basis represents the rotation and scale.

                // For more info, I suggest you use a IDE which support decompile to locate the definition and see the logic.

                var CamBasis = cam.GlobalTransform.basis;
                var basis = CamBasis.Rotated(CamBasis.x, -CamBasis.GetEuler().x);
                move = basis.Xform(new Vector3(h, 0, v));
            }
            else
            {
                move = (v * Vector3.Forward + h * Vector3.Right).Normalized();
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            ball.Move(move, jump);
            jump = false;
        }
    }
}
