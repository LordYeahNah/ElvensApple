using System;
using System.Diagnostics;
using Godot;

public partial class PlayerController : BaseCharacter, ICombat
{
    public bool CanAttack = true;                       // if the character can attack

    [ExportGroup("Movement Settings")]
    [Export] private float mMovementSpeed;                  // How fast the character moves at
    private bool mCanMove = true;                       // if the character can move
    public bool IsMoving = false;                       // if the character is currently moving
    [Export] private Node3D mMovementDirection;             // Object to set the direction we are moving in
    public float SpeedModifier = 1.0f;

    [ExportGroup("Jump Settings")] 
    [Export] private float mGravity = -9.81f;
    [Export] private float mJumpForce;
    [Export] private float mAirMovementForce;                   // Fast the character can move when not grounded

    [ExportGroup("Components")]
    [Export] private AnimationPlayer mAnimPlayer;
    [Export] private DialogBoxController mDialogBox;                        // Reference to the dialog box
    [Export] private RayCast3D mInteractionCast;                            // Reference to the interaction raycast
    [Export] private Label mInteractionLabel;                       // Reference to the indicator for interaction
    [Export] private RayCast3D mGroundCast;

    [ExportGroup("Stat Components")]
    [Export] private TextureProgressBar mHealthBar;
    [Export] private TextureProgressBar mManaBar;
    [Export] private Control mStatBarContainer;

    // === INTERACTION === //
    private bool mIsInInteraction = false;                       // if the character is currently interacting with something
    private FriendlyController mInteractingWith;                    // The character that we are interacting with
    public override void _Ready()
    {
        base._Ready();

        mAnimator = new PlayerAnimator(mAnimPlayer, this);              // Create a new animator
        mInventory = new Inventory(20, mLeftHand, mRightHand, this);            // Create the inventory

        mStats = new CharacterStats("Player");
        mStats.ManaIncrease += UpdateManaBar;

        mHealthBar.Value = mStats.CurrentHealth;
        mManaBar.Value = mStats.CurrentMana;
        
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
            mStatBarContainer.Visible = !mIsInventoryOpen;

            if(mIsInventoryOpen)
            {
                mInventoryPanel.Setup(mInventory);
                Input.MouseMode = Input.MouseModeEnum.Visible;
            } else 
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }

        if (Input.IsActionJustPressed("Spells"))
        {
            mSpellsIsOpen = !mSpellsIsOpen;
            mSpellInventory.Visible = mSpellsIsOpen;
            if (mSpellsIsOpen)
            {
                mSpellInventory.Setup();
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else
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

        if (Input.IsActionJustPressed("SpellOne"))
        {
            UseMagic(1);
        }

        if (Input.IsActionJustPressed("SpellTwo"))
        {
            UseMagic(2);
        }

        if (!mIsInInteraction)
        {
            if (mInteractionCast.IsColliding())
            {
                GodotObject collision = mInteractionCast.GetCollider();
                if (collision is FriendlyController friend)
                {
                    mInteractionLabel.Visible = true;
                    if (friend.CanInteract)
                    {
                        if (Input.IsActionJustPressed("Interact"))
                        {
                            mInteractingWith = friend;
                            friend.InteractWith(this);
                        }
                    }
                }
                else
                {
                    mInteractionLabel.Visible = false;
                }
            }
            else
            {
                mInteractionLabel.Visible = false;
            }
        }
        else
        {
            mInteractionLabel.Visible = false;
            if (mInteractingWith != null)
            {
                if (Input.IsActionJustPressed("ProgressDialog"))
                {
                    if (mDialogBox.IsTyping)
                    {
                        mDialogBox.CompleteMessage();
                    }
                    else
                    {
                        if (mInteractingWith.FinishDialog())
                        {
                            mInteractingWith = null;
                            mIsInInteraction = false;
                        }
                    }
                    
                }
            }
        }

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
                Vector3 forward;
                Vector3 right;
                if (IsGrounded())
                {
                    // Calculate movement directions
                    forward = this.Transform.Basis.X * (movementInput.Y * (mMovementSpeed * SpeedModifier));
                    right = this.Transform.Basis.Z * (movementInput.X * (mMovementSpeed * SpeedModifier));
                }
                else
                {
                    forward = this.Transform.Basis.X * (movementInput.Y * mAirMovementForce);
                    right = this.Transform.Basis.Y * (movementInput.X * mAirMovementForce);
                }

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

        if (IsGrounded() && Input.IsActionJustPressed("Jump"))
        {
            moveDirection.Y = mJumpForce * delta;
        }

        if (!IsGrounded())
        {
            moveDirection.Y += mGravity * delta;
        }

        if (IsGrounded())
        {
            this.Velocity = moveDirection;                  // apply the movement direction
        }
        else
        {
            this.Velocity += moveDirection;
        }
        
        MoveAndSlide();
    }

    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        bool added = mInventory.AddItem(GetNode<ItemDatabase>("/root/ItemDatabase").GetRandomWeapon());
        bool addedArmor = mInventory.AddItem(GetNode<ItemDatabase>("/root/ItemDatabase").GetRandomArmor());
        
        HealSpell heal = new HealSpell();
        if (heal != null)
        {
            mInventory.SpellsObtained.Add(heal);
        }
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

    public override void TakeDamage(float dp, bool armorReduction = true)
    {
        // TODO: Reduce DP by armor
        base.TakeDamage(dp);
        if(mStats != null && !mStats.IsAlive)
        {
            mCanMove = false;
        }

        mHealthBar.Value = mStats.CurrentHealth;
    }

    public override void UseMagic(int slot)
    {
        base.UseMagic(slot);

        if (mStats != null && mManaBar != null)
        {
            mManaBar.Value = mStats.CurrentMana;
        }
    }

    public void UpdateManaBar()
    {
        if (mStats != null && mManaBar != null)
        {
            mManaBar.Value = mStats.CurrentMana;
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

    /// <summary>
    /// Checks if the character is grounded
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded() => mGroundCast.IsColliding();

    /// <summary>
    /// Displays the dialog box with the selected dialog
    /// </summary>
    /// <param name="mDialogData"></param>
    public void ShowDialogBox(DialogData mDialogData)
    {
        mDialogBox.Visible = true;
        mDialogBox.Setup(mDialogData);
        mIsInInteraction = true;
    }

    /// <summary>
    /// Closes the dialog box
    /// </summary>
    public void CloseDialogBox()
    {
        mDialogBox.Visible = false;
        mIsInInteraction = false;
    }
}