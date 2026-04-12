using UnityEngine;

public class Mutante : BaseEntity
{
    [SerializeField] private float collisionAttackCooldown = 1f;
    private float lastCollisionAttackTime = -Mathf.Infinity;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Evitamos aplicar daño si el cooldown no ha expirado
        if (Time.time - lastCollisionAttackTime < collisionAttackCooldown)
            return;

        // Verificamos si el objeto con el que chocamos tiene el script Colector
        if (collision.gameObject.TryGetComponent(out Colector colector))
        {
            // Le aplicamos daño al Player usando el Poder y Elemento de este enemigo
            colector.player.TakeDamage(stats.Power, element);
            lastCollisionAttackTime = Time.time;
        }
    }    
}
