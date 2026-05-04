using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class EnemyFollow : MonoBehaviour
{
    private BaseEntity myEntity; // Referencia a sus propias estadísticas
    private Colector colector;       // Referencia al objetivo

    [Header("Efectos Visuales")]
    private SpriteRenderer spriteRenderer; // Referencia para voltear el sprite

    private void Awake()
    {
        myEntity = GetComponent<BaseEntity>();

        // Usamos GetComponentInChildren por si el sprite está en un objeto hijo
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        colector = FindAnyObjectByType<Colector>();
    }

    void Update()
    {
        if (colector == null) return;

        // 1. Calcula la dirección hacia el jugador
        Vector2 direction = ((Vector2)colector.transform.position - (Vector2)transform.position).normalized;

        // 2. Mueve al enemigo (AQUÍ ES EL CAMBIO CLAVE)
        // Usamos myEntity.ActualSpeed para que respete el multiplicador de los estados
        transform.position += (Vector3)(direction * myEntity.ActualSpeed * Time.deltaTime);

        // 3. Voltea el sprite (Para sprites que miran a la izquierda originalmente)
        if (spriteRenderer != null)
        {
            if (direction.x < 0f && spriteRenderer.flipX)
            {
                // Si va a la izquierda y ESTÁ volteado, regrésalo a la normalidad (mirar a la izq)
                spriteRenderer.flipX = false;
            }
            else if (direction.x > 0f && !spriteRenderer.flipX)
            {
                // Si va a la derecha y NO está volteado, voltéalo (mirar a la der)
                spriteRenderer.flipX = true;
            }
        }
    }
}