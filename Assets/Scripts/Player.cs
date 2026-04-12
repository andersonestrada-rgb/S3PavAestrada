/*
2️ Sistema de daño y dependencias
Player y Enemy deben tener métodos 
para recibir daño. Player debe tener 
un método SetWeapon(WeaponData weaponData). 
El daño aplicado depende del WeaponData. 
La vida no debe modificarse directamente, 
solo mediante métodos.

3️ Sistema de ataque automático
Player debe implementar un método Attack 
que busque enemigos con FindObjectsWithTag, 
calcule distancia y aplique daño a todos los 
que estén dentro del rango respetando el 
tiempo entre ataquesV5

2️ Sistema de combate con daño elemental:
El Player debe poder atacar automáticamente 
aplicando distintos tipos de daño elemental, 
definidos mediante un enum.
Cada enemigo debe reaccionar de forma distinta 
al daño recibido según su afinidad o resistencia 
elemental, pudiendo recibir más o menos daño según
el tipo de ataque.
La vida de las entidades no debe modificarse 
directamente: debe actualizarse únicamente mediante 
métodos.
*/

using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseEntity
{
    [Header("Configuración de Ataque")]
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Efectos Visuales")]
    [SerializeField] private float damageFlashDuration;
    private Color originalColor;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private bool facingRight = true;

    // Progresión del jugador
    [Header("Progresión")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;
    [SerializeField] private int xpIncreasePerLevel = 50;

    private void Awake()
    {
        inputs = new();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning("No se encontró un SpriteRenderer en el Player ni en sus hijos.");
        }
    }

    void Start()
    {
        InvokeRepeating(nameof(Attack), 0.5f, attackCooldown);
    }

    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            MovementMechanism(moveInput);
        }

        const float flipThreshold = 0.01f;
        if (Mathf.Abs(moveInput.x) > flipThreshold && spriteRenderer != null)
        {
            if (moveInput.x < 0f && facingRight)
            {
                spriteRenderer.flipX = true;
                facingRight = false;
            }
            else if (moveInput.x > 0f && !facingRight)
            {
                spriteRenderer.flipX = false;
                facingRight = true;
            }
        }
    }

    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Move.performed += OnMovement;
        inputs.Player.Move.canceled += OnMovement;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void MovementMechanism(Vector2 input)
    {
        transform.position += (Vector3)input * stats.Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<BaseEntity>();
        if (entity != null && !(entity is Player))
        {
            Enemys.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemys.Remove(collision.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float currentAttackRange = weaponData != null ? weaponData.Range : 2f;
        Gizmos.DrawWireSphere(transform.position, currentAttackRange);
    }

    public int Damage => weaponData != null ? weaponData.Damage : 0;
    public float AttackRange => weaponData != null ? weaponData.Range : 0f;
    public int Ammo => weaponData != null ? weaponData.Ammo : 0;

    public List<GameObject> Enemys = new();
    public InputSystem_Actions inputs;

    public void Attack()
    {
        GameObject[] enemies = Enemys.ToArray();

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null) continue;

            float distance = Vector3.Distance(enemyObj.transform.position, transform.position);

            if (distance <= AttackRange)
            {
                var enemyEntity = enemyObj.GetComponent<BaseEntity>();
                if (enemyEntity != null)
                {
                    enemyEntity.TakeDamage(this.Damage, this.Element);
                }
            }
        }
    }

    protected override float GetElementalMultiplier(Elements damageElement)
    {
        return damageElement switch
        {
            Elements.Fire => 2f,
            Elements.Water => 0.5f,
            Elements.Earth => 3f,
            Elements.Air => 0.5f,
            _ => 1f
        };
    }

    // CORRECCIÓN 3: Sobrescribimos TakeDamage para activar el color justo después de recibir el golpe
    public override void TakeDamage(int damageAmount, Elements damageElement)
    {
        // 1. Llamamos al método base para que haga todo el cálculo de daño
        base.TakeDamage(damageAmount, damageElement);

        // 2. Activamos el efecto visual
        if (spriteRenderer != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(FlashRedCoroutine());
        }
    }

    public void SetWeapon(WeaponData newWeapon)
    {
        this.weaponData = newWeapon;
    }

    private IEnumerator FlashRedCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }

    // ----- Sistema de experiencia / niveles -----
    public void AddExperience(int amount)
    {
        if (amount <= 0) return;
        currentXP += amount;
        Debug.Log($"Experiencia +{amount} => {currentXP}/{xpToNextLevel}");

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        xpToNextLevel += xpIncreasePerLevel;

        // Ejemplo: mejorar stats al subir de nivel
        stats.SetPower(stats.Power + 1);
        stats.SetHealth(stats.Health + 10);
        // Podría añadirse curación completa o mejoras adicionales aquí

        Debug.Log($"¡Nivel {level}! Nuevas stats: Power={stats.Power}, Health={stats.Health}");
    }

    // Propiedades públicas usadas por UI
    public int Level => level;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => xpToNextLevel;
}
