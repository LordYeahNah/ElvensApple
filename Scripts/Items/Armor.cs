using System;
using System.Collections.Generic;
using Godot;

public class Armor : Equipable
{
    private float mReductionModifier = 1.0f;
    public float ReductionModifier => mReductionModifier;
    public Armor(string itemName, bool isConsumable, string desc, int cost, Texture2D icon, PackedScene mesh, float reduction) : base(itemName, isConsumable, desc, cost, icon, mesh)
    {
        mReductionModifier = reduction;
    }
}