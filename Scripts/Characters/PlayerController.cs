using System;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    [ExportGroup("Movement Settings")]
    [Export] private float mMovementSpeed;                  // How fast the character moves at
    private bool mCanMove = true;                       // if the character can move

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
            Vector3 forward = this.Transform.Basis.X * (moveDirection.Y * mMovementSpeed);
            Vector3 right = this.Transform.Basis.Z * (moveDirection.X * mMovementSpeed);

            // Apply the movement
            moveDirection = (forward + right) * delta;
        }

        this.Velocity = moveDirection;                  // apply the movement direction
    }

}