using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "FSM/State Tree Object")]
public class StateTreeObject : ScriptableObject
{
    [SerializeField]
    StateObject initialState;

    [System.NonSerialized]
    public StateObject currentState;

    [System.NonSerialized]
    Dictionary<EventObject, GameObject> queuedActions;

    public void UpdateState(EventObject action, GameObject callingObject)
    {
        if (currentState == null) currentState = initialState;
        if (queuedActions == null) queuedActions = new Dictionary<EventObject, GameObject>();

        if (TryTransitionState(action, callingObject) || TryInvokeActionOnState(action, callingObject))
        {
            foreach(KeyValuePair<EventObject, GameObject> kvp in queuedActions)
            {
                if(TryTransitionState(kvp.Key, kvp.Value) || TryInvokeActionOnState(kvp.Key, kvp.Value))
                {
                    queuedActions.Remove(action);
                }
            }
        }
        else
        {
            if (action.queueable && !queuedActions.ContainsKey(action))
            {
                queuedActions.Add(action, callingObject);
            }
        }

        UpdateChildFSM(action, callingObject);
    }

    void UpdateChildFSM(EventObject action, GameObject callingObject)
    {
        if (currentState.stateChild == null) return;

        currentState.stateChild.UpdateState(action, callingObject);
    }

    bool TryTransitionState(EventObject action, GameObject callingObject)
    {
        bool success = false;
        StateObject transitionTo = null;

        for (int i = 0; i < currentState.stateTransitions.Count; i++)
        {
            if (currentState.stateTransitions[i].action == action)
            {
                transitionTo = currentState.stateTransitions[i].stateObject;
                success = true;
                break;
            }
        }

        if (success)
        {
            for(int i = 0; i < currentState.onStateExit.Count; i++)
            {
                currentState.onStateExit[i].Invoke(callingObject);
            }

            for(int i = 0; i < transitionTo.onStateEnter.Count; i++)
            {
                transitionTo.onStateEnter[i].Invoke(callingObject);
            }

            currentState = transitionTo;

            return true;
        }

        return false;
    }

    bool TryInvokeActionOnState(EventObject action, GameObject callingObject)
    {
        for (int i = 0; i < currentState.stateActions.Count; i++)
        {
            if (action == currentState.stateActions[i].action)
            {
                currentState.stateActions[i].GetTranslatedEvent().Invoke(callingObject);
                return true;
            }
        }

        return false;
    }
}
