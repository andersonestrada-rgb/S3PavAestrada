using UnityEngine;

public enum Estados
{
    None,   // Sin estado       0
    Burn,   // Quemado          1
    Freeze, // Congelado        2
    Poison, // Envenenado       3
    Shock,  // Electrocución    4
    Slow,   // Ralentizado      5
}

public abstract class BaseAbility : MonoBehaviour
{   
    public abstract void Execute(BaseEntity target);
}
