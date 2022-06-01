using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "FSM/State Tree Object")]
public class StateTreeObject : ScriptableObject
{
    [SerializeField]
    GenericReference<StateObject> currentState;

    StateObject currentStateObject;

    List<EventObjectPairs> queuedActions;

    CachedObjectWrapper cachedObjects;


    private void OnEnable()
    {
        queuedActions = new List<EventObjectPairs>();
        currentStateObject = currentState.GetValue(cachedObjects);
    }

    public void UpdateState(EventObject action, GameObject callingObject)
    {
        bool transitioned = TryTransitionState(action, callingObject);
        bool invoked = TryInvokeActionOnState(action, callingObject);

        if (transitioned)
        {
            for(int i = 0; i < queuedActions.Count; i++)
            {
                TryTransitionState(queuedActions[i].eventObject, queuedActions[i].gameObject);
                TryInvokeActionOnState(queuedActions[i].eventObject, queuedActions[i].gameObject);

                UpdateChildFSM(queuedActions[i].eventObject, queuedActions[i].gameObject);

                queuedActions.RemoveAt(i);
            }
        }
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

        UpdateChildFSM(action, callingObject);
    }

    void UpdateChildFSM(EventObject action, GameObject callingObject)
    {
        if (currentStateObject.stateChild == null) return;

        currentStateObject.stateChild.UpdateState(action, callingObject);
    }

    bool TryTransitionState(EventObject action, GameObject callingObject)
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

    bool TryInvokeActionOnState(EventObject action, GameObject callingObject)
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
