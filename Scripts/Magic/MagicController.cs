using System;
using Godot;

public partial class MagicController : Node3D
{
    public BaseCharacter Owner;                     // reference to the character that owns this magic attack
    public BaseMagic AssignedMagic;                 // Magic that this class is attached to

    public void UseMagic()
    {
        if (AssignedMagic == null)
            return;
        
        AssignedMagic.UseMagic();
        if (AssignedMagic.VFX_Scene != null)
        {
            Vector3 spawnLocation = Vector3.Zero;               // pre-define the spawn location
            switch (AssignedMagic.SpawnPoint)
            {
                case EMagicVFXSpawn.LEFT_HAND:
                    spawnLocation = Owner.MagicLeftHandPoint;
                    break;
                case EMagicVFXSpawn.ABOVE_HEAD:
                    spawnLocation = Owner.MagicAboveHeadPoint;
                    break;
            }

            MagicParticleController particle = AssignedMagic.VFX_Scene.Instantiate<MagicParticleController>();
            particle.GlobalPosition = spawnLocation;
            particle.EmitParticle();
        }
    }
}