using System;
using System.Collections.Generic;
using Godot;

public enum ETaskState
{
    SUCCESS = 0,
    FAILURE = 1,
    RUNNING = 2
}

public abstract class Task
{
    public Task Parent;
    protected List<Task> mChildren;

    public Task()
    {

    }

    public Task(List<Task> children)
    {
        foreach(var child in children)
        {
            child.Parent = this;
            mChildren.Add(child);
        }
    }

    public abstract ETaskState RunTask();
}