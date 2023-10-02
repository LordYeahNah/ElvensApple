using System;
using Godot;

public class MoveToLocationTask : Task
{
    private const float STOPPING_DISTANCE = 0.5f;
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
            // Get the move to location
            Vector3 moveToLocation = board.GetValue<Vector3>("MoveToLocation");
            // Get the current location of the agent
            Vector3 currentLocation = board.GetValue<BaseAI>("Self").Position;
            // Get the distance to the position
            float distance = moveToLocation.DistanceTo(currentLocation);

            if(!mHasPolled)
                return PollLocation(distance, board);

            mTimeSincePoll += 1 * delta;                    // Update the poll timer
            // If we need to poll then poll the location
            if(mTimeSincePoll > POLL_TIME)
                return PollLocation(distance, board);
        }

        return ETaskState.RUNNING;
    }

    private ETaskState PollLocation(float distance, Blackboard board)
    {
        BaseAI ai = board.GetValue<BaseAI>("Self");
        GD.Print(distance);
        if(distance > STOPPING_DISTANCE)
        {
            if(ai != null)
            {
                ai.SetTargetPosition(board.GetValue<Vector3>("MoveToLocation"));
            }
            mHasPolled = true;
            mTimeSincePoll = 0.0f;
            return ETaskState.RUNNING;
        } else 
        {
            if(ai != null)
                ai.StopMovement();
            mHasPolled = false;
            mTimeSincePoll = 0.0f;
            return ETaskState.SUCCESS;
        }

    }
}