using Godot;
using System;

public partial class Note : Node3D
{
    [Export] private string mNoteMessage;
    public string NoteMessage => mNoteMessage;
}
