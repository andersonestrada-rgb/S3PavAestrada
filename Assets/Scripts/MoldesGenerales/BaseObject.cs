using UnityEngine;


public class BaseObject : MonoBehaviour, IDamageable
{
    [Header("Configuración Base")]
    [SerializeField] protected int objectHealth = 10; // Salud actual del objeto
    [SerializeField] protected int objectMaxHealth = 10; // Para mostrar en UI o restaurar salud
    [SerializeField] protected string objectName;

    [Header("Efectos Visuales")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    private Color originalColor;

    protected virtual void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }
 
    public virtual void TakeDamage(int amount)
    {
        objectHealth -= amount;
        Debug.Log($"{objectName} recibió {amount} de daño. Vida restante: {objectHealth}");

        // Efecto visual de parpadeo
        FlashColor();

        // Si la salud llega a 0, destruir
        if (objectHealth <= 0)
        {
            OnDestroyed();
            Destroy(gameObject);
        }
    }
 
    protected virtual void OnDestroyed()
    {
        Debug.Log($"{objectName} ha sido destruido");
    }

   
    protected virtual void FlashColor()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
    }

 
    protected virtual void ResetColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    // Properties
    public int Health => objectHealth;
    public int MaxHealth => objectMaxHealth;
}
