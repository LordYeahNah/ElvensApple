using System;
using System.Collections.Generic;
using Godot;

public class PlayerAnimator : AnimationController
{
    public PlayerAnimator(AnimationPlayer mPlayer) : base(mPlayer)
    {
    }

    public override Animation CreateAnimationTree()
    {
        SetBool("IsMoving", false);                     // Add the property for the movement

        // Create the movement animation
        Animation moveAnimation = new Animation("Run", mAnimator, this);
        moveAnimation.RequiredProperties.Add(new AnimationBool("IsMoving", true));

        // Create the idle animation
        Animation idleAnimation = new Animation("Idle", mAnimator, this);
        idleAnimation.RequiredProperties.Add(new AnimationBool("IsMoving", false));

        // Add the animation transitions
        idleAnimation.TransitionAnimations.Add(moveAnimation);
        moveAnimation.TransitionAnimations.Add(idleAnimation);

        idleAnimation.AnimEvents.Add(new AnimationEvent(0.8f, TestAnimation));

        return idleAnimation;                       // Return the idle animation as the root
    }

    public void TestAnimation()
    {
        GD.Print("Event Hit");
    }
}