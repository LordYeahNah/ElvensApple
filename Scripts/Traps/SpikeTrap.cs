using Godot;
using System;

public partial class SpikeTrap : Node3D
{
    [Export] private float mDamagePoints;                    // How many damage points are delta to the character
    public void OnBodyEnter(Node3D body)
    {
        if (body is PlayerController player)
        {
            player.TakeDamage(mDamagePoints, false);
        }
    }
}