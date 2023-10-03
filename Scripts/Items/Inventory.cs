using System;
using System.Collections.Generic;
using Godot;

public class InventoryStack
{
    public BaseItem Item;
    public int CurrentStackSize;
    public int MaxStackSize;
}

public class Inventory
{
    private List<InventoryStack> mItems = new List<InventoryStack>();                   // List of sttack items
    public List<InventoryStack> Items => mItems;

    private int mInventorySize;             // The current size of the inventory
    private int mMaxInventorySize;              // The max size of the inventory

    private BoneAttachment3D mLeftHandAttachment;
    private BoneAttachment3D mRightHandAttachment;
    private Node3D mInRightHand;
    private Node3D mInLeftHand;

    // Equipped settings
    private Equipable mEquippedLeftHand;
    private Equipable mEquippedRight;

    public Inventory(int inventorySize, BoneAttachment3D leftHand, BoneAttachment3D rightHand)
    {
        mMaxInventorySize = inventorySize;
        mLeftHandAttachment = leftHand;
        mRightHandAttachment = rightHand;
    }
    
    /// <summary>
    /// Adds an item to the inventory
    /// </summary>
    /// <param name="item">Item to add to the inventory</param>
    /// <returns>If the item was added to the inventory</returns>
    public bool AddItem(BaseItem item)
    {
        if(item == null)
            return false;

        bool addedToStack = false;                  // If this item has been added to a stack
        if(HasItem(item))
        {
            foreach(var i in mItems)
            {
                // Check the item to see if it matches
                if(item == i.Item)
                {
                    // Check if the item can be added to the inventory
                    if(CanAddToStack(i))
                    {
                        i.CurrentStackSize += 1;                // Added the item to the stack
                        return true;
                    }
                        
                }
            }
        }

        // Check if there is remove for the inventory
        if(mInventorySize < mMaxInventorySize)
        {
            InventoryStack stack = new InventoryStack();            // Create a new stack
            stack.Item = item;                  // Set the item
            stack.CurrentStackSize = 1;                 // Set the current size of the stack
            stack.MaxStackSize = GetMaxStackSize(item);             // Determine the max size of the stack
            mItems.Add(stack);
            return true;
        }

        return false;                   // No room to add the item 
    }

    /// <summary>
    /// Determines the size of the inventory stack
    /// </summary>
    /// <param name="item">Item to get the item type</param>
    /// <returns>Max size of the inventory stack</returns>
    private int GetMaxStackSize(BaseItem item)
    {
        if(item is Weapon)
        {
            return 1;
        } else 
        {
            return 15;
        }
    }

    /// <summary>
    /// Checks to see if the item can be added to an existing stack
    /// </summary>
    /// <param name="stack">Stack to check</param>
    /// <returns>If the item can be added to this stack</returns>
    public bool CanAddToStack(InventoryStack stack)
    {
        foreach(var i in mItems)
        {
            if(i.CurrentStackSize < i.MaxStackSize)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the item from the inventory stack or the inventory itself.
    /// </summary>
    /// <param name="stackItem">Stack item to remove/remove from</param>
    /// <returns></returns>
    public bool RemoveItem(InventoryStack stackItem)
    {
        if(stackItem.CurrentStackSize == 1)
        {
            mItems.Remove(stackItem);
            return true;
        }

        stackItem.CurrentStackSize -= 1;
        return true;
    }

    /// <summary>
    /// Checks if the inventory has the specified item
    /// </summary>
    /// <param name="item">Item to check</param>
    /// <returns>If the item exist in the inventory</returns>
    public bool HasItem(BaseItem item)
    {
        foreach(var i in mItems)
        {
            if(i.Item == item)
                return true;
        }

        return false;
    }

    public void EquipItem(Equipable item, EAttachmentHand hand)
    {
        Node3D spawned = item.ItemMesh.Instantiate<Node3D>();
        if(hand == EAttachmentHand.LEFT)
        {
            if(mInLeftHand != null)
            {
                mInLeftHand.QueueFree();
            }

            if(item != null)
            {
                mEquippedLeftHand = item;
                mInLeftHand = spawned;
                mLeftHandAttachment.AddChild(spawned);
            }
        } else 
        {
            if(mInRightHand != null)
            {
                mInRightHand.QueueFree();
            }

            if(item != null)
            {
                mEquippedRight = item;
                mInRightHand = spawned;
                mRightHandAttachment.AddChild(spawned);
            }
        }
    }

}