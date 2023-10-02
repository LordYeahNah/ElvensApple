using System;
using System.Collections.Generic;
using Godot;

public enum EPropertyType
{
    INT = 0,
    FLOAT = 1,
    BOOL = 2
}

public abstract class AnimationProperty
{
    public string Key;
    protected EPropertyType mType;                      // What type of property this is
    public AnimationProperty(string key)
    {
        Key = key;
    }
}

public class AnimationInt : AnimationProperty
{
    public int Value;
    AnimationInt(string key, int value) : base(key)
    {
        Value = value;
        mType = EPropertyType.INT;
    }
}

public class AnimationFloat : AnimationProperty
{
    public float Value;
    AnimationFloat(string key, float value) : base(key)
    {
        Value = value;
        mType = EPropertyType.FLOAT;
    }


}

public class AnimationBool : AnimationProperty
{
    public bool Value;
    AnimationBool(string key, bool value) : base(key)
    {
        Value = value;
        mType = EPropertyType.BOOL;
    }
}