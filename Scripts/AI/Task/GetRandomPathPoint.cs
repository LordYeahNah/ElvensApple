using System;
using System.Collections.Generic;
using Godot;

public class GetRandomPathPoint : Task
{
    public GetRandomPathPoint(BehaviorTree bTree) : base(bTree)
    {
    }

    public GetRandomPathPoint(BehaviorTree bTree, List<Task> children) : base(bTree, children)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if (mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();               // Get reference to the blackboard
        if (board != null)
        {
            
        }
    }
}