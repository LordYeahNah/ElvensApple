using System;
using System.Collections.Generic;
using Godot;

public class FinishWander : Task
{
    
    public FinishWander(BehaviorTree bTree) : base(bTree)
    {
    }

    public FinishWander(BehaviorTree bTree, List<Task> children) : base(bTree, children)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if (mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();
        if (board != null)
        {
            board.SetValue("HasWanderPoint", false);
            
        }

        return ETaskState.SUCCESS;
    }
}