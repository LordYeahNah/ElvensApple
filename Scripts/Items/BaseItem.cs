using System;
using System.Collections.Generic;
using Godot;

public abstract class BaseItem
{
    protected string mItemName;                        // Name of the item
    protected bool mIsConsumable;                        // IF we can consume this (false means it's equippable)
    protected string mItemDescription;                 // A description of the item
    protected int mCost;                   // The value of the item
    protected Texture2D mIcon;

    public Texture2D Icon => mIcon;
    public string ItemName => mItemName;
    public bool IsConsumable => mIsConsumable;
    public string ItemDesc => mItemDescription;
    public int ItemCost => mCost;

    public BaseItem(string itemName, bool isConsumable, string desc, int cost, Texture2D icon)
    {
        mItemName = itemName;
        mIsConsumable = isConsumable;
        mItemDescription = desc;
        mCost = cost;
        mIcon = icon;
    }
}