using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class DialogBoxController : Control
{
    private DialogData mCurrentDialog;

    [Export] private Label mDialogName;                 // name of the character that you have interacted with
    [Export] private Label mDialogMessage;

    [ExportGroup("Timer Settings")] 
    [Export] private float mPrintTime;                      // How frequently letters are typed
    private Timer mPrintTimer;
    private bool mIsTyping;

    public bool IsTyping => mIsTyping;
    
    private int mMessageLength;
    private int mTotalLetters = 0;
    private string mCurrentMessage;                             // Reference to the dialog that has been printed
    public void Setup(DialogData data)
    {
        mCurrentDialog = data;
        mDialogName.Text = mCurrentDialog.DialogName;
        mPrintTimer = new Timer(mPrintTime, true, OnUpdateLetter, true);
        mIsTyping = true;
        mMessageLength = mCurrentDialog.DialogMessage.Length;
        mCurrentMessage = "";
        mTotalLetters = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(mPrintTimer != null)
            mPrintTimer.OnUpdate((float)delta);
    }

    private void OnUpdateLetter()
    {
        mCurrentMessage += mCurrentDialog.DialogMessage[mTotalLetters];
        mTotalLetters += 1;
        if (mTotalLetters >= mMessageLength)
        {
            mPrintTimer.IsActive = false;
            mIsTyping = false;
        }

        mDialogMessage.Text = mCurrentMessage;
    }

    public void CompleteMessage()
    {
        mCurrentMessage = mCurrentDialog.DialogMessage;
        mIsTyping = false;
        mPrintTimer.IsActive = false;
        mDialogMessage.Text = mCurrentMessage;
    }

    public void OnComplete()
    {
        mCurrentDialog.CompleteDialog();
    }
}