using System;
using System.Collections.Generic;
using Godot;

public class BaseAI_Animator : AnimationController
{
    // === PROPERTY KEYS === //
    public const string IS_ATTACKING = "IsAttacking";
    public const string IS_MOVING = "IsMoving";
    public const string ATTACK_TYPE = "AttackType";
    public const string IS_ALIVE = "IsAlive";


    public BaseAI_Animator(AnimationPlayer mPlayer) : base(mPlayer)
    {
    }

    public override Animation CreateAnimationTree()
    {
        CreateProperties();

        Animation idleAnim = new Animation("Idle", mAnimator, this);
        idleAnim.RequiredProperties.Add(new AnimationBool(IS_MOVING, false));

        Animation runAnim = new Animation("Run", mAnimator, this);
        runAnim.RequiredProperties.Add(new AnimationBool(IS_MOVING, true));

        idleAnim.TransitionAnimations.Add(runAnim);
        runAnim.TransitionAnimations.Add(idleAnim);

        mAnyTransitions.Add(CreateDeathAnim());
        mAnyTransitions.Add(CreateLightAttack(idleAnim));
        mAnyTransitions.Add(CreateHeavyAttack(idleAnim));

        return idleAnim;
    }

    private Animation CreateDeathAnim()
    {
        TriggerAnimation anim = new TriggerAnimation("Defeat", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool(IS_ALIVE, false));
        
        return anim;
    }

    private Animation CreateLightAttack(Animation returnAnimation)
    {
        TriggerAnimation anim = new TriggerAnimation("Attack(1h)", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool(IS_ATTACKING, true));
        anim.RequiredProperties.Add(new AnimationInt(ATTACK_TYPE, (int)EAttackType.LIGHT));
        AnimationEvent animEvent = new AnimationEvent(0.8f, ResetAttack);
        anim.AnimEvents.Add(animEvent);
        anim.TransitionAnimations.Add(returnAnimation);
        return anim;
    }

    private Animation CreateHeavyAttack(Animation returnAnimation)
    {
        TriggerAnimation anim = new TriggerAnimation("HeavyAttack", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool(IS_ATTACKING, true));
        anim.RequiredProperties.Add(new AnimationInt(ATTACK_TYPE, (int)EAttackType.HEAVY));

        AnimationEvent animEvent = new AnimationEvent(0.8f, ResetAttack);
        anim.AnimEvents.Add(animEvent);
        anim.TransitionAnimations.Add(returnAnimation);
        return anim;
    }

    protected virtual void CreateProperties()
    {
        this.SetBool("IsMoving", false);
        this.SetBool("IsAlive", true);
        this.SetBool("IsAttacking", false);
        this.SetInt("AttackType", 0);
    }

    public void ResetAttack()
    {
        this.SetBool("IsAttacking", false);
        this.SetInt("AttackType", 0);
    }
}