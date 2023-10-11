using System;
using Godot;

public class HealSpell : BaseMagic
{
    private const float HEAL_POINTS = 25;
    public float SpellHealModifier = 1.0f;
    
    public HealSpell()
    {
        MagicName = "Heal";
        SpellIcon = GD.Load<Texture2D>("res://UI/painterly-spell-icons-1/heal-jade-3.png");
        VFX_Scene = GD.Load<PackedScene>("res://VFX/heal_vfx.tscn");
        SpawnPoint = EMagicVFXSpawn.ABOVE_HEAD;
        
    }

    public override void UseMagic()
    {
        if (SpellOwner != null)
        {
            SpellOwner.IncreaseHealth(HEAL_POINTS * SpellHealModifier);
        }
    }
}