using System;
using System.Collections.Generic;
using Godot;

public class Selector : Task
{
    public Selector(BehaviorTree bTree) : base(bTree)
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

        return ETaskState.FAILURE;
    }
}