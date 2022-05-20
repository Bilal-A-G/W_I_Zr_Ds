using System;
using UnityEngine;

[Serializable]
public class GenericReference<T>
{
    [SerializeField]
    bool useOverride;
    [SerializeField]
    bool isFolded;

    [SerializeField]
    GenericValue<T> variableValue;
    [SerializeField]
    T overrideValue;

    public T GetValue() => useOverride ? overrideValue : variableValue.GetValue();

    public void SetValue(T value) 
    {
        if (useOverride)
        {
            overrideValue = value;
        }
        else
        {
            variableValue.SetValue(value);
        }
    }
    
    public static implicit operator T(GenericReference<T> refrence)
    {
        return refrence.GetValue();
    }
}
