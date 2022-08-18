using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Holds a state object, transitions, and updates it. A layer in the hierarchical state machine system 
[CreateAssetMenu(fileName = "New Tree", menuName = "FSM/State Layer Object")]
public class StateLayerObject : ScriptableObject
{
    [SerializeField]
    GenericReference<StateObject> currentState;

    StateObject currentStateObject;

    List<EventObjectPairs> queuedActions;

    //Clears/initializes queuedActions on enable
    private void OnEnable()
    {
        queuedActions = new List<EventObjectPairs>();
    }

    //Handles transitioning current state based on the event the function is called with,
    //and propogates events as well if the currrent state allows it
    public void UpdateState(EventObject action, GameObject callingObject, CachedObjectWrapper cachedObjects)
    {
        //Tries to transition and then propogate the event
        bool transitioned = TryTransitionState(action, callingObject, cachedObjects);
        bool invoked = TryPropogateActionOnState(action, callingObject, cachedObjects);

        //If current state was transitioned, then tries to transition, propogate,
        //and update the child of everything in queued actions
        if (transitioned)
        {
            for(int i = 0; i < queuedActions.Count; i++)
            {
                TryTransitionState(queuedActions[i].eventObject, queuedActions[i].gameObject, cachedObjects);
                TryPropogateActionOnState(queuedActions[i].eventObject, queuedActions[i].gameObject, cachedObjects);

                UpdateChildFSM(queuedActions[i].eventObject, queuedActions[i].gameObject, cachedObjects);

                queuedActions.RemoveAt(i);
            }
        }
        //If the current state didn't transition or propogate the event,
        //then add it to the queued actions list according to its configurations
        else if(!transitioned && !invoked)
        {
            if (action.queueable && !queuedActions.Contains(new EventObjectPairs(callingObject, action)))
            {
                for(int i = 0; i < action.dequeueLayer.Count; i++)
                {
                    if(action.dequeueLayer[i] == this)
                    {
                        queuedActions.Add(new EventObjectPairs(callingObject, action));
                        break;
                    }
                }
            }
        }

        //Updates the child of current state
        UpdateChildFSM(action, callingObject, cachedObjects);
    }

    void UpdateChildFSM(EventObject action, GameObject callingObject, CachedObjectWrapper cachedObjects)
    {
        if (currentStateObject.childLayer == null) return;

        currentStateObject.childLayer.UpdateState(action, callingObject, cachedObjects);
    }

    bool TryTransitionState(EventObject action, GameObject callingObject, CachedObjectWrapper cachedObjects)
    {
        currentStateObject = currentState.GetValue(cachedObjects);

        bool success = false;
        StateObject transitionTo = null;

        for (int i = 0; i < currentStateObject.stateTransitions.Count; i++)
        {
            if (currentStateObject.stateTransitions[i].action == action)
            {
                transitionTo = currentStateObject.stateTransitions[i].stateObject;
                success = true;
                break;
            }
        }

        if (success)
        {
            for(int i = 0; i < currentStateObject.onStateExit.Count; i++)
            {
                currentStateObject.onStateExit[i].Invoke(callingObject);
            }

            for(int i = 0; i < transitionTo.onStateEnter.Count; i++)
            {
                transitionTo.onStateEnter[i].Invoke(callingObject);
            }

            currentState.SetValue(transitionTo, cachedObjects);

            return true;
        }

        return false;
    }

    bool TryPropogateActionOnState(EventObject action, GameObject callingObject, CachedObjectWrapper cachedObjects)
    {
        currentStateObject = currentState.GetValue(cachedObjects);

        for (int i = 0; i < currentStateObject.stateActions.Count; i++)
        {
            if (action == currentStateObject.stateActions[i].action)
            {
                currentStateObject.stateActions[i].GetTranslatedEvent().Invoke(callingObject);
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public struct EventObjectPairs
{
    public GameObject gameObject;
    public EventObject eventObject;

    public EventObjectPairs(GameObject gameObject, EventObject eventObject)
    {
        this.gameObject = gameObject;
        this.eventObject = eventObject;
    }
}
