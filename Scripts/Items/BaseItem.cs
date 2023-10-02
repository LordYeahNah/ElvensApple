using System;
using System.Collections.Generic;
using Godot;

public abstract class BaseItem
{
    protected string mItemName;                        // Name of the item
    protected string mIsConsumable;                        // IF we can consume this (false means it's equippable)
    protected string mItemDescription;                 // A description of the item
    protected int mCost;                   // The value of the item

    /// <summary>
    /// Consume or equip this item
    /// </summary>
    public abstract void Action();
}