using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Monobehaviour wrapper around a state layer object.
public class FiniteStateMachine : MonoBehaviour
{
    public StateLayerObject initialState;

    public void UpdateState(EventObject action, GameObject callingObject, CachedObjectWrapper cachedObjects) => initialState.UpdateState(action, callingObject, cachedObjects);
}

