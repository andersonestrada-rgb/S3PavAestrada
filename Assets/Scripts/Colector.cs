using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))] // Asegura que el GameObject tenga un CircleCollider2D
public class Colector : MonoBehaviour
{    
    public Player player;
    public CircleCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        coll.isTrigger = true; // Asegurarnos de que sea trigger

        // Intentar obtener la referencia al Player
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
    }

    private void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, coll.radius);
    }    
}