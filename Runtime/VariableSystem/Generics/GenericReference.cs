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
    GenericVariable<T> variableValue;
    [SerializeField]
    T overrideValue;

    public T GetValue() => useOverride ? overrideValue : variableValue.GetValue;

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
}
