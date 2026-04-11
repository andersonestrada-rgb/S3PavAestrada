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
    Earth,
    Air
}

public class BaseEntity : MonoBehaviour
{
    [SerializeField] protected int entityID;
    [SerializeField] protected string entityName;
    [SerializeField] protected string enetityDescription;

    [SerializeField] protected Elements element;

    [SerializeField] protected BaseStats stats;

    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        Debug.Log($"{entityName} ha sido destruido");
    }

    public BaseStats Stats => stats;
    public Elements Element => element;

    // Recibir daño: la vida de la entidad sólo se modifica a través de este método

    // Ahora se recibe un valor de daño ya calculado y el elemento del ataque.
    public virtual void TakeDamage(int damageAmount, Elements damageElement)
    {
        //Debug.Log(damageElement);
        stats.TakeDamage(damageAmount);

        if (stats.Health <= 0)
        {
            print($"{entityName} ha sido derrotado"); 
            if (Score.Instance != null)
            {
                Score.Instance.AddScore(stats.XP);
            }

            // Los enemigos derrotados dejan caer una esfera de XP (delegado al Spawner)
            if (Spawner.Instance != null && !(this is Player))
            {
                Spawner.Instance.DropXPOrb(transform.position);
            }

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

    public int Health => stats.Health;
    public int Power => stats.Power;
    public int Speed => stats.Speed;
    public int Knockback => stats.Knockback;
    public int XP => stats.XP;   



}
