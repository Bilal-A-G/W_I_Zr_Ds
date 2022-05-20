using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event Objects")]
public class EventObject : ScriptableObject
{
    public bool queueable;

    [System.NonSerialized]
    List<EventListenerBase> subscribedListeners = new List<EventListenerBase>();

    public void Subscribe(EventListenerBase listener)
    {
        if (!subscribedListeners.Contains(listener))
        {
            subscribedListeners.Add(listener);
        }
    }

    public void UnSubscribe(EventListenerBase listener)
    {
        if (subscribedListeners.Contains(listener))
        {
            subscribedListeners.Remove(listener);
        }
    }

    public void Invoke(GameObject callingObject)
    {
        for(int i = 0; i < subscribedListeners.Count; i++)
        {
            subscribedListeners[i].OnInvoke(this, callingObject);
        }
    }

    public void InvokeInEditor() => Invoke(null);
}
