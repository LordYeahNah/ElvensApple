using System;
using Godot;

public class HasTarget : Task
{
    public HasTarget(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if(mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();
        if(board != null)
        {
            BaseCharacter target = board.GetValue<BaseCharacter>("Target");
            if(target != null)
                return ETaskState.SUCCESS;
        }

        return ETaskState.FAILURE;
    }
}