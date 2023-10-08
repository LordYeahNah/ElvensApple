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

    public event Action OnDialogComplete;

    public DialogData(string id, string name, string message, Action dialogComplete)
    {
        OnDialogComplete += dialogComplete;
        mDialogID = id;
        DialogName = name;
        mDialogMessage = message;
    }

    public void CompleteDialog()
    {
        OnDialogComplete?.Invoke();
    }
}