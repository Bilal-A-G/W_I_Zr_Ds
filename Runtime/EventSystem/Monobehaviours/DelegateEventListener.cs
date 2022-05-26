using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelegateEventListener : EventListenerBase
{
    public List<EventActions> eventActions;
    public CachedObjectWrapper cachedObjects;

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
        if (callingObject != cachedObjects.GetGameObjectFromCache("Parent") && callingObject != null) return;

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

[System.Serializable]
public struct EventActions
{
    public List<EventObject> events;
    public List<ActionBase> actions;
}

[System.Serializable]
public struct CachedObjects
{
    public GameObject cachedObject;
    public string key;

    public GameObject GetValueFromKey(string key)
    {
        if (key == this.key)
        {
            return cachedObject;
        }

        return null;
    }
}

[System.Serializable]
public struct CachedObjectWrapper
{
    public List<CachedObjects> cache;

    public GameObject GetGameObjectFromCache(string key)
    {
        GameObject currentObject;

        for(int i = 0; i < cache.Count; i++)
        {
            currentObject = cache[i].GetValueFromKey(key);
            if (currentObject != null) return currentObject;
        }

        Debug.LogError("Key " + key + " not found in cache, did you forget to assign it?");
        return null;
    }
}
