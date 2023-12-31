using System;
using System.Collections.Generic;
using Godot;

public enum EActionDecision
{
    BLOCK = 0,
    ATTACK = 1,
}

public enum EMovementState
{
    IDLE = 0,
    WANDERING = 1,
    TARGET = 2,
}

public partial class BaseAI : BaseCharacter, ICombat
{

    [ExportGroup("Movement Settings")]
    [Export] protected NavigationAgent3D mAgent;
    [Export] protected float mMovementSpeed;
    protected Vector3 mTargetPosition;
    public bool CanMove = false;
    [Export] private EMovementState mMovementState;

    // Reference to AI Components
    protected Blackboard mBlackboard;
    protected BehaviorTree mTree;

    [ExportGroup("Components")]
    [Export] protected AnimationPlayer mAnimPlayer;

    [ExportGroup("Inventory")]
    [Export] private bool mRandomInventory;
    [Export] private string mWeaponName;
 
    public override void _Ready()
    {
        base._Ready();
        
        CreateAI();
        if(mTree != null)
            mTree.OnInitialize(mBlackboard);

        mInventory = new Inventory(5, mLeftHand, mRightHand, this);
        
        Callable.From(ActorSetup).CallDeferred();
    }

    /// <summary>
    /// Creates the properties for the AI
    /// </summary>
    protected virtual void CreateAI()
    {
        CreateBlackboard();
    }

    /// <summary>
    /// Handles generating the inventory on spawn
    /// </summary>
    protected void CreateInventory()
    {
        if(!mRandomInventory)
            return;

        Weapon weapon = (Weapon)GetNode<ItemDatabase>("/root/ItemDatabase").GetRandomWeapon();
        if(mInventory.AddItem(weapon))
        {
            mInventory.EquipItem(weapon, EAttachmentHand.RIGHT);
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // Update the behaviour tree
        if(mTree != null)
            mTree.OnUpdate((float)delta);

        HandleLookAt((float)delta);

        
    }

    protected void HandleLookAt(float delta)
    {
        Vector3 lookAt = Vector3.Zero;
        if(mBlackboard.GetValue<BaseCharacter>("Target") != null)
        {
            lookAt = mBlackboard.GetValue<BaseCharacter>("Target").Position;
        } else
        {
            lookAt = mTargetPosition;
        }

        if (lookAt != Vector3.Zero)
        {
            Transform3D lookTarget = Transform.LookingAt(lookAt, Vector3.Up);
            GlobalTransform = GlobalTransform.InterpolateWith(lookTarget, 1 * delta);
        }
            
        //this.LookAt(new Vector3(lookAt.X, lookAt.Y, lookAt.Z), Vector3.Up);
    }

    /// <summary>
    /// Sets the location for the agent to move to
    /// </summary>
    /// <param name="targetLocation">Location to set for the agent</param>
    public void SetTargetPosition(Vector3 targetLocation)
    {
        mTargetPosition = targetLocation;                   // Set the target location on this controller
        mAgent.TargetPosition = mTargetPosition;                // Update the location on the agent
        CanMove = true;                 // Enable movement

        // Update animator
        if(mAnimator != null)
            mAnimator.SetBool("IsMoving", true);
    }

    /// <summary>
    /// Stops the agent from moving
    /// </summary>
    public void StopMovement()
    {
        CanMove = false;                        // Disable movement
        mAgent.TargetPosition = this.Position;              // Update the agents position
        mBlackboard.SetValue("HasLocation", false);             // Update the blackboard

        // Update the animator
        if(mAnimator != null)
            mAnimator.SetBool("IsMoving", false);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        // Check that we have a location and can move
        if(mAgent.IsNavigationFinished() || !CanMove || IsBlocking)
            return;
        // Determine the path to take
        Vector3 currentAgentPosition = this.GlobalTransform.Origin;
        Vector3 nextPathPoint = mAgent.GetNextPathPosition();

        // Determine the velocity the agent requires & add the movement speed
        Vector3 createdVelocity = (nextPathPoint - currentAgentPosition).Normalized();
        createdVelocity *= mMovementSpeed;

        // Apply the velocity
        this.Velocity = createdVelocity;
        MoveAndSlide();
    }

    /// <summary>
    /// Creates the default blackboard for this character
    /// </summary>
    protected virtual void CreateBlackboard()
    {
        mBlackboard = new Blackboard();
        mBlackboard.SetValue<BaseAI>("Self", this);
        mBlackboard.SetValue("HasLocation", false);
        mBlackboard.SetValue("MoveToLocation", Vector3.Zero);
        mBlackboard.SetValue<BaseCharacter>("Target", null);
        mBlackboard.SetValue("MovementState", (int)mMovementState);
        mBlackboard.SetValue("HasWanderPoiint", false);
    }

    public override void LightAttack()
    {
        if(IsBlocking || IsAttacking)
            return;

        base.LightAttack();
    }

    public override void HeavyAttack()
    {
        if(IsBlocking || IsAttacking)
            return;

        base.HeavyAttack();
    }

    public override void TakeDamage(float dp, bool armorReduction = true)
    {
        base.TakeDamage(dp);
        // TODO: reduce damage with armor
        if(mStats != null && !mStats.IsAlive)
        {
            CanMove = false;
        }
    }

    public override void StopBlocking()
    {
        base.StopBlocking();

    }

    protected virtual async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        CreateInventory();
    }
}