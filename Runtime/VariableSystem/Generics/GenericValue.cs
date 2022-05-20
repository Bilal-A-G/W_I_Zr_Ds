using UnityEngine;

public abstract class GenericValue<T> : ScriptableObject
{
    public abstract T GetValue();

    public abstract void SetValue(T value);
}
