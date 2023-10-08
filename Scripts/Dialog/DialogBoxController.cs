using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class DialogBoxController : Control
{
    private DialogData mCurrentDialog;

    [Export] private Label mDialogName;                 // name of the character that you have interacted with
    [Export] private Label mDialogMessage;
    
    

    public void Setup(DialogData data)
    {
        mCurrentDialog = data;
        mDialogMessage.Text = mCurrentDialog.DialogMessage;
        mDialogName.Text = mCurrentDialog.DialogName;
    }

    public void OnComplete()
    {
        mCurrentDialog.CompleteDialog();
    }
}