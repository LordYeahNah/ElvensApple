using System;
using System.Collections.Generic;
using Godot;

public abstract class BehaviorTree
{
    protected Task mRootTask;
    protected bool mIsInitialized = false;                      // If the tree has been set up

    public void OnInitialize()
    {
        mRootTask = CreateTree();
        mIsInitialized = true;
    }

    public void OnUpdate(float delta)
    {
        if(mIsInitialized)
            mRootTask.RunTask();
    }

    protected abstract Task CreateTree();
}