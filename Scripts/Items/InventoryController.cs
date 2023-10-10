using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Godot;

public enum EItemType
{
    WEAPON = 0,
    CONSUMABLE = 1,
    EQUIPPED = 2,
    ARMOR = 3,
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
    [Export] private Panel mArmorPanel;
    [Export] private Vector2 mPanelOffset;

    [ExportGroup("Viewing Item")] 
    [Export] private Control mViewingPanel;                  // Reference to the viewing panel
    [Export] private TextureRect mViewingItemIcon;
    [Export] private Label mViewingItemName;
    [Export] private Label mViewingItemDescription;
    [Export] private Label mViewingItemStatHeader;
    [Export] private Label mViewingItemStatValue;

    public void Setup(Inventory inv)
    {
        mViewingInv = inv;                      // Set reference for the inventory
        // Get the current equipped items
        BaseItem eRightHand = mViewingInv.EquippedRightHand;
        BaseItem eLeftHand = mViewingInv.EquippedLeftHand;
        BaseItem eArmor = mViewingInv.EquippedArmor;

        

        // Loop through each item in the inventory
        for(int i = 0; i < inv.Items.Count; ++i)
        {
            bool isEquipped = false;
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
            } else if(item == eArmor)
            {
                mEquippedArmor.Setup(inv.Items[i]);
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
        mEquippedPanel.Position = slot.GlobalPosition + mPanelOffset;
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
                        mActionPanel.Position = selected.GlobalPosition + mPanelOffset;
                    }
                    break;
                case EItemType.EQUIPPED:
                    if(mEquippedPanel != null)
                    {
                        mEquippedPanel.Visible = true;
                        mEquippedPanel.Position = selected.GlobalPosition + mPanelOffset;
                    }
                    break;
                case EItemType.CONSUMABLE:
                    if(mConsumablePanel != null)
                    {
                        mConsumablePanel.Visible = true;
                        mConsumablePanel.Position = selected.GlobalPosition + mPanelOffset;
                    }
                    break;
                case EItemType.ARMOR:
                    if(mArmorPanel != null)
                    {
                        mArmorPanel.Visible = true;
                        mArmorPanel.Position = selected.GlobalPosition + mPanelOffset;
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

    public void EquipArmor()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem.Item is Equipable eItem)
        {
            mViewingInv.EquipItem(eItem, EAttachmentHand.BODY);
        }

        RedrawInventory();
    }

    public void OnView()
    {
        if (mSelectedItem == null)
        {
            mViewingPanel.Visible = false;
            return;
        }

        mViewingPanel.Visible = true;
        mViewingItemIcon.Texture = mSelectedItem.Icon;
        mViewingItemName.Text = mSelectedItem.Item.ItemName;
        mViewingItemDescription.Text = mSelectedItem.Item.ItemDesc;

        if (mSelectedItem.Item is Weapon weapon)
        {
            mViewingItemStatHeader.Text = "Damage Points";
            mViewingItemStatValue.Text = weapon.DamagePoints.ToString("N");
        } else if (mSelectedItem.Item is Armor armor)
        {
            mViewingItemStatHeader.Text = "Resistance";
            int reductionPoints = Mathf.FloorToInt(armor.ReductionModifier * 100);
            mViewingItemStatValue.Text = $"{reductionPoints} %";
        }
    }

    public void OnDrop()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem.Item == mViewingInv.EquippedLeftHand || mSelectedItem.Item == mViewingInv.EquippedRightHand)
        {
            OnUnequip();
        }

        if(mViewingInv.RemoveItem(mSelectedItem))
        {
            RedrawInventory();
        }
    }

    public void OnUnequip()
    {
        if(mSelectedItem == null || mViewingInv == null)
            return;

        if(mSelectedItem.Item is Weapon weapon)
        {
            mViewingInv.EquipItem(null, EAttachmentHand.RIGHT);
        }

        RedrawInventory();
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