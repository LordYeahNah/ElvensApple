using System;
using Godot;

public partial class InventoryItem : TextureButton
{
    public InventoryStack Item;
    [Export] private InventoryController mController;                       // Reference to the inventory controller
    [Export] private int mIndex;                            // Index of this button

    public void Setup(InventoryStack item)
    {
        Item = item;
        this.TextureNormal = Item.Icon;
    }

    public void OnPressed()
    {
        mController.SelectItem(mIndex);
    }
}