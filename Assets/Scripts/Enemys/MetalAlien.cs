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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto con el que chocamos tiene el script Player
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            // Le aplicamos daño al Player usando el Poder y Elemento de este enemigo
            player.TakeDamage(this.Power, this.Element);
        }
    }

}
