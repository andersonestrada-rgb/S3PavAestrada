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
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseEntity
{    
    private WeaponData weaponData;

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool facingRight = true;


    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        coll.radius = range;
        inputs = new();
        stats = new BaseStats(50, 35, 5, 1, 20);
        weaponData = new WeaponData(10, 3, 10);
        // Si no se asignó en el inspector, intentar obtener el SpriteRenderer del mismo GameObject
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
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
        
        // Usar un umbral para evitar fluctuaciones por entradas pequeñas
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
        transform.position += (Vector3)input * speed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        GameObject player =  GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Gizmos.DrawWireSphere(player.transform.position, weaponData.Range);
        }
        else
        {
            // si no hay player, dibujar el rango de ataque en la posicion de este script
            Gizmos.DrawWireSphere(transform.position, weaponData.Range);
        }
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


    public void Attack()
    {
        print("ATAQUE!");

        // Iterar sobre una copia para evitar InvalidOperationException si la lista se modifica
        foreach (GameObject enemy in Enemys.ToArray())
        {
            if (enemy == null) continue;
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            var entity = enemy.GetComponent<BaseEntity>();
            if (distance <= range && entity != null)
                entity.TakeDamage(this.Damage, this.Element);
        }
    }
       

    public InputSystem_Actions inputs;
    
    public int Damage => weaponData.Damage;
    public int Range => weaponData.Range;
    public int Ammo => weaponData.Ammo;

    public CircleCollider2D coll;
    public float range;
    public List<GameObject> Enemys = new();

    public override void TakeDamage(int damageAmount, Elements damageElement)
    {
        Debug.Log(damageElement);

        float multiplier = 1f;
        switch (damageElement)
        {
            case Elements.None:
                multiplier = 1f;
                break;
            case Elements.Fire:
                multiplier = 2f;
                break;
            case Elements.Water:
                multiplier = 0.5f;
                break;
            case Elements.Earth:
                multiplier = 3f;
                break;
            case Elements.Air:
                multiplier = 0.5f;
                break;
            default:
                multiplier = 1f;
                break;
        }

        int damager = Mathf.RoundToInt(damageAmount * multiplier);
        if (damageAmount > 0 && damager < 1) damager = 1;

        Debug.Log($"{entityName} ha sufrido {damager} punto(s) de daño ({damageElement})");
        base.TakeDamage(damager, damageElement);
    }





    // Permite asignar o cambiar el arma del jugador
    public void SetWeapon(WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }   
}
