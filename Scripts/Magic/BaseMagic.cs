using System;
using Godot;

public enum EMagicVFXSpawn
{
    ABOVE_HEAD = 0,
    LEFT_HAND = 1,
}

public abstract class BaseMagic
{
    
    public string MagicName;                        // Name of the magic
    public Texture2D SpellIcon;
    public PackedScene VFX_Scene;                     // Reference to the VFX
    public EMagicVFXSpawn SpawnPoint;                       // Where the magic spawns from

    private bool mHasCooldown = false;
    private float mCooldownTime;

    public abstract void UseMagic();
}