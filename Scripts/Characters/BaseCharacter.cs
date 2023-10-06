using System;
using System.Collections.Generic;
using Godot;

public partial class BaseCharacter : CharacterBody3D, ICombat
{
    // === CHARACTER STATS === //
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

    /// <summary>
    /// Performs the light attack
    /// </summary>
    public virtual void LightAttack()
    {
        if(mAnimator != null)
        {
            mAnimator.SetBool("IsAttacking", true);
            mAnimator.SetInt("AttackType", (int)EAttackType.LIGHT);
        }
    }

    /// <summary>
    /// Performs the heavy attack
    /// </summary>
    public virtual void HeavyAttack()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// handles the character taking damage
    /// </summary>
    /// <param name="dp"></param>
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

    /// <summary>
    /// Performs block
    /// </summary>
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
            mAnimator.SetBool(PlayerAnimator.IS_BLOCKING, false);
    }
}