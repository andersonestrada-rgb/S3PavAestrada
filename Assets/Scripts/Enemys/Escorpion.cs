/*
4️ Destrucción y flujo final
Cuando la vida de un Enemy llegue 
a 0 debe ejecutarse Destroy(gameObject). 
Implementar OnDestroy para evidenciar 
la destrucción. No usar destructores 
(~Clase) para lógica de gameplay.
*/

using UnityEngine;
public class Escorpion : BaseEntity
{
    [SerializeField] private float collisionAttackCooldown = 1f;
    private float lastCollisionAttackTime = -Mathf.Infinity;

    private void Start()
    {
        print($"{entityName} es de elemento {element}");
    }
   
    protected override float GetElementalMultiplier(Elements damageElement)
    {
        return damageElement switch
        {
            Elements.Fire => 1.4f,  // algo más de daño
            Elements.Water => 0.5f, // reducido
            Elements.Earth => 6f,   // muy vulnerable
            Elements.Air => 0.5f,   // reducido
            _ => 1f                 // Default (None y otros)
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
