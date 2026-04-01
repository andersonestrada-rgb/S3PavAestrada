/*
4️ Destrucción y flujo final
Cuando la vida de un Enemy llegue 
a 0 debe ejecutarse Destroy(gameObject). 
Implementar OnDestroy para evidenciar 
la destrucción. No usar destructores 
(~Clase) para lógica de gameplay.
*/

using System.Timers;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    private BaseStats stats;

    private void Awake()
    {
        stats = new BaseStats(9, 10, 3, 1, 20);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(Player player)
    {
        stats.TakeDamage(player.Damage);
        if (stats.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int Health => stats.Health;
    public int Power => stats.Power;
    public int Speed => stats.Speed;
    public int Knockback => stats.Knockback;
    public int XP => stats.XP;


    private void OnDestroy()
    {
        Debug.Log("El enemigo ha muerto");
    }
}
