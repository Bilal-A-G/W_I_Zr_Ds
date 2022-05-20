using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericVariable<T> : GenericValue<T>
{
    public T value;

    public override T GetValue() => value;

    public override void SetValue(T value) => this.value = value;
}
