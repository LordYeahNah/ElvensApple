using System;
using Godot;

public partial class CameraController : Node3D
{
    [ExportGroup("Look Settings")]
    [Export] private float mLookSensitivity;
    [Export] private float mLookClampMin;
    [Export] private float mLookClampMax;
    
    [ExportGroup("Player Objects")]
    [Export] private PlayerController mCharacterObject;                   // Reference to the character object
    [Export] private Node3D mMovementDirectionObject;           // Reference to the movement direction
    

    private Vector2 mMouseDelta;

    public override void _Ready()
    {
        base._Ready();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        if (mCharacterObject.IsInventoryOpen || mCharacterObject.IsSpellOpen || mCharacterObject.IsInInteraction) 
            return;
        
        base._Process(delta);
        HandleRotation((float)delta);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(@event is InputEventMouseMotion e)
        {
            mMouseDelta = e.Relative;
        }


    }

    private void HandleRotation(float delta)
    {
        // Get the rotation input
        // Determine the rotation on the X
        float desiredRotationX = this.RotationDegrees.Z - mMouseDelta.Y;  
        
        float desiredrotationY = this.RotationDegrees.Y - mMouseDelta.X;
        desiredRotationX = Mathf.Clamp(desiredRotationX, mLookClampMin, mLookClampMax);

        
        //this.RotateZ(Mathf.DegToRad(desiredrotationY * (mLookSensitivity * delta)));
        //this.RotateY(Mathf.DegToRad(desiredRotationX * (mLookSensitivity * delta)));

        this.Rotation = new Vector3(0f, 0f, Mathf.DegToRad(desiredRotationX));
        

        // Rotate the character object
        if(mCharacterObject != null)
        {
            mCharacterObject.RotateY(Mathf.DegToRad(desiredrotationY));
        }


        mMouseDelta = Vector2.Zero;
    }
}