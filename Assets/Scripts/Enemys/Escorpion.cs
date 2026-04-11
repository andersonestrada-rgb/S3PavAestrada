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
    private void Awake()
    {
        stats = new BaseStats(12, 10, 3, 1, 20);
    }

    private void Start()
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
                multiplier = 1.4f; // algo más de daño
                break;
            case Elements.Water:
                multiplier = 0.5f; // reducido
                break;
            case Elements.Earth:
                multiplier = 6f; // muy vulnerable
                break;
            case Elements.Air:
                multiplier = 0.5f; // reducido
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


    // Recibir daño desde otras fuentes (por ejemplo Player.Attack)
    //public void TakeDamage(int damage)
    //{
    //    stats.TakeDamage(damage);
    //    if (stats.Health <= 0)
    //    {
    //        // Incrementar puntaje global cuando este enemigo muere
    //        if (Score.Instance != null)
    //        {
    //            Score.Instance.AddScore(stats.XP);
    //        }

    //        Destroy(gameObject);
    //    }
    //}



    

}
