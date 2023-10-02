using System.Collections.Generic;
using System;
using Godot;

public class Blackboard
{
    private List<BlackboardProperty> mProperties = new List<BlackboardProperty>();

    public void SetValue<T>(string key, T Value)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key)
            {
                if(prop is BlackboardValue<T> v)
                {
                    v.Value = Value;
                    return;
                }
            }
        }

        mProperties.Add(new BlackboardValue<T>(key, Value));
    }

    public T GetValue<T>(string key)
    {
        foreach(var prop in mProperties)
        {
            if(prop.Key == key && prop is BlackboardValue<T> propValue)
            {
                return propValue.Value;
            }
        }

        return default;
    }
}

public abstract class BlackboardProperty
{
    public string Key;
    public BlackboardProperty(string key)
    {
        Key = key;
    }
}

public class BlackboardValue<T> : BlackboardProperty
{
    public T Value;

    public BlackboardValue(string key, T value) : base(key)
    {
        Value = value;
    }
}