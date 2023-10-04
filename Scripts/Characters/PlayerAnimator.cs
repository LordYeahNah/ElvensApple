using System;
using System.Collections.Generic;
using Godot;

public class PlayerAnimator : AnimationController
{
    private PlayerController mPlayerController;
    public PlayerAnimator(AnimationPlayer mPlayer, PlayerController controller) : base(mPlayer)
    {
        mPlayerController = controller;
    }

    public override Animation CreateAnimationTree()
    {
        SetBool("IsMoving", false);                     // Add the property for the movement
        SetBool("IsAttacking", false);

        // Create the movement animation
        Animation moveAnimation = new Animation("Run", mAnimator, this);
        moveAnimation.RequiredProperties.Add(new AnimationBool("IsMoving", true));
        moveAnimation.RequiredProperties.Add(new AnimationBool("IsAttacking", false));

        // Create the idle animation
        Animation idleAnimation = new Animation("Idle", mAnimator, this);
        idleAnimation.RequiredProperties.Add(new AnimationBool("IsMoving", false));
        idleAnimation.RequiredProperties.Add(new AnimationBool("IsAttacking", false));

        // Add the animation transitions
        idleAnimation.TransitionAnimations.Add(moveAnimation);
        moveAnimation.TransitionAnimations.Add(idleAnimation);

        mAnyTransitions.Add(CreateLightAttack(idleAnimation));

        return idleAnimation;                       // Return the idle animation as the root
    }

    private Animation CreateLightAttack(Animation transitionBack)
    {
        TriggerAnimation anim = new TriggerAnimation("Attack(1h)", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool("IsAttacking", true));
        anim.TransitionAnimations.Add(transitionBack);

        AnimationEvent resetEvent = new AnimationEvent(0.7f, OnResetAttackAnimation);
        anim.AnimEvents.Add(resetEvent);

        return anim;
    }

    public void OnResetAttackAnimation()
    {
        this.SetBool("IsAttacking", false);                 // Reset the animator
        
        // Update the player controller
        if(mPlayerController != null)
            mPlayerController.IsAttacking = false;
    }

    public void TestAnimation()
    {
        GD.Print("Event Hit");
    }
}