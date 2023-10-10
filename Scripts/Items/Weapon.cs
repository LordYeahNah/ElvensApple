using System;
using System.Collections.Generic;
using Godot;

public partial class Weapon : Equipable
{
    

    protected float mDamagePoints;                 // How many damage points this weapon deals
    protected float mCriticalHitChance = 0.0f;             // Chance of getting a critical hit for this item   
    protected float mHeavyAttackBoost = 1.3f;               // Additional Damage when performing heavy attack
    public bool IsHeavyAttack = false;                      // If we are to perform a heavy attack

    public float DamagePoints => mDamagePoints;

    public Weapon(string itemName, string desc, int cost, float damage, float critHit, Texture2D icon, PackedScene mesh) : base(itemName, false, desc, cost, icon, mesh)
    {
        mDamagePoints = damage;
        mCriticalHitChance = critHit;
    }

    private bool IsCriticalHit()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        float percentage = rand.Randf();
        if(percentage < mCriticalHitChance)
            return true;

        return false;
    }

    public float GetDamagePoints()
    {
        float dp = mDamagePoints;
        if(IsCriticalHit())
        {
            dp *= 1.2f;
        }

        // Check if heavy attack, if so then apply the boost to the damage points
        if(IsHeavyAttack)
            dp *= mHeavyAttackBoost;

        IsHeavyAttack = false;                      // Reset the heavy attack

        return dp;
    }
}