using System;
using System.Collections.Generic;
using Godot;

public partial class BaseAI : CharacterBody3D
{

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