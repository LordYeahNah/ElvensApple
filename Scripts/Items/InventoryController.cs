using System;
using System.Collections.Generic;
using Godot;

public partial class InventoryController : Control
{
    
    [Export] private TextureButton[] mSlotItems;

    [ExportGroup("Action Panel")]
    [Export] private Panel mActionPanel;
    [Export] private Vector2 mPanelOffset;


    public void SelectItem(int itemIndex)
    {
        TextureButton selected = mSlotItems[itemIndex];
        if(selected != null)
        {
            if(mActionPanel != null)
            {
                mActionPanel.Visible = true;
                mActionPanel.Position = selected.Position + mPanelOffset;
            }
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
    }
}