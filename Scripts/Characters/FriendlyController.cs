using System;
using ElvensApple.Scripts.Characters;
using Godot;

public partial class FriendlyController : BaseAI
{
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