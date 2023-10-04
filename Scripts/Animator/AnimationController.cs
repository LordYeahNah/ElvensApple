using System;
using System.Collections.Generic;
using Godot;

public abstract class AnimationController
{
    public bool IsActive = true;
    protected AnimationPlayer mAnimator;                          // Reference to the animation controller

    protected List<AnimationProperty> mProperties = new List<AnimationProperty>();                    // Reference to the animation properties used for transitions
    protected List<Animation> mAnyTransitions = new List<Animation>();
    protected List<AnimationProperty> mTriggers = new List<AnimationProperty>();
    protected Animation mCurrentAnimation;
    public Animation CurrentAnimation => mCurrentAnimation;

    public AnimationController(AnimationPlayer mPlayer)
    {
        mAnimator = mPlayer;
        mCurrentAnimation = CreateAnimationTree();                 // Create the tree
        mCurrentAnimation.ShouldPlay = true;
    } 

    public abstract Animation CreateAnimationTree();   

    public void OnUpdate(float delta)
    {
        if(mCurrentAnimation != null)
            mCurrentAnimation.OnUpdate(delta);

        if(mAnyTransitions.Count > 0)
        {
            foreach(var anim in mAnyTransitions)
            {
                if(anim == mCurrentAnimation)
                    continue;

                bool transitionValid = true;
                foreach(var animProp in anim.RequiredProperties)
                {
                    foreach(var transProp in mProperties)
                    {
                        if(transProp.Key == animProp.Key)
                        {
                            transitionValid = CompareProperty(animProp, transProp);
                            if(!transitionValid)
                                break;
                        }
                    }

                    if(!transitionValid)
                        break;

                }

                if(transitionValid && mCurrentAnimation != anim)
                {
                    SetCurrentAnimation(anim);
                    return;
                }
                    
            }
        }
    }

    public void CheckTransition()
    {
        if(mCurrentAnimation == null || mCurrentAnimation.TransitionAnimations.Count == 0)
            return;

        foreach(var animation in mCurrentAnimation.TransitionAnimations)
        {
            // Don't check currently playing animation
            if(animation == mCurrentAnimation)
                continue;

            List<AnimationProperty> transProps = animation.RequiredProperties;
            bool transitionValid = true;
            foreach(var transProp in transProps)
            {
                foreach(var animProp in mProperties)
                {
                    if(transProp.Key == animProp.Key)
                    {
                        transitionValid = CompareProperty(transProp, animProp);
                        if(!transitionValid)
                            break;
                    }
                }

                if(!transitionValid)
                    break;
            }

            if(transitionValid)
            {
                SetCurrentAnimation(animation);
                return;
            }
        }
    }

    private bool CompareProperty(AnimationProperty a, AnimationProperty b)
    {
        if(a is AnimationBool aBool && b is AnimationBool bBool)
        {
            if(aBool.Value == bBool.Value)
                return true;
        } else if(a is AnimationFloat aFloat && b is AnimationFloat bFloat)
        {
            if(aFloat.Value == bFloat.Value)
                return true;
        } else if(a is AnimationInt aInt && b is AnimationInt bInt)
        {
            if(aInt.Value == bInt.Value)
                return true;
        }

        return false;
    }

    #region Properties Getters & Setters

    public void SetInt(string key, int value)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is AnimationInt propInt)
            {
                propInt.Value = value;
                CheckTransition();
                return;
            }
        }

        mProperties.Add(new AnimationInt(key, value));
        CheckTransition();
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
                CheckTransition();
                return;
            }
        }

        mProperties.Add(new AnimationFloat(key, value));
        CheckTransition();
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
                CheckTransition();
                return;
            }
        }

        mProperties.Add(new AnimationBool(key, Value));
        CheckTransition();
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

    public void SetCurrentAnimation(Animation anim)
    {
        if(anim != null)
        {
            if(mCurrentAnimation != null)
                mCurrentAnimation.StopAnimation();

            mCurrentAnimation = anim;
            mCurrentAnimation.ShouldPlay = true;
        }
    }
}