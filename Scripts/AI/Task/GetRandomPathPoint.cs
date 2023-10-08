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
            if (!board.GetValue<bool>("HasWanderPoint"))
            {
                board.SetValue("MoveToLocation", LevelController.Instance.GetRandomPosition());
                board.SetValue("HasWanderPoint", true);
            }

            return ETaskState.SUCCESS;
        }

        return ETaskState.FAILURE;
    }
}