using System;
using Godot;

public partial class MagicInventorySlot : TextureButton
{
    private SpellInventory mInv;
    [Export] private TextureRect mEquippedIndicator;

    private BaseMagic mAssignedMagic;
    public BaseMagic AssignedMagic => mAssignedMagic;

    public void Setup(SpellInventory inv, BaseMagic magic, bool isEquipped = false)
    {
        mInv = inv;
        mAssignedMagic = magic;                 // Set the magic
        // Determine if we need to display the indicator 
        if (isEquipped)
        {
            mEquippedIndicator.Visible = true;
        }
        else
        {
            mEquippedIndicator.Visible = false;
        }

        if (magic != null)
        {
            this.TextureNormal = magic.SpellIcon;                 // Set the spell texture
        }
        else
        {
            this.TextureNormal = null;
        }
    }

    public void Select()
    {
        mInv.SelectItem(this);
    }
}