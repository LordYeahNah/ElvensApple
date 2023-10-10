using Godot;
using System;

public partial class SpikeTrap : Node3D
{
    public void OnBodyEnter(Node3D body)
    {
        if (body is PlayerController player)
        {
            player.TakeDamage(110, false);
        }
    }
}