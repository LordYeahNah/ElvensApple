using System;
using Godot;

public partial class InventoryItem : TextureButton
{
    public InventoryStack Item;
    [Export] private InventoryController mController;                       // Reference to the inventory controller
    [Export] private int mIndex;                            // Index of this button
    [Export] private bool mIsEquippedSlot;

    [ExportGroup("Indicators")]
    [Export] private TextureRect mEquippedIndicator;
    [Export] private TextureRect mCountIndicator;
    [Export] private Label mStackCount;

    public void Setup(InventoryStack item, bool equipped = false)
    {
        Item = item;
            
        if(item != null)
        {
            this.TextureNormal = Item.Icon;
            if(item.CurrentStackSize > 1 && mCountIndicator != null)
            {
                mCountIndicator.Visible = true;
                mStackCount.Text = item.CurrentStackSize.ToString();
            }

            if(equipped && mEquippedIndicator != null)
            {
                mEquippedIndicator.Visible = true;
            }
        } else 
        {
            this.TextureNormal = null;
            if(mEquippedIndicator != null)
                mEquippedIndicator.Visible = false;

            if(mCountIndicator != null)
                mCountIndicator.Visible = false;
        }
    }

    public void OnPressed()
    {
        if(mIsEquippedSlot)
        {
            mController.SelectItem(Item, this);
        } else
        {
            if(Item.Item is Weapon)
            {
                mController.SelectItem(mIndex, EItemType.WEAPON);
            }
        }
    }
}