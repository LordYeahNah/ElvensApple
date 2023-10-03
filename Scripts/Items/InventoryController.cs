using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

public enum EItemType
{
    WEAPON = 0,
    CONSUMABLE = 1,
    EQUIPPED = 2
}

public partial class InventoryController : Control
{
    private Inventory mViewingInv;
    private InventoryStack mSelectedItem;
    
    [Export] private TextureButton[] mSlotItems;

    [ExportGroup("Equipped Panel")]
    [Export] private InventoryItem mEquippedLeftHand;
    [Export] private InventoryItem mEquippedRightHand;
    [Export] private InventoryItem mEquippedArmor;
    [Export] private InventoryItem mQuickAccessLeft;
    [Export] private InventoryItem mQuickAccessRight;

    [ExportGroup("Action Panel")]
    [Export] private Panel mActionPanel;
    [Export] private Panel mConsumablePanel;
    [Export] private Panel mEquippedPanel;
    [Export] private Vector2 mPanelOffset;

    public void Setup(Inventory inv)
    {
        mViewingInv = inv;                      // Set reference for the inventory
        // Get the current equipped items
        BaseItem eRightHand = mViewingInv.EquippedRightHand;
        BaseItem eLeftHand = mViewingInv.EquippedLeftHand;

        bool isEquipped = false;

        // Loop through each item in the inventory
        for(int i = 0; i < inv.Items.Count; ++i)
        {
            BaseItem item = inv.Items[i].Item;                  // Get the base item of the stack

            // Check if equipped
            if(item == eRightHand)
            {
                mEquippedRightHand.Setup(inv.Items[i]);
                isEquipped = true;
            } else if(item == eLeftHand)
            {
                mEquippedLeftHand.Setup(inv.Items[i]);
                isEquipped = true;
            }

            InventoryItem itemSlot = (InventoryItem)mSlotItems[i];              // Get the base item slot
            // If valid then setup the slot with the item
            if(itemSlot != null)
                itemSlot.Setup(inv.Items[i], isEquipped);
                
        }
    }

    public void SelectItem(InventoryStack item, InventoryItem slot)
    {
        mSelectedItem = item;
        HideAllPanels();
        mEquippedPanel.Visible = true;
        mEquippedPanel.Position = slot.Position + mPanelOffset;
    }


    public void SelectItem(int itemIndex, EItemType type)
    {
        TextureButton selected = mSlotItems[itemIndex];
        if(selected != null)
        {
            // Set the selected item
            InventoryItem invSelectedItem = (InventoryItem)selected;
            if(invSelectedItem != null)
                mSelectedItem = invSelectedItem.Item;

            HideAllPanels();
            switch(type)
            {
                case EItemType.WEAPON:
                    if(mActionPanel != null)
                    {
                        mActionPanel.Visible = true;
                        mActionPanel.Position = selected.Position + mPanelOffset;
                    }
                    break;
                case EItemType.EQUIPPED:
                    if(mEquippedPanel != null)
                    {
                        mEquippedPanel.Visible = true;
                        mEquippedPanel.Position = selected.Position + mPanelOffset;
                    }
                    break;
                case EItemType.CONSUMABLE:
                    if(mConsumablePanel != null)
                    {
                        mConsumablePanel.Visible = true;
                        mConsumablePanel.Position = selected.Position + mPanelOffset;
                    }
                    break;
            }
        }
    }  

    private void HideAllPanels()
    {
        if(mActionPanel != null)
            mActionPanel.Visible = false;

        if(mConsumablePanel != null)
            mConsumablePanel.Visible = false;
            
        if(mEquippedPanel != null)    
            mEquippedPanel.Visible = false;

    } 

    public void EquipLeftHand()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem.Item is Equipable eItem)
        {
            mViewingInv.EquipItem(eItem, EAttachmentHand.LEFT);
        }

        RedrawInventory();

    }

    public void EquipRightHand()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem.Item is Equipable eItem)
        {
            mViewingInv.EquipItem(eItem, EAttachmentHand.RIGHT);
        }
        RedrawInventory();
    }

    public void OnDrop()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mViewingInv.RemoveItem(mSelectedItem))
        {
            RedrawInventory();
        }
    }

    public void CloseInventory()
    {
        mActionPanel.Visible = false;
        this.Visible = false;

        foreach(var btn in mSlotItems)
        {
            if(btn is InventoryItem item)
                item.Setup(null);
        }

        mEquippedLeftHand.Setup(null);
        mEquippedRightHand.Setup(null);
        mEquippedArmor.Setup(null);

        mSelectedItem = null;
        mViewingInv = null;
    }

    private void RedrawInventory()
    {
        mActionPanel.Visible = false;
        foreach(var btn in mSlotItems)
        {
            if(btn is InventoryItem item)
                item.Setup(null);
        }

        mSelectedItem = null;

        mEquippedLeftHand.Setup(null);
        mEquippedRightHand.Setup(null);
        mEquippedArmor.Setup(null);

        Setup(mViewingInv);
    }
}