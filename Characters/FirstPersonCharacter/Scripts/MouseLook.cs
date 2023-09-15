using Godot;
using System;

public class MouseLook : Camera
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;


    private Vector3 m_CharacterTargetRot;
    private Vector3 m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public void Init(Spatial character, Camera camera)
    {
        m_CharacterTargetRot = character.Rotation;
        m_CameraTargetRot = camera.Rotation;
    }

    // WARNING: Time.deltaTime does not exist in Godot Engine.
    // But we can still get a similar value in the _Process/_PhysicsProcess's parameter called delta.
    // So the way we call this LookRotation function will be a little different from the way we do in Unity Engine.
    public void LookRotation(Spatial character, Camera camera, float delta)
    {
        float yRot = (Input.GetActionStrength("view_left") - Input.GetActionStrength("view_right")) * XSensitivity;
        float xRot = (Input.GetActionStrength("view_up") - Input.GetActionStrength("view_down")) * YSensitivity;

        m_CharacterTargetRot *= new Vector3(0f, yRot, 0f);
        m_CameraTargetRot *= new Vector3(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            character.Rotation = character.Rotation.Slerp(m_CharacterTargetRot,
                smoothTime * delta);
            camera.Rotation = camera.Rotation.Slerp(m_CameraTargetRot,
                smoothTime * delta);
        }
        else
        {
            character.Rotation = m_CharacterTargetRot;
            camera.Rotation = m_CameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.IsKeyPressed((int)Godot.KeyList.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.IsMouseButtonPressed(1))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        else if (!m_cursorIsLocked)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }

    Vector3 ClampRotationAroundXAxis(Vector3 v)
    {
        float angleX = Mathf.Clamp(v.x, MinimumX, MaximumX);

        return new Vector3(angleX, v.y, v.z);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
