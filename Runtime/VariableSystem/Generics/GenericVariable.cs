using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to create variables
public abstract class GenericVariable<T> : GenericValue<T>
{
    public T value;

    public override T GetValue(CachedObjectWrapper cachedObjects) => value;

    public override void SetValue(T value, CachedObjectWrapper cachedObjects) => this.value = value;
}
