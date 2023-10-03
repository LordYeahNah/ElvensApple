using System;
using System.Collections.Generic;
using Godot;

public partial class InventoryController : Control
{
    private Inventory mViewingInv;
    private BaseItem mSelectedItem;
    
    [Export] private TextureButton[] mSlotItems;

    [ExportGroup("Action Panel")]
    [Export] private Panel mActionPanel;
    [Export] private Vector2 mPanelOffset;

    public void Setup(Inventory inv)
    {
        mViewingInv = inv;
        for(int i = 0; i < inv.Items.Count; ++i)
        {
            InventoryItem item = (InventoryItem)mSlotItems[i];
            if(item != null)
                item.Setup(inv.Items[i].Item);
        }
    }


    public void SelectItem(int itemIndex)
    {
        TextureButton selected = mSlotItems[itemIndex];
        if(selected != null)
        {
            // Set the selected item
            InventoryItem invSelectedItem = (InventoryItem)selected;
            if(invSelectedItem != null)
                mSelectedItem = invSelectedItem.Item;

            if(mActionPanel != null)
            {
                mActionPanel.Visible = true;
                mActionPanel.Position = selected.Position + mPanelOffset;
            }
        }
    }   

    public void EquipLeftHand()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem is Equipable eItem)
        {
            mViewingInv.EquipItem(eItem, EAttachmentHand.LEFT);
        }
    }

    public void EquipRightHand()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem is Equipable eItem)
        {
            mViewingInv.EquipItem(eItem, EAttachmentHand.RIGHT);
        }
    }

    public void CloseInventory()
    {
        mActionPanel.Visible = false;
        this.Visible = false;

        foreach(var btn in mSlotItems)
        {
            btn.TextureNormal = null;
        }

        mSelectedItem = null;
        mViewingInv = null;
    }
}