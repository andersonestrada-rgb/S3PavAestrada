using UnityEngine;
using UnityEngine.Events;


public class Barril : BaseObject
{
    [Header("Configuración de Explosión")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 15;
    [Space]
    [Header("Efectos de Explosión")]
    public UnityEvent Boom;


    public override void TakeDamage(int amount)
    {
        objectHealth -= amount;

        if (objectHealth <= 0)
        {
            OnDestroyed(); // Aquí se dispara el UnityEvent (Boom) con tus partículas

            // Desactivamos lo que no queremos ver ni tocar
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;

            // Esperamos 2 segundos antes de borrar el objeto real
            // permitiendo que el UnityEvent termine de mostrar las partículas
            Destroy(gameObject, 2.0f);
        }
        else
        {
            FlashColor();
        }
    }

    protected override void OnDestroyed()
    {
        Debug.Log($"{objectName} ha explotado!");

        // Invocar el evento de explosión
        Boom?.Invoke();

        // Aplicar daño en área a todos los objetos cercanos
        CauseExplosionDamage();
    }
 
    private void CauseExplosionDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in hitColliders)
        {
            // Ignorar el barril mismo
            if (collider.gameObject == gameObject)
                continue;

            // Aplicar daño si el objeto implementa IDamageable
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(explosionDamage);
                Debug.Log($"Explosión del barril golpeó a {collider.gameObject.name}");
            }
        }
    }

    protected override void FlashColor()
    {
        // El barril explota inmediatamente sin parpadear
        // Pero podríamos agregar un cambio de color más dramático
        if (spriteRenderer == null) return;
        spriteRenderer.color = Color.yellow;
    }

    //Visualizar el radio de explosión en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
