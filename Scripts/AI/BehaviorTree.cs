using System;
using System.Collections.Generic;
using Godot;

public abstract class BehaviorTree
{
    protected Task mRootTask;
    protected bool mIsInitialized = false;                      // If the tree has been set up
    protected Blackboard mBlackboard;
    public void OnInitialize(Blackboard board)
    {
        mBlackboard = board;
        mRootTask = CreateTree();
        mIsInitialized = true;
    }

    public void OnUpdate(float delta)
    {
        if(mIsInitialized)
            mRootTask.RunTask(delta);
    }

    protected abstract Task CreateTree();

    public Blackboard GetBlackboard() => mBlackboard;
}