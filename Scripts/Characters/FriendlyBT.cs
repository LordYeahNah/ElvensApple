using System.Collections.Generic;
using System.Threading;

namespace ElvensApple.Scripts.Characters;

public class FriendlyBT : BehaviorTree
{
    protected override Task CreateTree()
    {
        return new Selector(this, new List<Task>
        {
            new Sequence(this, new List<Task>
            {
                new CheckWandering(this),
                new GetRandomPathPoint(this),
                new MoveToLocationTask(this),
                new FinishWander(this)
            })
        });
    }
}