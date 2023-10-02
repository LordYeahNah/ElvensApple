using System;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    private PlayerAnimator mAnimator;

    [ExportGroup("Movement Settings")]
    [Export] private float mMovementSpeed;                  // How fast the character moves at
    private bool mCanMove = true;                       // if the character can move
    public bool IsMoving = false;                       // if the character is currently moving
    [Export] private Node3D mMovementDirection;             // Object to set the direction we are moving in

    public override void _Ready()
    {
        base._Ready();

        AnimationPlayer anim = GetNode<AnimationPlayer>("Character/Animated/AnimationPlayer");
        if(anim != null)
            mAnimator = new PlayerAnimator(anim);
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        HandleMovement((float)delta);
    }

    private void HandleMovement(float delta)
    {
        Vector3 moveDirection = Vector3.Zero;                       // Initialize the movement direction for this frame

        // Get the movement axis
        Vector2 movementInput = Input.GetVector("MoveLeft", "MoveRight", "MoveBack", "MoveForward");
        
        // Check that we are moving
        if(movementInput.Length() > 0.1)
        {
            // Calculate movement directions
            Vector3 forward = this.Transform.Basis.X * (movementInput.Y * mMovementSpeed);
            Vector3 right = this.Transform.Basis.Z * (movementInput.X * mMovementSpeed);

            // Apply the movement
            moveDirection = (forward + right) * delta;
            IsMoving = true;
        } else 
        {
            IsMoving = false;
        }

        this.Velocity = moveDirection;                  // apply the movement direction
        MoveAndSlide();
    }

}