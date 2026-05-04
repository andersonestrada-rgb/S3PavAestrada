using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum Elements
{
    None, Fire, Water, Earth, Air
}

public class BaseEntity : MonoBehaviour
{
    [Header("Configuración de la Entidad")]
    [SerializeField] protected int entityID;
    [SerializeField] protected string entityName;
    [SerializeField] protected string enetityDescription;
    [SerializeField] protected Elements element;
    [SerializeField] protected BaseStats stats;

    [Header("Efectos Visuales de Estados")]
    [SerializeField] protected SpriteRenderer entitySpriteRenderer;
    public UnityEvent onStateApplied; // Arrastra tus partículas aquí desde el Inspector

    private Color originalColor;
    private Coroutine visualCoroutine;

    // Multiplicador de velocidad (para Freeze y Slow)
    public float currentSpeedMultiplier { get; private set; } = 1f;

    protected virtual void Start()
    {
        if (entitySpriteRenderer == null) entitySpriteRenderer = GetComponent<SpriteRenderer>();
        if (entitySpriteRenderer != null) originalColor = entitySpriteRenderer.color;
    }

    private void OnDestroy() { Debug.Log($"{entityName} ha sido destruido"); }

    protected virtual float GetElementalMultiplier(Elements damageElement) { return 1f; }

    public virtual void TakeDamage(int damageAmount, Elements damageElement)
    {
        float multiplier = GetElementalMultiplier(damageElement);
        int finalDamage = Mathf.RoundToInt(damageAmount * multiplier);
        if (damageAmount > 0 && finalDamage < 1) finalDamage = 1;

        stats.TakeDamage(finalDamage);

        if (stats.Health <= 0)
        {
            if (Score.Instance != null) Score.Instance.AddScore(stats.XP);
            if (Spawner.Instance != null && !(this is Player)) Spawner.Instance.DropXPOrb(transform.position);

            if (!(this is Player))
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null && playerObj.TryGetComponent(out Player player))
                {
                    player.AddExperience(stats.XP);
                }
            }
            Destroy(gameObject);
        }
    }

    public virtual void HealHealth(int healAmount)
    {
        if (healAmount <= 0) return;
        stats.HealHealth(healAmount);
    }

    // ==========================================
    // NUEVA LÓGICA DE ESTADOS (HITOS 7 y 8)
    // ==========================================

    public void ApplyState(Estados estado)
    {
        // Factory pattern simple: Crea la habilidad correcta según el enum
        BaseAbility ability = estado switch
        {
            Estados.Burn => new BurnAbility(),
            Estados.Freeze => new FreezeAbility(),
            Estados.Poison => new PoisonAbility(),
            Estados.Shock => new ShockAbility(),
            Estados.Slow => new SlowAbility(),
            _ => null
        };

        // Ejecuta la habilidad pasándole este enemigo como objetivo
        ability?.Execute(this);
    }

    // Método que las habilidades llamarán para cambiar el color y disparar las partículas
    public void ApplyVisualEffect(Color tintColor, float duration)
    {
        onStateApplied?.Invoke(); // Dispara las partículas

        if (visualCoroutine != null) StopCoroutine(visualCoroutine);
        visualCoroutine = StartCoroutine(VisualRoutine(tintColor, duration));
    }

    private IEnumerator VisualRoutine(Color tint, float duration)
    {
        if (entitySpriteRenderer != null) entitySpriteRenderer.color = tint;
        yield return new WaitForSeconds(duration);
        if (entitySpriteRenderer != null) entitySpriteRenderer.color = originalColor;
    }

    // Método que las habilidades usarán para alterar la velocidad (Freeze/Slow)
    public void SetSpeedMultiplier(float multiplier)
    {
        currentSpeedMultiplier = multiplier;
    }

    // ==========================================
    // PROPERTIES
    // ==========================================
    public BaseStats Stats => stats;
    public Elements Element => element;
    public int Health => stats.Health;
    public int Power => stats.Power;

    // IMPORTANTE: Los enemigos ahora deben usar esta propiedad para moverse
    public float ActualSpeed => stats.Speed * currentSpeedMultiplier;
    
    public int Knockback => stats.Knockback;
    public int XP => stats.XP;
}