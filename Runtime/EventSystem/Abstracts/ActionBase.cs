using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : ScriptableObject
{
    public abstract void Execute(CachedObjectWrapper cachedObjects);

    public virtual void UpdateLoop(CachedObjectWrapper cachedObjects)
    {
        return;
    }

    public virtual void FixedUpdateLoop(CachedObjectWrapper cachedObjects)
    {
        return;
    }
}
