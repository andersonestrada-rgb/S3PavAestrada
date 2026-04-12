using UnityEngine;

public class MetalAlien : BaseEntity
{
    void Start()
    {
        print($"{entityName} es de elemento {element}");
    }

    protected override float GetElementalMultiplier(Elements damageElement)
    {
        return damageElement switch
        {
            Elements.Fire => 0.5f, 
            Elements.Water => 5f,
            Elements.Earth => 2f,   
            Elements.Air => 0.5f,   
            _ => 1f                 
        };
    }
}
