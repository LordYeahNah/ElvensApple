using System;
using System.Collections.Generic;
using Godot;

public class Sequence : Task
{
    public Sequence(BehaviorTree bTree) : base(bTree)
    {
    }

    public override ETaskState RunTask()
    {
        foreach(var child in mChildren)
        {
            switch(child.RunTask())
            {
                case ETaskState.SUCCESS:
                    continue;
                case ETaskState.FAILURE:
                    return ETaskState.FAILURE;
                case ETaskState.RUNNING:
                    return ETaskState.RUNNING;
            }
        }

        return ETaskState.FAILURE;
    }
}