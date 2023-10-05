using System;
using System.Collections.Generic;
using Godot;

public enum EActionDecision
{
    BLOCK = 0,
    ATTACK = 1,
}

public partial class BaseAI : BaseCharacter, ICombat
{

    [ExportGroup("Movement Settings")]
    [Export] protected NavigationAgent3D mAgent;
    [Export] protected float mMovementSpeed;
    protected Vector3 mTargetPosition;
    public bool CanMove = false;

    // Reference to AI Components
    protected Blackboard mBlackboard;
    protected BehaviorTree mTree;

    [ExportGroup("Components")]
    [Export] protected AnimationPlayer mAnimPlayer;

    public override void _Ready()
    {
        base._Ready();

        // Debug blackboard setup
        CreateBlackboard();
        mTree = new TestBT();
        mTree.OnInitialize(mBlackboard);
        if(mAnimPlayer != null)
            mAnimator = new BaseAI_Animator(mAnimPlayer);

        mInventory = new Inventory(5, mLeftHand, mRightHand, this);

        // Debug //
        BaseCharacter player = (BaseCharacter)GetTree().GetFirstNodeInGroup("Player");
        if(player != null)
            mBlackboard.SetValue("Target", player);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(mTree != null)
            mTree.OnUpdate((float)delta);
    }

    public void SetTargetPosition(Vector3 targetLocation)
    {
        GD.Print(targetLocation);
        mTargetPosition = targetLocation;
        mAgent.TargetPosition = mTargetPosition;
        CanMove = true;

        if(mAnimator != null)
            mAnimator.SetBool("IsMoving", true);
    }

    public void StopMovement()
    {
        CanMove = false;
        mAgent.TargetPosition = this.Position;
        mBlackboard.SetValue("HasLocation", false);

        if(mAnimator != null)
            mAnimator.SetBool("IsMoving", false);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if(mAgent.IsNavigationFinished() || !CanMove)
            return;

        Vector3 currentAgentPosition = this.GlobalTransform.Origin;
        Vector3 nextPathPoint = mAgent.GetNextPathPosition();

        Vector3 createdVelocity = (nextPathPoint - currentAgentPosition).Normalized();
        createdVelocity *= mMovementSpeed;

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
    }

    public override void LightAttack()
    {
        throw new NotImplementedException();
    }

    public override void HeavyAttack()
    {
        throw new NotImplementedException();
    }

    public override void TakeDamage(float dp)
    {
        base.TakeDamage(dp);
        // TODO: reduce damage with armor
        if(mStats != null && !mStats.IsAlive)
        {
            CanMove = false;
        }
    }
}