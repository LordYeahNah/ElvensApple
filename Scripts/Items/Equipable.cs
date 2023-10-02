using System.Collections.Generic;
using Godot;

public enum EAttachmentHand
{
    LEFT = 0,
    RIGHT = 1
}

public abstract class Equipable : BaseItem
{
    protected PackedScene mItemMesh;
    

    public abstract void Equip(EAttachmentHand hand);
}