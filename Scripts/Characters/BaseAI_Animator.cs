using System;
using System.Collections.Generic;
using Godot;

public class BaseAI_Animator : AnimationController
{
    public BaseAI_Animator(AnimationPlayer mPlayer) : base(mPlayer)
    {
    }

    public override Animation CreateAnimationTree()
    {
        CreateProperties();

        Animation idleAnim = new Animation("Idle", mAnimator);
        idleAnim.RequiredProperties.Add(new AnimationBool("IsMoving", false));

        Animation runAnim = new Animation("Run", mAnimator);
        runAnim.RequiredProperties.Add(new AnimationBool("IsMoving", true));

        idleAnim.TransitionAnimations.Add(runAnim);
        runAnim.TransitionAnimations.Add(idleAnim);

        return idleAnim;
    }

    protected virtual void CreateProperties()
    {
        this.SetBool("IsMoving", false);
    }
}