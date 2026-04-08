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
        print($"{entityName} es de elemento {element}");        
    }

    public BaseStats Stats => stats;
    public Elements Element => element;

    // Recibir daño: la vida de la entidad sólo se modifica a través de este método

    public virtual void TakeDamage(BaseEntity damage)
    {        
        stats.TakeDamage(damage.stats.Power);

        if (stats.Health <= 0)
        {
            print($"{entityName} ha sido derrotado");
            if (Score.Instance != null)
            {
                Score.Instance.AddScore(stats.XP);
            }
            Destroy(gameObject);
        }
    }

    //public virtual void TakeDamage(int damage)
    //{
    //    stats.TakeDamage(damage);
    //    if (stats.Health <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}


}
