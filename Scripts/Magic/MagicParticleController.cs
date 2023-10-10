using System;
using Godot;

public partial class MagicParticleController : Node3D
{
    [Export] private GpuParticles3D mParticleSystem;

    public void EmitParticle()
    {
        if (mParticleSystem != null)
            mParticleSystem.Emitting = true;
    }
}