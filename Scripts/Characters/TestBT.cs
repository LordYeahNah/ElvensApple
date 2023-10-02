using System;
using System.Collections.Generic;
using Godot;

public class TestBT : BehaviorTree
{
    protected override Task CreateTree()
    {
        return new Selector(this, new List<Task>
        {
            new Sequence(this, new List<Task>
            {
                new HasLocation(this),
                new MoveToLocationTask(this)
            })
        });
    }
}