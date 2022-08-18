using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "FSM/State Object")]
public class StateObject : ScriptableObject
{
    public List<EventObject> onStateEnter;
    public List<EventObject> onStateExit;

    public List<StateActionTranslationPairs> stateActions;
    public List<EventStatePairs> stateTransitions;

    public StateLayerObject childLayer;
}

[System.Serializable]
public struct EventStatePairs
{
    public EventObject action;
    public StateObject stateObject;
}

[System.Serializable]
public struct StateActionTranslationPairs
{
    [SerializeField]
    bool translate;

    [SerializeField]
    bool isFolded;

    [SerializeField]
    EventObject translateTo;

    public EventObject action;

    public EventObject GetTranslatedEvent() => translate ? translateTo : action;
}
