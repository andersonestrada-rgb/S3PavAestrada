using UnityEngine;

public class EricLecarde : BaseEntity
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
            Elements.Water => 0.5f, 
            Elements.Earth => 6f,   
            Elements.Air => 2f,   
            _ => 1f                 
        };
    }
}
