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

    protected BeahviourTree mTree;

    public Task(BeahviourTree bTree)
    {
        mTree = bTree;
    }

    public Task(BeahviourTree bTree, List<Task> children)
    {
        mTree = bTree;
        foreach(var child in children)
        {
            child.Parent = this;
            mChildren.Add(child);
        }
    }

    public abstract ETaskState RunTask();
}