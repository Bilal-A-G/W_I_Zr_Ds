using UnityEngine;

[CreateAssetMenu(fileName = "New Enum", menuName = "Variables/Enum Variable")]
public class EnumVariable : GenericVariable<EnumVariable>
{
    new public EnumVariable GetValue => this;

}
