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
    

    public BaseMagic(string name, PackedScene vfx, EMagicVFXSpawn spawnPoint, float cooldownTime = 0f)
    {
        MagicName = name;
        VFX_Scene = vfx;
        SpawnPoint = spawnPoint;
        if (cooldownTime > 0f)
            mHasCooldown = true;

        mCooldownTime = cooldownTime;
    }
    
    public abstract void UseMagic();
}