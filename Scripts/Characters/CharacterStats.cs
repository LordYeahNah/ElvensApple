using System;
using Godot;

public class CharacterStats
{
    public const float MAX_HEALTH = 100;

    protected string mCharacterName;                    // name of the character
    protected float mCurrentHealth;                     // Current level of health
    public float CurrentHealth => mCurrentHealth;
    public bool IsAlive => mCurrentHealth > 0;                  // Check if the character is alive

    public CharacterStats(string characterName = "Skeleton")
    {
        mCharacterName = characterName;
        mCurrentHealth = MAX_HEALTH;
    }

    public void TakeDamage(float dp)
    {
        mCurrentHealth -= dp;
    }

    public void IncreaseHealth(float points)
    {
        mCurrentHealth += points;
        if (mCurrentHealth > MAX_HEALTH)
            mCurrentHealth = MAX_HEALTH;
    }
    
}