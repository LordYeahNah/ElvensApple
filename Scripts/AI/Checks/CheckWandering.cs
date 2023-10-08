using System.Collections.Generic;
using System;
using Godot;

public class CheckWandering : Task
{
    public CheckWandering(BehaviorTree bTree) : base(bTree)
    {
    }

    public CheckWandering(BehaviorTree bTree, List<Task> children) : base(bTree, children)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if (mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();
        if (board != null)
        {
            EMovementState state = (EMovementState)board.GetValue<int>("MovementState");
            if (state == EMovementState.WANDERING)
            {
                return ETaskState.SUCCESS;
            }
                
        }

        return ETaskState.FAILURE;
    }
}