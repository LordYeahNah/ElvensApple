using System;
using Godot;

public class CharacterStats
{
    public const float MAX_HEALTH = 100;
    public const float MAX_MANA = 100;
    public const float MANA_REGEN_WAIT = 5.0f;
    public const float MANA_REGEN_TIME = 0.3f;
    public const float MANA_REGEN_AMOUNT = 1.0f;

    protected string mCharacterName;                    // name of the character
    
    // === HEALTH === //
    protected float mCurrentHealth;                     // Current level of health
    public float CurrentHealth => mCurrentHealth;
    public bool IsAlive => mCurrentHealth > 0;                  // Check if the character is alive
    
    // === MANA === //
    private float mCurrentMana;
    public float CurrentMana => mCurrentMana;
    private Timer mStartManaRegenTimer;                 // Time to wait before regen
    private Timer mManaRegenTimer;                      // Time when regeneration
    private bool mCanRegenMana = false;                 // if we can regen mana

    public CharacterStats(string characterName = "Skeleton")
    {
        mCharacterName = characterName;
        mCurrentHealth = MAX_HEALTH;
        mCurrentMana = MAX_MANA;

        mStartManaRegenTimer = new Timer(MANA_REGEN_WAIT, false, StartManaRegen, false);
        mManaRegenTimer = new Timer(MANA_REGEN_TIME, true, RegenMana, false);
    }

    public void OnUpdate(float delta)
    {
        if (mStartManaRegenTimer != null)
            mStartManaRegenTimer.OnUpdate(delta);
        
        if(mManaRegenTimer != null)
            mManaRegenTimer.OnUpdate(delta);
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

    public void UseMana(float manaPoints)
    {
        mCurrentMana -= manaPoints;
    }

    public void RegenMana()
    {
        mCurrentMana += MANA_REGEN_AMOUNT;
        if (mCurrentMana > MAX_MANA)
        {
            mCurrentMana = MAX_MANA;
            mCanRegenMana = false;
        }
    }

    public void StartManaRegen()
    {
        if(mManaRegenTimer != null)
            mManaRegenTimer.Restart(true);

        mStartManaRegenTimer.IsActive = false;
    }

    public void ResetManaWait()
    {
        if (mStartManaRegenTimer != null)
            mStartManaRegenTimer.Restart(true);

        mManaRegenTimer.IsActive = false;
    }
    
}