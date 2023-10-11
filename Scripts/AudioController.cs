using System;
using Godot.Collections;
using Godot;


public partial class AudioController : AudioStreamPlayer3D
{

    [Export] private Dictionary<string, AudioStream> mAudioTracks;

    public override void _Ready()
    {
        base._Ready();
    }

    public void PlayAudio(string trackName)
    {
        if (mAudioTracks.ContainsKey(trackName))
        {
            this.Stream = mAudioTracks[trackName];
            this.Play();
        }
    }
}