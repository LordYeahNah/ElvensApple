using System;
using System.Collections.Generic;
using Godot;

public partial class BaseCharacter : CharacterBody3D, ICombat
{
    // === CHARACTER STATS === //
    protected CharacterStats mStats;
    public CharacterStats Stats => mStats;
    public float CurrentHealth => mStats.CurrentHealth;

    public event Action ATakeDamage;

    // === CURRENT STATE === //
    public bool IsBlocking;
    public bool IsAttacking;

    [ExportGroup("Inventory")]
    [Export] protected InventoryController mInventoryPanel;
    protected bool mIsInventoryOpen = false;
    protected Inventory mInventory;
    [Export] protected BoneAttachment3D mLeftHand;
    [Export] protected BoneAttachment3D mRightHand;

    public bool IsInventoryOpen => mIsInventoryOpen;

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
        if(mInventory.EquippedRightHand == null || IsAttacking)
            return;


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
        if(mInventory.EquippedRightHand == null || IsAttacking)
            return;

        if(mAnimator != null)
        {
            mAnimator.SetBool("IsAttacking", true);
            mAnimator.SetInt("AttackType", (int)EAttackType.HEAVY);
        }

        // Trigger heavy attack
        if(mInventory.EquippedRightHand is Weapon w)
        {
            w.IsHeavyAttack = true;
        }
    }

    /// <summary>
    /// handles the character taking damage
    /// </summary>
    /// <param name="dp"></param>
    public virtual void TakeDamage(float dp, bool armorReduction = true)
    {
        // If armor equipped apply the duction on take damage
        if(mInventory != null && mInventory.EquippedArmor != null)
        {
            Armor equippedArmor = (Armor)mInventory.EquippedArmor;
            if(equippedArmor != null)
                dp *= equippedArmor.ReductionModifier;
        }
        // Take damage from the character stats
        if(mStats != null)
        {
            mStats.TakeDamage(dp);
            if(!mStats.IsAlive)
            {
                mAnimator.SetBool("IsAlive", false);
            }
        }

        ATakeDamage?.Invoke();
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