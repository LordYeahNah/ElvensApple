using System;
using Godot;

public class HasLocation : Task
{
    public HasLocation(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if(mTree == null)
            return ETaskState.FAILURE;

        Blackboard  board = mTree.GetBlackboard();
        if(board != null)
        {
            if(board.GetValue<bool>("HasLocation"))
                return ETaskState.SUCCESS;
        }

        return ETaskState.FAILURE;
    }
}