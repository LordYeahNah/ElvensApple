using System;
using System.Collections.Generic;
using Godot;

public interface ICombat
{
    public void LightAttack();
    public void HeavyAttack();
    public void TakeDamage();
}