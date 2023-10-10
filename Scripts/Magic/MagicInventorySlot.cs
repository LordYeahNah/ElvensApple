using System;
using Godot;

public partial class MagicInventorySlot : TextureRect
{
    [Export] private TextureRect mEquippedIndicator;

    private BaseMagic mAssignedMagic;

    public void Setup(BaseMagic magic, bool isEquipped = false)
    {
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

        this.Texture = magic.SpellIcon;                 // Set the spell texture
    }
}