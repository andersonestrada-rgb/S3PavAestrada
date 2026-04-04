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
*/

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private BaseStats stats;
    private WeaponData weaponData;

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool facingRight = true;

    private void Awake()
    {
        inputs = new();
        stats = new BaseStats(10, 10, 5, 1, 20);
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
        Gizmos.color = Color.yellow;
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
   

    public void Attack()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance <= weaponData.Range)//
            {
                var e = enemy.GetComponent<Enemy>();
                if (e != null)
                    e.TakeDamage(this.Damage);
            }
        }
    }

    // Recibir daño: la vida del jugador sólo se modifica a través de este método
    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        if (stats.Health <= 0)
        {
            Destroy(gameObject);
        }
    }


    public InputSystem_Actions inputs;    

    public int Health => stats.Health;
    public int Power => stats.Power;
    public int Speed => stats.Speed;
    public int Knockback => stats.Knockback;
    public int XP => stats.XP;

    public int Damage => weaponData.Damage;
    public int Range => weaponData.Range;
    public int Ammo => weaponData.Ammo;


    // Permite asignar o cambiar el arma del jugador
    public void SetWeapon(WeaponData weaponData)
    {
        this.weaponData = weaponData;
    }

    private void OnDestroy()
    {
        Debug.Log("oh no me cancelaron");
    }
}
