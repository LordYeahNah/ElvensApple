using System.Collections.Generic;
using System.Xml.Serialization;
using Godot;

public enum EAttachmentHand
{
    LEFT = 0,
    RIGHT = 1
}

public abstract class Equipable : BaseItem
{
    protected PackedScene mItemMesh;

    public PackedScene ItemMesh => mItemMesh;

    protected Equipable(string itemName, bool isConsumable, string desc, int cost, Texture2D icon, PackedScene mesh) : base(itemName, isConsumable, desc, cost, icon)
    {
        mItemMesh = mesh;
    }
}