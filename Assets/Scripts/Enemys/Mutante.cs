using UnityEngine;

public class Mutante : BaseEntity
{
    void Start()
    {
        print($"{entityName} es de elemento {element}");
    }

    protected override float GetElementalMultiplier(Elements damageElement)
    {
        return damageElement switch
        {
            Elements.Fire => 0.25f,  
            Elements.Water => 1.5f, 
            Elements.Earth => 6f,   
            Elements.Air => 2f,   
            _ => 1f                
        };
    }  
}
