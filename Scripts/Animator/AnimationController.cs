using System;
using System.Collections.Generic;
using Godot;

public abstract class AnimationController
{
    public bool IsActive = true;
    protected AnimationPlayer mAnimator;                          // Reference to the animation controller

    protected List<AnimationProperty> mProperties = new List<AnimationProperty>();                    // Reference to the animation properties used for transitions
    
    protected Animation mRootAnimation;

    public AnimationController(AnimationPlayer mPlayer)
    {
        mAnimator = mPlayer;
        mRootAnimation = CreateAnimationTree();                 // Create the tree
    } 

    public abstract Animation CreateAnimationTree();   

    #region Properties Getters & Setters

    public void SetInt(string key, int value)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationInt propInt)
            {
                propInt.Value = value;
                return;
            }
        }

        mProperties.Add(new AnimationInt(key, value));
    }

    public int GetInt(string key)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationInt propInt)
                return propInt.Value;
        }

        return 0;
    }

    public void SetFloat(string key, float value)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationFloat propFloat)
            {
                propFloat.Value = value;
                return;
            }
        }

        mProperties.Add(new AnimationFloat(key, value));
    }

    public float GetFloat(string key)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationFloat propFloat)
                return propFloat.Value;
        }

        return 0f;
    }

    public void SetBool(string key, bool Value)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationBool propBool)
            {
                propBool.Value = Value;
                return;
            }
        }

        mProperties.Add(new AnimationBool(key, Value));
    }

    public bool GetBool(string key)
    {
        foreach (var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationBool propBool)
                return propBool.Value;
        }

        return false;
    }

    #endregion
}