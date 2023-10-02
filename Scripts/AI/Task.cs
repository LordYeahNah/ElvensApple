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
    protected List<Task> mChildren = new List<Task>();

    protected BehaviorTree mTree;

    public Task(BehaviorTree bTree)
    {
        mTree = bTree;
    }

    public Task(BehaviorTree bTree, List<Task> children)
    {
        mTree = bTree;
        foreach(var child in children)
        {
            child.Parent = this;
            mChildren.Add(child);
        }
    }

    public abstract ETaskState RunTask(float delta);
}