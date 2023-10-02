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
        Animation moveAnimation = new Animation("Run", mAnimator);
        moveAnimation.RequiredProperties.Add(new AnimationBool("IsMoving", true));

        // Create the idle animation
        Animation idleAnimation = new Animation("Idle", mAnimator);
        idleAnimation.RequiredProperties.Add(new AnimationBool("IsMoving", false));

        // Add the animation transitions
        idleAnimation.TransitionAnimations.Add(moveAnimation);
        moveAnimation.TransitionAnimations.Add(idleAnimation);

        return idleAnimation;                       // Return the idle animation as the root
    }
}