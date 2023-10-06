using System;
using System.Collections.Generic;
using Godot;

public enum EAttackType
{
    NONE = 0,
    LIGHT = 1,
    HEAVY = 2,
}

public interface ICombat
{
    public void LightAttack();
    public void HeavyAttack();
    public void TakeDamage(float dp, bool armorReduction = true);
    public void Block();
    public void StopBlocking();
}