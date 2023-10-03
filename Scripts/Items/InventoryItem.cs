using System;
using Godot;

public partial class InventoryItem : TextureButton
{
    public InventoryStack Item;
    [Export] private InventoryController mController;                       // Reference to the inventory controller
    [Export] private int mIndex;                            // Index of this button

    [ExportGroup("Indicators")]
    [Export] private TextureRect mEquippedIndicator;
    [Export] private TextureRect mCountIndicator;
    [Export] private Label mStackCount;

    public void Setup(InventoryStack item, bool equipped = false)
    {
        Item = item;
        this.TextureNormal = Item.Icon;

        if(item.CurrentStackSize > 1)
        {
            mCountIndicator.Visible = true;
            mStackCount.Text = item.CurrentStackSize.ToString();
        }

        if(equipped)
        {
            mEquippedIndicator.Visible = true;
        } else 
        {
            mEquippedIndicator.Visible = false;
        }
    }

    public void OnPressed()
    {
        mController.SelectItem(mIndex);
    }
}