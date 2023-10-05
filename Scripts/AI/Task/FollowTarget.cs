using System;
using Godot;

public class FollowTarget : Task
{
    private const float POLL_TIME = 0.8f;
    private float mCurrentTime = 0.0f;
    private bool mHasPolled = false;

    public FollowTarget(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if(mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();
        if(board != null)
        {
            BaseCharacter target = board.GetValue<BaseCharacter>("Target");                 // Get reference to the target
            BaseAI self = board.GetValue<BaseAI>("Self");
            if(target != null && self != null)
            {
                if(!mHasPolled)
                {
                    PollLocation(target, board);
                    return ETaskState.SUCCESS;
                } else 
                {
                    mCurrentTime += 1 * delta;
                    if(mCurrentTime > POLL_TIME)
                    {
                        PollLocation(target, board);
                        return ETaskState.SUCCESS;
                    }
                }
            }
        }

        return ETaskState.FAILURE;
    }

    private void PollLocation(BaseCharacter target, Blackboard board)
    {
        Vector3 targetLocation = target.Position;                   // Get reference to the target location
        board.SetValue("MoveToLocation", targetLocation);
        mCurrentTime = 0.0f;
        mHasPolled = true;
    }
}