using System;
using Godot;

public partial class PlayerController : BaseCharacter, ICombat
{
    public bool IsAttacking = false;                    // Indicator for if the player is currently attacking
    public bool CanAttack = true;                       // if the character can attack

    [ExportGroup("Movement Settings")]
    [Export] private float mMovementSpeed;                  // How fast the character moves at
    private bool mCanMove = true;                       // if the character can move
    public bool IsMoving = false;                       // if the character is currently moving
    [Export] private Node3D mMovementDirection;             // Object to set the direction we are moving in

    [ExportGroup("Components")]
    [Export] private AnimationPlayer mAnimPlayer;

    public override void _Ready()
    {
        base._Ready();

        mAnimator = new PlayerAnimator(mAnimPlayer, this);              // Create a new animator
        mInventory = new Inventory(20, mLeftHand, mRightHand, this);            // Create the inventory

        mStats = new CharacterStats("Player");

        // Debug Inventory
        Callable.From(ActorSetup).CallDeferred();   
    }


    public override void _Process(double delta)
    {
        base._Process(delta);

        if(Input.IsActionJustPressed("Inventory"))
        {
            mIsInventoryOpen = !mIsInventoryOpen;
            mInventoryPanel.Visible = mIsInventoryOpen;

            if(mIsInventoryOpen)
            {
                mInventoryPanel.Setup(mInventory);
                Input.MouseMode = Input.MouseModeEnum.Visible;
            } else 
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }

        if(Input.IsActionJustPressed("LightAttack"))
            LightAttack();

        if(Input.IsActionJustPressed("Block"))
            Block();

        if(Input.IsActionJustReleased("Block"))
            StopBlocking();

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        HandleMovement((float)delta);
    }

    private void HandleMovement(float delta)
    {
        Vector3 moveDirection = Vector3.Zero;                       // Initialize thew movement direction for this frame

        if(mCanMove)
        {
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
                if(mAnimator != null)
                {
                    mAnimator.SetBool("IsMoving", true);
                }
            } else 
            {
                IsMoving = false;
                if(mAnimator != null)
                    mAnimator.SetBool("IsMoving", false);
            }
        }

        this.Velocity = moveDirection;                  // apply the movement direction
        MoveAndSlide();
    }

    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        bool added = mInventory.AddItem(GetNode<ItemDatabase>("/root/ItemDatabase").GetRandomWeapon());
        bool addedArmor = mInventory.AddItem(GetNode<ItemDatabase>("/root/ItemDatabase").GetRandomArmor());
    }

    public override void LightAttack()
    {
        if(!CanAttack || IsAttacking)
            return;

        base.LightAttack();
    }

    public override void HeavyAttack()
    {
        if(!CanAttack || IsAttacking)
            return;

        base.HeavyAttack();
    }

    public override void TakeDamage(float dp)
    {
        // TODO: Reduce DP by armor
        base.TakeDamage(dp);
        if(mStats != null && !mStats.IsAlive)
        {
            mCanMove = false;
        }
    }

    public override void Block()
    {
        base.Block();
        mCanMove = false;
    }

    public override void StopBlocking()
    {
        base.StopBlocking();
        if(mAnimator != null && mAnimator is PlayerAnimator pAnim)
            pAnim.ResumeBlockAnim();

        mCanMove = true;
    }
}