using System;
using Godot;

public class Timer
{
    public float TimerLength;                   // The length of the timer
    private float mCurrentTime;                 // Reference to where the time is at
    private bool mLoop;                         // If the timer will loop
    public bool IsActive = true;                    // If the timer is active
    public event Action TimerComplete;                  // Events for when the timer is complete


    public Timer(float length, bool loop, Action onTimerComplete, bool isActive = true)
    {
        TimerLength = length;
        mLoop = loop;
        IsActive = isActive;
        TimerComplete += onTimerComplete;
    }

    public void OnUpdate(float delta)
    {
        if (!IsActive)
            return;

        mCurrentTime += 1 * delta;
        if (mCurrentTime > TimerLength)
            OnTimerComplete();
    }

    public void Restart(bool active = false)
    {
        mCurrentTime = 0f;
        IsActive = active;
    }

    private void OnTimerComplete()
    {
        mCurrentTime = 0f;
        IsActive = mLoop;
        TimerComplete?.Invoke();
    }
}