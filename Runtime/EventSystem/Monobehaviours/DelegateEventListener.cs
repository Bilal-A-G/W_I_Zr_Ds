using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Standard event listener implementation, when an event is invoked,
//it executes all actions corresponding to said event
public class DelegateEventListener : EventListenerBase
{
    public List<EventActions> eventActions;
    public CachedObjectWrapper cachedObjects;

    //Subscribes to all events in eventActions
    protected override void SubscribeToEvents()
    {
        for (int i = 0; i < eventActions.Count; i++)
        {
            for (int v = 0; v < eventActions[i].events.Count; v++)
            {
                eventActions[i].events[v].Subscribe(this);
            }
        }
    }

    //Unsubscribes from all events in eventActions
    protected override void UnSubscribeFromEvents()
    {
        for (int i = 0; i < eventActions.Count; i++)
        {
            for (int v = 0; v < eventActions[i].events.Count; v++)
            {
                eventActions[i].events[v].UnSubscribe(this);
            }
        }
    }

    //Executes all actions corresponding to an event when invoked
    public override void OnInvoke(EventObject callingEvent, GameObject callingObject)
    {
        if (callingObject != cachedObjects.GetGameObjectFromCache("Parent") && !callingEvent.global && callingObject != null) return;

        for(int i = 0; i < eventActions.Count; i++)
        {
            for(int v = 0; v < eventActions[i].events.Count; v++)
            {
                if(eventActions[i].events[v] == callingEvent)
                {
                    for(int z = 0; z < eventActions[i].actions.Count; z++)
                    {
                        eventActions[i].actions[z].Execute(cachedObjects);
                    }
                }
            }
        }
    }

    //Delivers update loop to events
    private void Update()
    {
        for(int i = 0; i < eventActions.Count; i++)
        {
            for(int v = 0; v < eventActions[i].actions.Count; v++)
            {
                eventActions[i].actions[v].UpdateLoop(cachedObjects);
            }
        }
    }

    //Delivers fixed update loop to events
    private void FixedUpdate()
    {
        for (int i = 0; i < eventActions.Count; i++)
        {
            for (int v = 0; v < eventActions[i].actions.Count; v++)
            {
                eventActions[i].actions[v].FixedUpdateLoop(cachedObjects);
            }
        }
    }
}

//Struct for event action pairs, basically just an inspector visible dictionary with a fancy
//property drawer
[System.Serializable]
public struct EventActions
{
    public List<EventObject> events;
    public List<ActionBase> actions;
}
