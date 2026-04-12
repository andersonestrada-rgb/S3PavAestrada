/*
1️ Modelado base del sistema:
Crear una clase abstracta Entity 
que sirva como base para Player y 
los 4 tipos de enemigos.
Cada entidad debe contener una 
instancia de la clase pura BaseStats, 
inicializada mediante constructor 
parametrizado.
La clase Entity debe definir la 
estructura común del sistema y 
obligar a que cada clase hija implemente 
su propio método TakeDamage().
*/

using UnityEngine;

public enum Elements
{
    None,//0
    Fire,//1
    Water,//2
    Earth,//3
    Air //4
}

public class BaseEntity : MonoBehaviour
{
    [Header("Configuración de la Entidad")]
    [SerializeField] protected int entityID;
    [SerializeField] protected string entityName;
    [SerializeField] protected string enetityDescription;
    [SerializeField] protected Elements element;
    [SerializeField] protected BaseStats stats;

    private void OnDestroy()
    {
        Debug.Log($"{entityName} ha sido destruido");
    }   

    // Añade este método para que los hijos lo sobreescriban
    protected virtual float GetElementalMultiplier(Elements damageElement)
    {
        return 1f; // Por defecto el daño es normal
    }

    // Modifica el TakeDamage base para que haga todo el trabajo
    public virtual void TakeDamage(int damageAmount, Elements damageElement)
    {
        // 1. Obtiene el multiplicador del hijo correspondiente
        float multiplier = GetElementalMultiplier(damageElement);

        // 2. Calcula el daño final
        int finalDamage = Mathf.RoundToInt(damageAmount * multiplier);
        if (damageAmount > 0 && finalDamage < 1) finalDamage = 1;

        Debug.Log($"{entityName} ha sufrido {finalDamage} punto(s) de daño ({damageElement})");

        // 3. Aplica el daño a los stats
        stats.TakeDamage(finalDamage);

        // 4. Lógica de muerte
        if (stats.Health <= 0)
        {
            print($"{entityName} ha sido derrotado");
            if (Score.Instance != null) Score.Instance.AddScore(stats.XP);
            if (Spawner.Instance != null && !(this is Player)) Spawner.Instance.DropXPOrb(transform.position);
            Destroy(gameObject);
        }
    }

    // Método para restaurar vida (usado por pociones y otros efectos curativos)
    public virtual void HealHealth(int healAmount)
    {
        if (healAmount <= 0) return;

        int oldHealth = stats.Health;
        stats.HealHealth(healAmount);
        int newHealth = stats.Health;

        Debug.Log($"{entityName} fue curado: {oldHealth} -> {newHealth}");
    }

    public BaseStats Stats => stats;
    public Elements Element => element;
    public int Health => stats.Health;
    public int Power => stats.Power;
    public int Speed => stats.Speed;
    public int Knockback => stats.Knockback;
    public int XP => stats.XP; 
}
