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
    private List<InventoryStack> mItems = new List<InventoryStack>();
    public List<InventoryStack> Items => mItems;

    private int mInventorySize;
    private int mMaxInventorySize;
    
    public bool AddItem(BaseItem item)
    {
        if(item == null)
            return false;

        bool addedToStack = false;                  // If this item has been added to a stack
        if(HasItem(item))
        {
            foreach(var i in mItems)
            {
                if(item == i.Item)
                {
                    if(CanAddToStack(i))
                    {
                        i.CurrentStackSize += 1;                // Added the item to the stack
                        return true;
                    }
                        
                }
            }
        }

        if(mInventorySize < mMaxInventorySize)
        {
            InventoryStack stack = new InventoryStack();
            stack.Item = item;
            stack.CurrentStackSize = 1;
            stack.MaxStackSize = GetMaxStackSize(item);
            return true;
        }

        return false;
    }

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

    public bool CanAddToStack(InventoryStack stack)
    {
        foreach(var i in mItems)
        {
            if(i.CurrentStackSize < i.MaxStackSize)
                return true;
        }

        return false;
    }

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

    public bool HasItem(BaseItem item)
    {
        foreach(var i in mItems)
        {
            if(i.Item == item)
                return true;
        }

        return false;
    }

}