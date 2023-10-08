using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class DialogBoxController : Control
{
    private DialogData mCurrentDialog;

    public void Setup(DialogData data)
    {
        mCurrentDialog = data;
    }

    public void OnComplete()
    {
        mCurrentDialog.CompleteDialog();
    }
}