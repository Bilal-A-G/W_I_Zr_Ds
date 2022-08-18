using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to get and set values on a monobehaviour anchor for independent state
public interface IRuntimeVariable
{
    public object GetValueFromName(string name);

    public void SetValueFromName(string name, object value);
}
