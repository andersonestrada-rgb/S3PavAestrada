using UnityEngine;
using UnityEngine.Events;


public class Muro : BaseObject
{
    [Header("Configuración de Resistencia")]
    [SerializeField] private float damageReductionPercent = 50f; // Reduce el 50% del daño
    [SerializeField] private bool showDamageText = true;
    

    private void Start()
    {
        base.Start();
        objectName = objectName == "" ? "Muro" : objectName;
    }

    public override void TakeDamage(int amount)
    {
        // Aplicar reducción de daño
        int reducedDamage = Mathf.RoundToInt(amount * (1 - damageReductionPercent / 100f));
        if (reducedDamage < 1 && amount > 0)
            reducedDamage = 1; // Mínimo 1 de daño

        Debug.Log($"{objectName} absorbió el daño. Daño original: {amount}, Daño final: {reducedDamage}");

        // Mostrar mensaje de resistencia
        if (showDamageText)
        {
            ShowResistanceMessage(amount, reducedDamage);
        }

        // Aplicar el daño reducido
        objectHealth -= reducedDamage;
        FlashColor();

        if (objectHealth <= 0)
        {
            OnDestroyed();
            Destroy(gameObject);
        }
    }

    protected override void OnDestroyed()
    {
        Debug.Log($"{objectName} ha sido completamente destruido");
    }


    private void ShowResistanceMessage(int originalDamage, int finalDamage)
    {
        int absorbedDamage = originalDamage - finalDamage;
        Debug.Log($"{objectName} resistió {absorbedDamage} puntos de daño ({damageReductionPercent}% resistencia)");
    }

    protected override void FlashColor()
    {
        // El muro parpadea en azul para indicar resistencia
        if (spriteRenderer == null) return;
        spriteRenderer.color = new Color(0.5f, 0.7f, 1f); // Azul claro
        Invoke(nameof(ResetColor), 0.15f);
    }
}
