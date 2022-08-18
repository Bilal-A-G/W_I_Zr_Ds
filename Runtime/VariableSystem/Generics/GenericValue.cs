using UnityEngine;

//Used to implement variable functions, but also is the base class for all variables
public abstract class GenericValue<T> : ScriptableObject
{
    public abstract T GetValue(CachedObjectWrapper cachedObjects);

    public abstract void SetValue(T value, CachedObjectWrapper cachedObjects);
}
