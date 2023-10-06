using System;
using System.Reflection.Metadata.Ecma335;
using Godot;

public class CheckTargetDistance : Task
{
    private const float MIN_TARGET_DISTANCE = 1.2f;
    public CheckTargetDistance(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if(mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();               // Get reference to the blackboard
        if(board != null)
        {
            // Get reference to the target, and the AI itself
            BaseCharacter target = board.GetValue<BaseCharacter>("Target");
            BaseAI self = board.GetValue<BaseAI>("Self");

            if(target != null && self != null)
            {
                float distance = self.Position.DistanceTo(target.Position);                 // Get the distance to the target

                if(distance > MIN_TARGET_DISTANCE)
                {
                    return ETaskState.FAILURE;
                } else 
                {
                    return ETaskState.SUCCESS;
                }
            }
        }

        return ETaskState.FAILURE;
    }
}