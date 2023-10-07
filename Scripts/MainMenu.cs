using Godot;
using System;

public partial class MainMenu : Node
{
	private Timer mSplashTimer;
	private Timer mSplashFadeOutTimer;

	[Export] private AnimationPlayer mSplashAnim;					// Reference to the animator for the splash room
	[Export] private AnimationPlayer mMenuFadeIn;					// Reference to the animator for the main menu

	[Export] private Control mSplashScreen;
	[Export] private Control mMainMenu;
	
	public override void _Ready()
	{
		base._Ready();
		mSplashTimer = new Timer(5.0f, false, StartSplashFadeOut, true);
		mSplashFadeOutTimer = new Timer(1.0f, false, StartMenuFadeIn, false);
	}


	private void StartSplashFadeOut()
	{
		mSplashAnim.Play("SplashFadeOut");
		mSplashFadeOutTimer.IsActive = true;
	}

	private void StartMenuFadeIn()
	{
		mMenuFadeIn.Play("FadeIn");
		mSplashScreen.Visible = false;
		mMainMenu.Visible = false;
	}
}
