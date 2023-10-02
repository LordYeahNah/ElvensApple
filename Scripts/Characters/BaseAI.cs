using System;
using System.Collections.Generic;
using Godot;

public partial class BaseAI : CharacterBody3D
{

    private AnimationController mAnim;

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

        if(mAnim != null)
            mAnim.SetBool("IsMoving", true);
    }

    public void StopMovement()
    {
        CanMove = false;
        mAgent.TargetPosition = this.Position;
        if(mAnim != null)
            mAnim.SetBool("IsMoving", false);
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
        mBlackboard.SetValue("HasLocation", true);
        mBlackboard.SetValue("MoveToLocation", new Vector3(9.6f, 0.015f, 6.8f));
    }
}