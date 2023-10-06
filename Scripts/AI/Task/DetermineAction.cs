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
                // If we haven't made a decision yet make one
                if(!mHasPolled)
                    PollDecision(self, board);

                mCurrentPollTime += 1 * delta;              // Increment the time to the next poll
                // if the time since the last poll is great the time we need to wait
                // then make a new decision
                if(mCurrentPollTime > POLL_TIME)
                    PollDecision(self, board);

                return ETaskState.SUCCESS;
            }
        }

        return ETaskState.FAILURE;
    }

    private void PollDecision(BaseAI self, Blackboard board)
    {
        // Random value generation
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        float randValue = rand.Randf();                 // Get a random value between 0 and 1
        float chanceOfBlock = 0.3f;                 // Define the chance of a block

        // If low health make it so that the ai is more likely to block
        if(self.CurrentHealth < 0.3)
            chanceOfBlock = 0.7f;

        if(randValue < chanceOfBlock)
        {
            // If the first generated value it within the block chance,
            // Then perform a block
            self.Block();
        } else 
        {
            // If no within the block chance perform attack

            BaseCharacter target = board.GetValue<BaseCharacter>("Target");             // Get reference to the target
            if(target != null)
            {
                // Generate a random value to determine the type of attack
                rand.Randomize();
                float chanceValue = rand.Randf();

                float chanceOfHeavy = 0.2f;                 // Define the chance of a heavy attack

                // If the target is low on health increase the chance of a heavy attack
                if(target.CurrentHealth > 0.2)
                    chanceOfHeavy = 0.4f;

                // Perform the determined attack
                if(chanceValue < chanceOfHeavy)
                    self.HeavyAttack();
                else
                    self.LightAttack();
            }
        }

        // Reset the poll
        mHasPolled = true;
        mCurrentPollTime = 0f;
    }
}