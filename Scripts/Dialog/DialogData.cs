using System;
using System.Collections.Generic;
using Godot;

public class DialogData
{
    private string mDialogID;
    public string DialogID => mDialogID;
    public string DialogName;

    private string mDialogMessage;
    public string DialogMessage => mDialogMessage;
    public string NextDialogData;
    public bool HaasPlayed = false;                         // If the dialog has already played
    public event Action OnDialogComplete;
    

    public DialogData(string id, string name, string message, string nextDialog, Action dialogComplete = null)
    {
        OnDialogComplete += dialogComplete;
        mDialogID = id;
        DialogName = name;
        NextDialogData = nextDialog;
        mDialogMessage = message;
    }

    public void CompleteDialog()
    {
        OnDialogComplete?.Invoke();
    }
}