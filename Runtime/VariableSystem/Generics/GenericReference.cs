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

    public T GetValue(CachedObjectWrapper cachedObjects) => useOverride ? overrideValue : variableValue.GetValue(cachedObjects);

    public void SetValue(T value, CachedObjectWrapper cachedObjects) 
    {
        if (useOverride)
        {
            overrideValue = value;
        }
        else
        {
            variableValue.SetValue(value, cachedObjects);
        }
    }
}
