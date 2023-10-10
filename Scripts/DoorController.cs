using System;
using Godot;

public partial class DoorController : Node3D
{
    [Export] private AudioStreamPlayer3D mAudio;
    [Export] private AnimationPlayer mAnimation;

    public void OpenDoor()
    {
        mAudio.Play();
        mAnimation.Play();
    }
}