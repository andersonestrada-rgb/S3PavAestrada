using UnityEngine;

public class Mutante : BaseEntity
{
    private void Awake()
    {
        stats = new BaseStats(9, 25, 3, 1, 30);
    }

    // Start left empty to avoid overwriting stats set in Awake
    void Start()
    {
        print($"{entityName} es de elemento {element}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void TakeDamage(int damageAmount, Elements damageElement)
    {      
        // Aplicar multiplicadores por elemento (usar float y asegurar al menos 1 de daño si el ataque causó >0)
        float multiplier = 1f;
        switch (damageElement)
        {
            case Elements.None:
                multiplier = 1f;
                break;
            case Elements.Fire:
                multiplier = 0.25f; // resistente pero recibe daño
                break;
            case Elements.Water:
                multiplier = 0.25f;
                break;
            case Elements.Earth:
                multiplier = 0.25f;
                break;
            case Elements.Air:
                multiplier = 0.5f;
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
