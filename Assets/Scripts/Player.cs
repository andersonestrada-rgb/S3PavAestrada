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
tiempo entre ataques.

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


    private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private bool facingRight = true;       

    private void Awake()

    {       
        inputs = new();       

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Sistema de ataque automático
        InvokeRepeating(nameof(Attack), 0.5f, attackCooldown);
    }

    void Update()
    {
        if (moveInput != Vector2.zero)
        {
            MovementMechanism(moveInput);
        }

        const float flipThreshold = 0.01f; // Evitar cambios de dirección por inputs muy pequeños
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
        // Dibuja el rango de ataque (Rojo)
        Gizmos.color = Color.red;
        float currentAttackRange = weaponData != null ? weaponData.Range : 3f; // 3f como predeterminado visual
        Gizmos.DrawWireSphere(transform.position, currentAttackRange);
    }
    // Propiedades
    public int Damage => weaponData != null ? weaponData.Damage : 0;
    public float AttackRange => weaponData != null ? weaponData.Range : 0f;
    public int Ammo => weaponData.Ammo;

    public List<GameObject> Enemys = new();
    public InputSystem_Actions inputs;

    //public static Player Instance { get; private set; }

    public void Attack()
    {
        // 1. Busca enemigo
        GameObject[] enemies = Enemys.ToArray();

        foreach (GameObject enemyObj in enemies)
        {
            if (enemyObj == null) continue;

            // 2. Calcula distancia
            float distance = Vector3.Distance(enemyObj.transform.position, transform.position);

            // 3. Aplica daño si está en rango
            if (distance <= AttackRange)
            {
                var enemyEntity = enemyObj.GetComponent<BaseEntity>();
                if (enemyEntity != null)
                {
                    // La vida no se modifica directamente, se usa TakeDamage con su elemento
                    enemyEntity.TakeDamage(this.Damage, this.Element);
                }
            }
        }
    }

    // Sobreescritura del daño elemental recibido
    public override void TakeDamage(int damageAmount, Elements damageElement)
    {
        float multiplier = 1f;
        switch (damageElement)
        {
            case Elements.Fire: multiplier = 2f; break;
            case Elements.Water: multiplier = 0.5f; break;
            case Elements.Earth: multiplier = 3f; break;
            case Elements.Air: multiplier = 0.5f; break;
            case Elements.None: default: multiplier = 1f; break;
        }

        int finalDamage = Mathf.RoundToInt(damageAmount * multiplier);
        if (damageAmount > 0 && finalDamage < 1) finalDamage = 1;

        base.TakeDamage(finalDamage, damageElement); // Modifica vida mediante el método base
    }

    public void SetWeapon(WeaponData newWeapon)
    {

        this.weaponData = newWeapon;
    }   
}
