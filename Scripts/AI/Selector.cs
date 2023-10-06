using System;
using System.Collections.Generic;
using Godot;

public class Selector : Task
{
    public Selector(BehaviorTree bTree) : base(bTree)
    {
    }

    public Selector(BehaviorTree bTree, List<Task> children) : base(bTree, children)
    {
    }

    public override ETaskState RunTask(float delta)
    {
        foreach(var child in mChildren)
        {
            switch(child.RunTask(delta))
            {
                case ETaskState.SUCCESS:
                    return ETaskState.SUCCESS;
                case ETaskState.FAILURE:
                    continue;
                case ETaskState.RUNNING:
                    return ETaskState.RUNNING;
            }
        }

        return ETaskState.RUNNING;
    }
}