using System;
using System.Collections.Generic;
using Godot;

public enum EPlayDirection
{
    FORWARD = 0,
    BACK = 1
}

public class Animation
{
    protected string mAnimationName;                        // Name of the animation we will play
    public string Name => mAnimationName;
    protected AnimationPlayer mPlayer;                      // Reference to the animation player to play the animation
    protected AnimationController mAnimController;              // Reference to the controller that owns this animator
    // List of animations we can transition to
    public List<Animation> TransitionAnimations = new List<Animation>();                // List of animations that this animation can transition to
    // Properties required to transition to this animation
    protected List<AnimationProperty> mRequiredProperties = new List<AnimationProperty>();   
    public List<AnimationProperty> RequiredProperties => mRequiredProperties;    

    public List<AnimationEvent> AnimEvents = new List<AnimationEvent>();

    public bool IsPlaying = false;
    public bool ShouldPlay = false;

    // ANIMATION TIME //
    private float mCurrentTime;

    protected bool mIsPaused = false;                       // if the animation is currently paused 

    public Animation(string animationName, AnimationPlayer animPlayer, AnimationController anim)
    {
        mAnimationName = animationName;
        mPlayer = animPlayer;
        mAnimController = anim;
    }

    /// <summary>
    /// Plays the animation
    /// </summary>
    /// <param name="playDirection"></param>
    protected virtual void PlayAnimation(EPlayDirection playDirection = EPlayDirection.FORWARD)
    {
        if(mPlayer == null || mIsPaused)
            return;

        IsPlaying = true;

        if(playDirection == EPlayDirection.FORWARD)
        {
            mPlayer.Play(mAnimationName);
        } else if(playDirection == EPlayDirection.BACK)
        {
            mPlayer.PlayBackwards(mAnimationName);
        }
    }

    public void StopAnimation()
    {
        if(mAnimationName == "Block")
            GD.Print("Stopping Block Animation");

        ShouldPlay = false;
        IsPlaying = false;
    }

    public void OnUpdate(float delta)
    {


        if(ShouldPlay)
        {
            if(!IsPlaying)
            {
                PlayAnimation();
            } else 
            {
                
                mCurrentTime += 1 * delta;                  // Increment the timer to calculate how much of the animation has been played
                // Check if the animation has player
                if(mCurrentTime >= mPlayer.CurrentAnimationLength)
                {
                    AnimationComplete();                    // Perform any actions for when the animation is completed
                    mCurrentTime = 0f;                      // Reset the current timer

                    // Reset each event
                    foreach(var e in AnimEvents)
                        e.HasPlayed = false;
                }

                // Loop through each event to see if an event should occur
                foreach(var e in AnimEvents)
                {
                    if(e.AnimationEventTime > mCurrentTime)
                    {
                        e.PlayEvent();
                    }
                        
                }
            }
        }
    }

    public void SetPlayMove(bool paused)
    {
        mIsPaused = paused;
        if(mIsPaused)
            mPlayer.Pause();
        else
        PlayAnimation();
    }

    protected virtual void AnimationComplete()
    {

    }
}

public class TriggerAnimation : Animation
{
    public TriggerAnimation(string animationName, AnimationPlayer animPlayer, AnimationController mAnim) : base(animationName, animPlayer, mAnim)
    {
    }

    protected override void AnimationComplete()
    {
        base.AnimationComplete();
        if(TransitionAnimations.Count > 0)
        {
            mAnimController.SetCurrentAnimation(TransitionAnimations[0]);
        } else 
        {
            StopAnimation();
        }
    }
}

public class AnimationEvent
{
    public float AnimationEventTime;
    private event Action mEventAction;
    public bool HasPlayed = false;

    public AnimationEvent(float time, Action e)
    {
        AnimationEventTime = time;
        mEventAction = e;
    }

    public void PlayEvent()
    {
        if(!HasPlayed)
            mEventAction?.Invoke();

        HasPlayed = true;
    }
}