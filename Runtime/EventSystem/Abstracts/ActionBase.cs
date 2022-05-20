using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : ScriptableObject
{
    public abstract void Execute(GameObject callingObject);

    public virtual void UpdateLoop(GameObject callingObject)
    {
        return;
    }

    public virtual void FixedUpdateLoop(GameObject callingObject)
    {
        return;
    }
}
