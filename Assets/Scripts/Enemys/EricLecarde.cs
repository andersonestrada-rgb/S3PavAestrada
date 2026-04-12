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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto con el que chocamos tiene el script Player
        if (collision.gameObject.TryGetComponent(out Colector colector))
        {
            // Le aplicamos daño al Player usando el Poder y Elemento de este enemigo
            colector.player.TakeDamage(stats.Power, element);
            //player.TakeDamage(this.Power, this.Element);
        }
    }
}
