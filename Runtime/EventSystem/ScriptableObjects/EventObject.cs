using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be invoked by a fsm or directly. Once invoked, calls the onInvoke function on
//all subscribed event listeners
[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event Object")]
public class EventObject : ScriptableObject
{
    public bool global;
    public bool queueable;

    public List<StateLayerObject> dequeueLayer;

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
}
