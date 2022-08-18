using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event listener base class that every event listener must implement,
//listeners listen for when specific events are invoked and then execute the actions corresponding to the event
public abstract class EventListenerBase : MonoBehaviour
{
    protected void OnEnable() => SubscribeToEvents();

    protected void OnDisable() => UnSubscribeFromEvents();

    protected abstract void SubscribeToEvents();

    protected abstract void UnSubscribeFromEvents();

    //Called from subscribed events
    public abstract void OnInvoke(EventObject callingEvent, GameObject callingObject);
}
