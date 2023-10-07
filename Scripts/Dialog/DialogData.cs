using System;
using System.Collections.Generic;
using Godot;

public class DialogData
{
    private string mDialogID;
    public string DialogID => mDialogID;

    private string mDialogMessage;
    public string DialogMessage => mDialogMessage;

    public event Action OnDialogComplete;

    public void CompleteDialog()
    {
        OnDialogComplete?.Invoke();
    }
}