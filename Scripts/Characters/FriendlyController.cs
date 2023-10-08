using System;
using System.Collections.Generic;
using Godot;

public partial class FriendlyController : BaseAI
{
    [ExportGroup("Interaction")] 
    [Export] protected MeshInstance3D mQuestMarker;
    [Export] protected MeshInstance3D mInteractionMarker;
    [Export] protected bool mCanInteract;

    public bool CanInteract => mCanInteract;
    
    
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
}