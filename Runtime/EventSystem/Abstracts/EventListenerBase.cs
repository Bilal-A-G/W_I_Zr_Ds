using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventListenerBase : MonoBehaviour
{
    protected void OnEnable() => SubscribeToEvents();

    protected void OnDisable() => UnSubscribeFromEvents();


    protected abstract void SubscribeToEvents();

    protected abstract void UnSubscribeFromEvents();

    public abstract void OnInvoke(EventObject callingEvent, GameObject callingObject);
}
