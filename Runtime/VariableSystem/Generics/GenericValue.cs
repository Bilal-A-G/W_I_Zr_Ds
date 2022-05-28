using UnityEngine;

public abstract class GenericValue<T> : ScriptableObject
{
    public abstract T GetValue(CachedObjectWrapper cachedObjects);

    public abstract void SetValue(T value, CachedObjectWrapper cachedObjects);
}
