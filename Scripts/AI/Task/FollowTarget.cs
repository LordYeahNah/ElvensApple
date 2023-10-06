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
                PollLocation(target, board, self);
                return ETaskState.SUCCESS;
            }
        }

        return ETaskState.FAILURE;
    }

    private void PollLocation(BaseCharacter target, Blackboard board, BaseAI self)
    {
        Vector3 targetLocation = target.Position;                   // Get reference to the target location
        self.SetTargetPosition(targetLocation);
        board.SetValue("MoveToLocation", targetLocation);
        mCurrentTime = 0.0f;
        mHasPolled = true;
    }
}