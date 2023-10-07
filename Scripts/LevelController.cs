using System;
using System.Collections.Generic;
using Godot;

public partial class LevelController : Node3D
{
    public static LevelController Instance;                 // Global instance of this class
    protected Node3D[] mLevelPathPoints;                        // Reference to all the random path points a character may move to

    public override void _Ready()
    {
        base._Ready();
    
        // Create a singleton for this class
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            this.QueueFree();
        }
    }

    /// <summary>
    /// Get the position of a random path point in the level
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomPosition()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        return mLevelPathPoints[rand.RandiRange(0, mLevelPathPoints.Length - 1)].Position;
    }
}