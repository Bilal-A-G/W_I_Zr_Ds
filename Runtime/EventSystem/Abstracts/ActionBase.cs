using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class that all actions must implement, actions are essentially drag-and-drop friendly first class functions/methods
public abstract class ActionBase : ScriptableObject
{
    //Execute method, called by the event listener when the actions corresponding event is invoked
    public abstract void Execute(CachedObjectWrapper cachedObjects);

    //Update loop, runs every frame, called from the event listeners update method
    public virtual void UpdateLoop(CachedObjectWrapper cachedObjects)
    {
        return;
    }

    //Fixed update loop, called from the event listeners fixed update method
    public virtual void FixedUpdateLoop(CachedObjectWrapper cachedObjects)
    {
        return;
    }
}
