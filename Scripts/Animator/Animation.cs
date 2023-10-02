using System;
using System.Collections.Generic;
using Godot;

public enum EPlayDirection
{
    FORWARD = 0,
    BACK = 1
}

public class Animation
{
    protected string mAnimationName;                        // Name of the animation we will play
    protected AnimationPlayer mPlayer;                      // Reference to the animation player to play the animation

    // List of animations we can transition to
    public List<Animation> TransitionAnimations = new List<Animation>();
    // Properties required to transition to this animation
    protected List<AnimationProperty> mRequiredProperties = new List<AnimationProperty>();   
    public List<AnimationProperty> RequiredProperties => mRequiredProperties;    

    protected bool mIsPaused = false;                       // if the animation is currently paused 

    public Animation(string animationName, AnimationPlayer animPlayer)
    {
        mAnimationName = animationName;
        mPlayer = animPlayer;
    }

    public void PlayAnimation(EPlayDirection playDirection = EPlayDirection.FORWARD)
    {
        if(mPlayer == null || mIsPaused)
            return;

        if(playDirection == EPlayDirection.FORWARD)
        {
            mPlayer.Play(mAnimationName);
        } else if(playDirection == EPlayDirection.BACK)
        {
            mPlayer.PlayBackwards(mAnimationName);
        }
    }

    public void SetPlayMove(bool paused)
    {
        mIsPaused = paused;
        if(mIsPaused)
            mPlayer.Pause();
        else
        PlayAnimation();
    }
}