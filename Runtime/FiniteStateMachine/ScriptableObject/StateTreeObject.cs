using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "FSM/State Tree Object")]
public class StateTreeObject : ScriptableObject
{
    [SerializeField]
    StateObject initialState;

    StateObject currentState;

    List<EventObjectPairs> queuedActions;


    private void OnEnable()
    {
        currentState = initialState;
        queuedActions = new List<EventObjectPairs>();
    }

    public void UpdateState(EventObject action, GameObject callingObject)
    {
        bool transitioned = TryTransitionState(action, callingObject);
        bool invoked = TryInvokeActionOnState(action, callingObject);

        if (queuedActions.Contains(new EventObjectPairs(callingObject, action))) queuedActions.Remove(new EventObjectPairs(callingObject, action));

        if (transitioned || invoked)
        {
            for(int i = 0; i < queuedActions.Count; i++)
            {
                bool transitionedFromQueue = TryTransitionState(queuedActions[i].eventObject, queuedActions[i].gameObject);
                bool invokedFromQueue = TryInvokeActionOnState(queuedActions[i].eventObject, queuedActions[i].gameObject);

                if(transitionedFromQueue || invokedFromQueue) queuedActions.RemoveAt(i);
            }
        }
        else
        {
            if (action.queueable && !queuedActions.Contains(new EventObjectPairs(callingObject, action)))
            {
                queuedActions.Add(new EventObjectPairs(callingObject, action));
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
