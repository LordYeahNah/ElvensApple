using System;
using System.Collections.Generic;
using Godot;

public enum EPropertyType
{
    INT = 0,
    FLOAT = 1,
    BOOL = 2,
    TRIGGER = 3,
}

public class AnimationProperty
{
    public string Key;
    public EPropertyType Type;                      // What type of property this is
    public AnimationProperty(string key, bool trigger = false)
    {
        Key = key;         
    }
}

public class AnimationInt : AnimationProperty
{
    public int Value;
    public AnimationInt(string key, int value) : base(key)
    {
        Value = value;
        Type = EPropertyType.INT;
    }
}

public class AnimationFloat : AnimationProperty
{
    public float Value;
    public AnimationFloat(string key, float value) : base(key)
    {
        Value = value;
        Type = EPropertyType.FLOAT;
    }


}

public class AnimationBool : AnimationProperty
{
    public bool Value;
    public AnimationBool(string key, bool value) : base(key)
    {
        Value = value;
        Type = EPropertyType.BOOL;
    }
}