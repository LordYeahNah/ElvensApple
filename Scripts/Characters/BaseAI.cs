using System;
using System.Collections.Generic;
using Godot;

public partial class BaseAI : CharacterBody3D
{

    [ExportGroup("Movement Settings")]
    [Export] protected NavigationAgent3D mAgent;
    [Export] protected float mMovementSpeed;
    protected Vector3 mTargetPosition;
    public bool CanMove = false;

    // Reference to AI Components
    protected Blackboard mBlackboard;
    protected BehaviorTree mTree;

    public override void _Ready()
    {
        base._Ready();

        // Debug blackboard setup
        CreateBlackboard();
        mTree = new TestBT();
        mTree.OnInitialize(mBlackboard);
    }

    public void SetTargetPosition(Vector3 targetLocation)
    {
        mTargetPosition = targetLocation;
        CanMove = true;
    }

    public void StopMovement()
    {
        CanMove = false;
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
        mBlackboard.SetValue<Node3D>("Self", this);
        mBlackboard.SetValue("HasMoveToLocation", false);
        mBlackboard.SetValue("MoveToLocation", Vector3.Zero);
    }
}