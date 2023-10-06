using System;
using Godot;

public class DetermineAction : Task
{
    private const float POLL_TIME = 1.3f;
    private float mCurrentPollTime = 0.0f;
    private bool mHasPolled = false;
    public DetermineAction(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        if(mTree == null)
            return ETaskState.FAILURE;

        Blackboard board = mTree.GetBlackboard();
        if(board != null)
        {
            BaseAI self = board.GetValue<BaseAI>("Self");                   // Get reference to the owner of the board
            if(self != null)
            {
                if(!mHasPolled)
                    PollDecision(self, board);

                mCurrentPollTime += 1 * delta;
                if(mCurrentPollTime > POLL_TIME)
                    PollDecision(self, board);

                return ETaskState.SUCCESS;
            }
        }

        return ETaskState.FAILURE;
    }

    private void PollDecision(BaseAI self, Blackboard board)
    {

        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        float randValue = rand.Randf();
        float chanceOfBlock = 0.3f;

        if(self.CurrentHealth < 0.3)
            chanceOfBlock = 0.7f;

        if(randValue < chanceOfBlock)
        {
            self.Block();
        } else 
        {
            BaseCharacter target = board.GetValue<BaseCharacter>("Target");
            if(target != null)
            {
                rand.Randomize();
                float chanceValue = rand.Randf();
                float chanceOfHeavy = 0.2f;

                if(target.CurrentHealth > 0.2)
                    chanceOfHeavy = 0.4f;

                if(chanceValue < chanceOfHeavy)
                    self.HeavyAttack();
                else
                    self.LightAttack();
            }
        }

        mHasPolled = true;
        mCurrentPollTime = 0f;
    }
}