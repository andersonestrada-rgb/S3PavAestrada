/*
4️ Destrucción y flujo final
Cuando la vida de un Enemy llegue 
a 0 debe ejecutarse Destroy(gameObject). 
Implementar OnDestroy para evidenciar 
la destrucción. No usar destructores 
(~Clase) para lógica de gameplay.
*/

using UnityEngine;

public class Enemy : MonoBehaviour
{    
    private BaseStats stats;

    private void Awake()
    {
        stats = new BaseStats(9, 10, 3, 1, 20);
    }

    // Recibir daño desde otras fuentes (por ejemplo Player.Attack)
    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        if (stats.Health <= 0)
        {
            // Incrementar puntaje global cuando este enemigo muere
            if (Score.Instance != null)
            {
                Score.Instance.AddScore(stats.XP);
            }

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
