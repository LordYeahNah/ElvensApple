using Godot;
using System;
using System.Collections.Generic;

public partial class SpellInventory : Control
{
    [Export] private PlayerController mPlayer;                      // Store reference to the player controller
    [ExportGroup("UI")]
    [Export] private MagicInventorySlot[] mSpellsSlot;

    public override void _Ready()
    {
        base._Ready();

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
                            mSpellsSlot[i].Setup(spell, true);
                        }
                        else
                        {
                            mSpellsSlot[i].Setup(spell);
                        }
                    }
                }
            }
        }
    }
}