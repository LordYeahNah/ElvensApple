using System;
using Godot;

public partial class InventoryItem : TextureButton
{
    public BaseItem Item;
    [Export] private InventoryController mController;                       // Reference to the inventory controller
    [Export] private int mIndex;                            // Index of this button

    public void Setup(BaseItem item)
    {
        Item = item;
        this.TextureNormal = Item.Icon;
    }

    public void OnPressed()
    {
        mController.SelectItem(mIndex);
    }
}