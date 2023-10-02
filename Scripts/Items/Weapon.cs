using System;
using System.Collections.Generic;
using Godot;

public partial class Weapon : Equipable
{
    

     protected float mDamagePoints;                 // How many damage points this weapon deals
     protected float mCriticalHitChance = 0.0f;             // Chance of getting a critical hit for this item   

    public Weapon(string itemName, string desc, int cost, float damage, float critHit) : base(itemName, false, desc, cost)
    {
        mDamagePoints = damage;
        mCriticalHitChance = critHit;
    }

}