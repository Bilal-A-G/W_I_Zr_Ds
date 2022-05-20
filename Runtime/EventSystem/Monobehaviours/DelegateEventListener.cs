using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelegateEventListener : EventListenerBase
{
    public List<EventActions> eventActions;
    public GameObject parentObject;

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

    public override void OnInvoke(EventObject callingEvent, GameObject callingObject)
    {
        if (callingObject != parentObject && callingObject != null) return;

        for(int i = 0; i < eventActions.Count; i++)
        {
            for(int v = 0; v < eventActions[i].events.Count; v++)
            {
                if(eventActions[i].events[v] == callingEvent)
                {
                    for(int z = 0; z < eventActions[i].actions.Count; z++)
                    {
                        eventActions[i].actions[z].Execute(callingObject);
                    }
                }
            }
        }
    }

    private void Update()
    {
        for(int i = 0; i < eventActions.Count; i++)
        {
            for(int v = 0; v < eventActions[i].actions.Count; v++)
            {
                eventActions[i].actions[v].UpdateLoop(parentObject);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < eventActions.Count; i++)
        {
            for (int v = 0; v < eventActions[i].actions.Count; v++)
            {
                eventActions[i].actions[v].FixedUpdateLoop(parentObject);
            }
        }
    }
}

[System.Serializable]
public struct EventActions
{
    public List<EventObject> events;
    public List<ActionBase> actions;
}
