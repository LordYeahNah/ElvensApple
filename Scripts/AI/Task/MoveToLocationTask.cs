using System;
using Godot;

public class MoveToLocationTask : Task
{
    private const float STOPPING_DISTANCE = 2.0f;
    private const float POLL_TIME = 1.0f;
    private float mTimeSincePoll = 0.0f;
    private bool mHasPolled = false;
    public MoveToLocationTask(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if(mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();
        if(board != null)
        {
            Vector3 moveToLocation = board.GetValue<Vector3>("MoveToLocation");
            Vector3 currentLocation = board.GetValue<Node3D>("Self").Position;
            float distance = moveToLocation.DistanceTo(currentLocation);

            mTimeSincePoll += 1 * delta;
            if(mTimeSincePoll > POLL_TIME)
                return PollLocation(distance);
        }

        return ETaskState.RUNNING;
    }

    private ETaskState PollLocation(float distance)
    {
        if(distance > STOPPING_DISTANCE)
        {
            // TODO: Set move location on agent
            return ETaskState.RUNNING;
        } else 
        {
            // TODO: Stop moving agent
            return ETaskState.SUCCESS;
        }
        mTimeSincePoll = 0.0f;
        mHasPolled = true;
    }
}