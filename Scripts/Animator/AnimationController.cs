using System;
using System.Collections.Generic;
using Godot;

public class AnimationController
{
    public bool IsActive = true;
    private AnimationPlayer mAnimator;                          // Reference to the animation controller

    public AnimationController(AnimationPlayer mPlayer)
    {
        mAnimator = mPlayer;
    }    
}