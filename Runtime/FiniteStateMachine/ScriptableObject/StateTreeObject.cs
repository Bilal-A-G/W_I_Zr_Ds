using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "FSM/State Tree Object")]
public class StateTreeObject : ScriptableObject
{
    [SerializeField]
    StateObject initialState;

    StateObject currentState;

    List<KeyValuePair<EventObject, GameObject>> queuedActions;


    private void OnEnable()
    {
        currentState = initialState;
        queuedActions = new List<KeyValuePair<EventObject, GameObject>>();
    }

    public void UpdateState(EventObject action, GameObject callingObject)
    {
        bool transitioned = TryTransitionState(action, callingObject);
        bool invoked = TryInvokeActionOnState(action, callingObject);

        if (queuedActions.Contains(new KeyValuePair<EventObject, GameObject>(action, callingObject))) queuedActions.Remove(new KeyValuePair<EventObject, GameObject>(action, callingObject));

        if (transitioned || invoked)
        {
            foreach (KeyValuePair<EventObject, GameObject> kvp in queuedActions)
            {
                bool transitionedFromQueue = TryTransitionState(kvp.Key, kvp.Value);
                bool invokedFromQueue = TryInvokeActionOnState(kvp.Key, kvp.Value);

                if(transitionedFromQueue || invokedFromQueue) queuedActions.Remove(new KeyValuePair<EventObject, GameObject>(action, callingObject));
            }
        }
        else
        {
            if (action.queueable && !queuedActions.Contains(new KeyValuePair<EventObject, GameObject>(action, callingObject)))
            {
                queuedActions.Add(new KeyValuePair<EventObject, GameObject>(action, callingObject));
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
