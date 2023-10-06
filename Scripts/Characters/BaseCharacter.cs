using System;
using System.Collections.Generic;
using Godot;

public partial class BaseCharacter : CharacterBody3D, ICombat
{
    protected CharacterStats mStats;
    public CharacterStats Stats => mStats;
     public float CurrentHealth => mStats.CurrentHealth;

    // === CURRENT STATE === //
    public bool IsBlocking;

    [ExportGroup("Inventory")]
    [Export] protected InventoryController mInventoryPanel;
    protected bool mIsInventoryOpen = false;
    protected Inventory mInventory;
    [Export] protected BoneAttachment3D mLeftHand;
    [Export] protected BoneAttachment3D mRightHand;

    protected AnimationController mAnimator;

    public override void _Ready()
    {
        base._Ready();
        mStats = new CharacterStats();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(mAnimator != null)
            mAnimator.OnUpdate((float)delta);
    }

    public virtual void LightAttack()
    {
        if(mAnimator != null)
        {
            mAnimator.SetBool("IsAttacking", true),
            mAnimator.SetInt("AttackType", (int)EAttackType.LIGHT);
        }
    }

    public virtual void HeavyAttack()
    {
        throw new NotImplementedException();
    }

    public virtual void TakeDamage(float dp)
    {
        if(mStats != null)
        {
            mStats.TakeDamage(dp);
            if(!mStats.IsAlive)
            {
                mAnimator.SetBool("IsAlive", false);
            }
        }
    }

    public virtual void Block()
    {
        IsBlocking = true;
        if(mAnimator != null)
            mAnimator.SetBool(PlayerAnimator.IS_BLOCKING, true);
    }

    public virtual void StopBlocking()
    {
        IsBlocking = false;
        if(mAnimator != null)
            mAnimator.SetBool("IsBlocking", false);
    }
}