using Godot;
using System;

public partial class SlimeTrap : Node3D
{
    [Export] private float mSpeedModifier;                      // How much slower the character will move
    private float mOrginalSpeedMod;                         // What their speed was prior to entering the slime

    public void OnBodyEnter(Node3D body)
    {
        if (body is PlayerController player)
        {
            mOrginalSpeedMod = player.SpeedModifier;
            player.SpeedModifier = mSpeedModifier;
        }
    }

    public void OnBodyExit(Node3D body)
    {
        if (body is PlayerController player)
        {
            player.SpeedModifier = mOrginalSpeedMod;
        }
    }
}