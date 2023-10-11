using System;
using Godot;

public partial class MagicController : Node3D
{
    public BaseCharacter Owner;                     // reference to the character that owns this magic attack
    private Timer mDestroyTimer;
    [Export] private float mDestroyTime;
    
    [Export] private GpuParticles3D mParticleSystem;

    

    public override void _Ready()
    {
        mDestroyTimer = new Timer(mDestroyTime, false, () => { this.QueueFree(); }, true);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(mDestroyTimer != null)
            mDestroyTimer.OnUpdate((float)delta);
    }
    
    public void EmitParticle()
    {
        if (mParticleSystem != null)
            mParticleSystem.Emitting = true;
    }

    public void UseMagic(BaseMagic magic, BaseCharacter user)
    {
        if (magic == null)
            return;
        
        magic.UseMagic();
        if (magic.VFX_Scene != null)
        {
            Vector3 spawnLocation = Vector3.Zero;               // pre-define the spawn location
            switch (magic.SpawnPoint)
            {
                case EMagicVFXSpawn.LEFT_HAND:
                    spawnLocation = user.MagicLeftHandPoint;
                    break;
                case EMagicVFXSpawn.ABOVE_HEAD:
                    spawnLocation = user.MagicAboveHeadPoint;
                    break;
            }
            
            EmitParticle();
        }
    }
}