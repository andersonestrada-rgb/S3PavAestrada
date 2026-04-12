using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))] // Asegura que el GameObject tenga un CircleCollider2D
public class Colector : MonoBehaviour
{
    public Player player;
    public CircleCollider2D coll;

    private Vector3 originalLocalPosition; // Para recordar dónde va normalmente
    private Vector3 frozenWorldPosition;   // Para recordar dónde congelarlo

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

    private void Start()
    {
        // Guardamos su distancia (offset) original respecto al Player
        originalLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        // 1. En el instante exacto en que presionas Controll, guardamos su posición actual en el mundo
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            frozenWorldPosition = transform.position;
        }

        // 2. Mientras mantengas Controll presionado, lo forzamos a quedarse en esa coordenada      
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = frozenWorldPosition;
        }
        //// 3. Para que regre con el player
        //else if (Input.GetKeyUp(KeyCode.LeftControl))
        //{
        //    transform.localPosition = originalLocalPosition;
        //}
    }

    private void OnDrawGizmosSelected()
    {       
        if (coll != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, coll.radius);
        }
        else
        {
            // Intenta leer el radio temporalmente si aún no estamos en Play
            var tempColl = GetComponent<CircleCollider2D>();
            if (tempColl != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, tempColl.radius);
            }
        }
    }
}