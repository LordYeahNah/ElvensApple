using System;
using System.Collections.Generic;
using Godot;

public class Animation
{
    protected string mAnimationName;                        // Name of the animation we will play
    protected AnimationPlayer mPlayer;                      // Reference to the animation player to play the animation

    // List of animations we can transition to
    public List<Animation> TransitionAnimations = new List<Animation>();
    // Properties required to transition to this animation
    protected List<AnimationProperty> mRequiredProperties = new List<AnimationProperty>();   
    public List<AnimationProperty> RequiredProperties => mRequiredProperties;        

    public Animation(string animationName, AnimationPlayer animPlayer)
    {
        mAnimationName = animationName;
        mPlayer = animPlayer;
    }
}