using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Godot;

public class PlayerAnimator : AnimationController
{

    // === PROPERTY KEYS === //
    public const string IS_ATTACKING = "IsAttacking";
    public const string IS_MOVING = "IsMoving";
    public const string ATTACK_TYPE = "AttackType";
    public const string IS_ALIVE = "IsAlive";
    public const string IS_BLOCKING = "IsBlocking";
    public const string IS_USING_MAGIC = "IsUsingMagic";
    public const string MAGIC_INDEX = "MagicIndex";

    private PlayerController mPlayerController;
    public PlayerAnimator(AnimationPlayer mPlayer, PlayerController controller) : base(mPlayer)
    {
        mPlayerController = controller;
        CreateProperties();
    }

    private void CreateProperties()
    {
        SetBool(IS_MOVING, false);  
        SetBool(IS_ATTACKING, false);
        SetBool(IS_ALIVE, true);
        SetInt(ATTACK_TYPE, 0);
        SetBool(IS_BLOCKING, false);
        SetBool(IS_USING_MAGIC, false);
        SetInt(MAGIC_INDEX, 0);
    }

    public override Animation CreateAnimationTree()
    {
        

        // Create the movement animation
        Animation moveAnimation = new Animation("Run", mAnimator, this);
        moveAnimation.RequiredProperties.Add(new AnimationBool(IS_MOVING, true));
        moveAnimation.RequiredProperties.Add(new AnimationBool(IS_ATTACKING, false));
        moveAnimation.RequiredProperties.Add(new AnimationBool(IS_BLOCKING, false));
        moveAnimation.RequiredProperties.Add(new AnimationBool(IS_USING_MAGIC, false));

        // Create the idle animation
        Animation idleAnimation = new Animation("Idle", mAnimator, this);
        idleAnimation.RequiredProperties.Add(new AnimationBool(IS_MOVING, false));
        idleAnimation.RequiredProperties.Add(new AnimationBool(IS_ATTACKING, false));
        idleAnimation.RequiredProperties.Add(new AnimationBool(IS_BLOCKING, false));
        idleAnimation.RequiredProperties.Add(new AnimationBool(IS_USING_MAGIC, false));

        Animation blockAnim = new Animation("Block", mAnimator, this);
        blockAnim.RequiredProperties.Add(new AnimationBool(IS_BLOCKING, true));
        AnimationEvent pauseEvent = new AnimationEvent(1.2f, PauseBlockAnim);
        blockAnim.AnimEvents.Add(pauseEvent);

        // Add the animation transitions
        idleAnimation.TransitionAnimations.Add(moveAnimation);
        idleAnimation.TransitionAnimations.Add(blockAnim);
        moveAnimation.TransitionAnimations.Add(idleAnimation);
        moveAnimation.TransitionAnimations.Add(blockAnim);
        blockAnim.TransitionAnimations.Add(moveAnimation);
        blockAnim.TransitionAnimations.Add(idleAnimation);


        // Animation that can be triggered at any time
        mAnyTransitions.Add(CreateLightAttack(idleAnimation));
        mAnyTransitions.Add(CreateDeathAnimation());
        mAnyTransitions.Add(CreateHeavyAttack(idleAnimation));
        mAnyTransitions.Add(CreateSpellIndexOne(idleAnimation));

        return idleAnimation;                       // Return the idle animation as the root
    }

    private Animation CreateLightAttack(Animation transitionBack)
    {
        TriggerAnimation anim = new TriggerAnimation("Attack(1h)", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool(IS_ATTACKING, true));
        anim.RequiredProperties.Add(new AnimationInt(ATTACK_TYPE, (int)EAttackType.LIGHT));
        anim.TransitionAnimations.Add(transitionBack);

        AnimationEvent resetEvent = new AnimationEvent(0.7f, OnResetAttackAnimation);
        anim.AnimEvents.Add(resetEvent);

        return anim;
    }

    private Animation CreateHeavyAttack(Animation transitionBack)
    {
        TriggerAnimation anim = new TriggerAnimation("HeavyAttack", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool(IS_ATTACKING, true));
        anim.RequiredProperties.Add(new AnimationInt(ATTACK_TYPE, (int)EAttackType.HEAVY));

        AnimationEvent resetEvent = new AnimationEvent(0.7f, OnResetAttackAnimation);
        anim.AnimEvents.Add(resetEvent);
        anim.TransitionAnimations.Add(transitionBack);

        return anim;

    }

    private Animation CreateDeathAnimation()
    {
        TriggerAnimation anim = new TriggerAnimation("Defeat", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool("IsAlive", false));

        return anim;
    }

    private Animation CreateSpellIndexOne(Animation transBack)
    {
        TriggerAnimation anim = new TriggerAnimation("Cheer", mAnimator, this);
        anim.RequiredProperties.Add(new AnimationBool(IS_USING_MAGIC, true));
        anim.RequiredProperties.Add(new AnimationInt(MAGIC_INDEX, 1));
        anim.TransitionAnimations.Add(transBack);

        AnimationEvent animEvent = new AnimationEvent(1.1f, ResetMagic);
        anim.AnimEvents.Add(animEvent);

        return anim;
    }

    public void OnResetAttackAnimation()
    {
        this.SetBool("IsAttacking", false);                 // Reset the animator
        
        // Update the player controller
        if(mPlayerController != null)
            mPlayerController.IsAttacking = false;
    }

    private void ResetMagic()
    {
        SetBool(IS_USING_MAGIC, false);
        SetInt(MAGIC_INDEX, 0);
    }

    public void PauseBlockAnim()
    {
        mAnimator.Pause();
    }

    public void ResumeBlockAnim()
    {
        mAnimator.Play();
    }

    public void TestAnimation()
    {
        GD.Print("Event Hit");
    }
}