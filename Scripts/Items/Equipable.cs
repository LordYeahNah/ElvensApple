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
}