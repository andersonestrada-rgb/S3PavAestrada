using UnityEngine;

public class MetalAlien : BaseEntity
{

    private void Awake()
    {
        stats = new BaseStats(17, 18, 1, 1, 26);
    }

    void Start()
    {
        print($"{entityName} es de elemento {element}");
    }

    void Update()
    {
        
    }

    public override void TakeDamage(int damageAmount, Elements damageElement)
    {        
        float multiplier = 1f;
        switch (damageElement)
        {
            case Elements.None:
                multiplier = 1f;
                break;
            case Elements.Fire:
                multiplier = 0.5f; // algo resistente
                break;
            case Elements.Water:
                multiplier = 5f; // muy vulnerable
                break;
            case Elements.Earth:
                multiplier = 2f; // vulnerable
                break;
            case Elements.Air:
                multiplier = 0.5f; // algo resistente
                break;
            default:
                multiplier = 1f;
                break;
        }

        int damager = Mathf.RoundToInt(damageAmount * multiplier);
        if (damageAmount > 0 && damager < 1) damager = 1;

        Debug.Log($"{entityName} ha sufrido {damager} punto(s) de daño ({damageElement})");
        base.TakeDamage(damager, damageElement);
    }


}
