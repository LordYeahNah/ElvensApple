using System;
using System.Collections.Generic;
using Godot;

public partial class Weapon : BaseItem
{
    

     protected float mDamagePoints;                 // How many damage points this weapon deals
     protected float mCriticalHitChance = 0.0f;             // Chance of getting a critical hit for this item   
    public override void Action()
    {
        throw new NotImplementedException();
    }
}