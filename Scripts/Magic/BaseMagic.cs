using System;
using Godot;

public enum EMagicVFXSpawn
{
    ABOVE_HEAD = 0,
    LEFT_HAND = 1,
}

public abstract class BaseMagic
{
    public BaseCharacter SpellOwner;
    public string MagicName;                        // Name of the magic
    public Texture2D SpellIcon;
    public PackedScene VFX_Scene;                     // Reference to the VFX
    public EMagicVFXSpawn SpawnPoint;                       // Where the magic spawns from

    public bool CanUseSpell = true;                        // If this is spell can be used

    public float ManaUse;                               // How much mana is used

    protected Timer mCooldownTimer;

    public virtual void OnUpdate(float delta)
    {
        if(mCooldownTimer != null)
            mCooldownTimer.OnUpdate(delta);
    }
    
    public abstract void UseMagic();

    protected void ResetSpell()
    {
        CanUseSpell = true;
    }
}