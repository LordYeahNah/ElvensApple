using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class FriendlyController : BaseAI
{
    [ExportGroup("Interaction")] 
    [Export] protected MeshInstance3D mQuestMarker;
    [Export] protected MeshInstance3D mInteractionMarker;
    [Export] protected bool mCanInteract;

    public bool CanInteract => mCanInteract;

    protected PlayerController mPlayer;                         // Reference to the player controller
    
    
    // === DIALOG === //
    [ExportGroup("Dialog")] 
    [Export] protected string[] mDialogID;                      // Reference to all the dialogs by ID                       
    private List<DialogData> mDialog = new List<DialogData>();              // reference to all the dialogs
    
    public override void _Ready()
    {
        base._Ready();
        mAnimator = new BaseAI_Animator(mAnimPlayer, this);
    }

    protected override void CreateAI()
    {
        base.CreateAI();
        mTree = new FriendlyBT();
    }

    private void GetDialog()
    {
        if (mDialogID.Length > 0)
        {
            DialogDatabase db = GetNode<DialogDatabase>("/root/DialogDatabase");                // Get reference to the dialog database
            int level = LevelController.Instance.LevelNumber;                   // Get reference to the level number
            if (db != null)
            {
                // Loop throuh each id and get the dialog data
                foreach (var dialog in mDialogID)
                {
                    DialogData dialogRef = db.GetDialog(dialog, level);
                    mDialog.Add(dialogRef);                 // Add the new dialog
                }
            }
            
            // Add the dialog event
            foreach (var d in mDialog)
            {
                d.OnDialogComplete += FinishDialog;
            }
        }
    }

    private void CheckIfInteractable()
    {
        bool hasInteracted = true;                      // Predefine the flag
        foreach (var dialog in mDialog)
        {
            // Check if the dialog has already played, if it hasn't
            // break the loop
            if (!dialog.HaasPlayed)
            {
                hasInteracted = false;
                break;
            }
        }

        // if we still need to interact with character than display interaction marker
        if (!hasInteracted)
        {
            mInteractionMarker.Visible = true;
        }
    }

    public void InteractWith(PlayerController player)
    {
        mPlayer = player;
        if (mPlayer != null)
        {
            mPlayer.ShowDialogBox(mDialog[0]);
        }
    }

    public void FinishDialog()
    {
        if (mPlayer != null)
        {
            if (mDialog.Count > 1)
            {
                mPlayer.ShowDialogBox(mDialog[1]);
            }
            else
            {
                mPlayer.CloseDialogBox();
            }
            
            if(mDialog.Count > 0)
                mDialog.RemoveAt(0);
        }
    }

    protected override async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        GetDialog();
        CheckIfInteractable();
    }
}