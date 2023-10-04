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

        Animation idleAnim = new Animation("Idle", mAnimator, this);
        idleAnim.RequiredProperties.Add(new AnimationBool("IsMoving", false));

        Animation runAnim = new Animation("Run", mAnimator, this);
        runAnim.RequiredProperties.Add(new AnimationBool("IsMoving", true));

        idleAnim.TransitionAnimations.Add(runAnim);
        runAnim.TransitionAnimations.Add(idleAnim);

        mAnyTransitions.Add(CreateDeathAnim());

        return idleAnim;
    }

    private Animation CreateDeathAnim()
    {
        TriggerAnimation anim = new TriggerAnimation("Defeat", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool("IsAlive", false));

        return anim;
    }

    protected virtual void CreateProperties()
    {
        this.SetBool("IsMoving", false);
        this.SetBool("IsAlive", true);
    }
}