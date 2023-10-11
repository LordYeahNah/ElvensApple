using Godot;
using System;
using System.Collections.Generic;

public partial class SpellInventory : Control
{
    [Export] private PlayerController mPlayer;                      // Store reference to the player controller
    [ExportGroup("UI")]
    [Export] private MagicInventorySlot[] mSpellsSlot;

    [Export] private ColorRect mActionPanel;
    [Export] private Vector2 mPanelOffset;

    public void SetPlayer(PlayerController player) => mPlayer = player;

    private MagicInventorySlot mSelectedSlot;                       // reference to the slot we have selected

    public void SelectItem(MagicInventorySlot slot)
    {
        mSelectedSlot = slot;
        if (mActionPanel != null)
        {
            mActionPanel.Visible = true;
            mActionPanel.GlobalPosition = slot.GlobalPosition + mPanelOffset;
        }
    }

    public void EquipSlotOne()
    {
        if (mPlayer != null)
        {
            mPlayer.GetInventory().EquipSpell(mSelectedSlot.AssignedMagic, 1);
        }
        
        Redraw();
    }

    public void EquipSlotTwo()
    {
        if (mPlayer != null)
        {
            mPlayer.GetInventory().EquipSpell(mSelectedSlot.AssignedMagic, 1);
        }
        
        Redraw();
    }

    public void View()
    {
        
    }

    public void Setup()
    {

        if (mPlayer != null)
        {
            Inventory playerInv = mPlayer.GetInventory();
            if (playerInv != null && playerInv.SpellsObtained.Count > 0)
            {
                BaseMagic equippedSpellOnce = playerInv.EquippedSpellOnce;
                BaseMagic equippedSpellTwo = playerInv.EquippedSpellTwo;
                
                for(int i = 0; i < playerInv.SpellsObtained.Count; ++i)
                {
                    BaseMagic spell = playerInv.SpellsObtained[i];
                    if (spell != null)
                    {
                        if (spell == equippedSpellOnce || spell == equippedSpellTwo)
                        {
                            mSpellsSlot[i].Setup(this, spell, true);
                        }
                        else
                        {
                            mSpellsSlot[i].Setup(this, spell);
                        }
                    }
                }
            }
        }
    }

    public void Redraw()
    {
        foreach (var spell in mSpellsSlot)
        {
            spell.Setup(this ,null);
        }

        mSelectedSlot = null;
        mActionPanel.Visible = false;
        
        Setup();
    }
}