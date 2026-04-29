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

using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerController
{
    none,
    Player1,
    Player2,
}

public class Player : BaseEntity
{
    #region Variables
    #region References
    public List<GameObject> Enemys = new();
    public InputSystem_Actions inputs;
    [SerializeField] private PlayerController playerController;
    #endregion references

    [Header("Configuración de Ataque")]
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private float attackCooldown = 1f;   

    [Header("Efectos Visuales")]
    [SerializeField] private float damageFlashDuration;   
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Configuración de Disparo")]
    public Transform firePoint;

    [Tooltip("Arrastra aquí los 4 prefabs de tus balas en orden")]
    public WeaponBase[] bulletPrefabs;

    [Tooltip("0 = Spin, 1 = Throw, 2 = Falling, 3 = Ghosting")]
    public int currentWeaponIndex = 0; // Cambia este valor cuando el jugador cambie de arma/sprite

    [Header("Referencia de Arma Orbitante")]
    public GameObject orbitWeaponPrefab;
    private GameObject activeOrbitWeapon;

    private Vector2 moveInput;
    private bool facingRight = true;
    private Vector3 frozenWorldPosition;

    // Progresión del jugador
    [Header("Progresión")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;
    [SerializeField] private int xpIncreasePerLevel = 50;
    #endregion

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
    private void OnEnable()
    {
        inputs.Enable();    
        
        switch (playerController)
        {
            case PlayerController.none:
                break;
            case PlayerController.Player1:
                {   

                    inputs.Player1.Move.performed += OnMovement;
                    inputs.Player1.Move.canceled += OnMovementCanceled;
                    inputs.Player1.Attack.started += OnAttack1;
                    inputs.Player1.Attack2.started += OnAttack2;
                    inputs.Player1.ChangeType.started += TypeShoot;
                    inputs.Player1.Move.performed += StopPlayer;
                    inputs.Player1.Move.performed += StopCollector;
                }
                break;
            case PlayerController.Player2:
                {
                    inputs.Player2.Move.performed += OnMovement;
                    inputs.Player2.Move.canceled += OnMovementCanceled;
                    inputs.Player2.Attack.started += OnAttack1;
                    inputs.Player2.Attack2.started += OnAttack2;
                }
                break;

        }
    }
    void Start()
    {
        //InvokeRepeating(nameof(Attack), 0.5f, attackCooldown);
    }

    void Update()
    {     
        MovementMechanism(moveInput);       

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

        // 1. En el instante exacto en que presionas Shift, guardamos su posición actual en el mundo
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            frozenWorldPosition = transform.position;
        }

        // 2. Mientras mantengas Shift presionado, lo forzamos a quedarse en esa coordenada      
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = frozenWorldPosition;
        }
    }
    #region Eventos de Input
    private void StopCollector(InputAction.CallbackContext context)
    {
        Debug.Log("Usame");
    }

    private void StopPlayer(InputAction.CallbackContext context)
    {
        Debug.Log("Usame tambien");
    }

    private void TypeShoot(InputAction.CallbackContext context)
    {
        // Cambiar al siguiente tipo de disparo
        currentWeaponIndex = (currentWeaponIndex + 1) % bulletPrefabs.Length;

        // Obtener el nombre del tipo de disparo
        ProyectileType currentType = (ProyectileType)currentWeaponIndex;
        Debug.Log($"Tipo de disparo cambiado a: {currentType} (Índice: {currentWeaponIndex})");
    }

    private void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.started) // Al pulsar el botón
        {
            if (activeOrbitWeapon == null)
            {
                // Instanciar el arma
                activeOrbitWeapon = Instantiate(orbitWeaponPrefab);

                // Configurar la referencia del jugador en el script de la bala
                OrbitWeapon script = activeOrbitWeapon.GetComponent<OrbitWeapon>();
                script.player = this.transform;
            }
            else
            {
                // Si ya existe, podrías destruirla (toggle) o hacer otra cosa
                Destroy(activeOrbitWeapon);
            }
        }
    }

    private void OnAttack1(InputAction.CallbackContext context)
    {
        // Verificamos que haya prefabs asignados
        if (bulletPrefabs.Length == 0 || firePoint == null) return;

        // Seleccionamos el prefab actual según el índice
        WeaponBase selectedPrefab = bulletPrefabs[currentWeaponIndex];

        // Instanciamos
        WeaponBase newBullet = Instantiate(selectedPrefab, firePoint.position, firePoint.rotation);

        // (Opcional) Asignamos la dirección si es necesario
        newBullet.dir = firePoint.right;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        print("Wazaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        moveInput = context.ReadValue<Vector2>();
    }
    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void MovementMechanism(Vector2 input)
    {
        if (input == Vector2.zero) return;
        transform.position += (Vector3)input * stats.Speed * Time.deltaTime;
    }
    #endregion   

    #region Triggers
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
    #endregion

    #region Private Methods
    private IEnumerator FlashRedCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }
 
    // ----- Sistema de experiencia / niveles -----
    private void LevelUp()
    {
        level++;
        xpToNextLevel += xpIncreasePerLevel;
        stats.SetPower(stats.Power + 1);
        stats.SetHealth(stats.Health + 10);

        Debug.Log($"¡Nivel {level}! Nuevas stats: Power={stats.Power}, Health={stats.Health}");
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
    #endregion

    #region Public Methods
    //public void Attack()
    //{
    //    GameObject[] enemies = Enemys.ToArray();

    //    foreach (GameObject enemyObj in enemies)
    //    {
    //        if (enemyObj == null) continue;

    //        float distance = Vector3.Distance(enemyObj.transform.position, transform.position);

    //        if (distance <= AttackRange)
    //        {
    //            var enemyEntity = enemyObj.GetComponent<BaseEntity>();
    //            if (enemyEntity != null)
    //            {
    //                enemyEntity.TakeDamage(this.Damage, this.Element);
    //            }
    //        }
    //    }
    //} 

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
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float currentAttackRange = weaponData != null ? weaponData.Range : 2f;
        Gizmos.DrawWireSphere(transform.position, currentAttackRange);
    }       

    #region Getters

    // Propiedades públicas usadas por UI
    public int Level => level;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => xpToNextLevel;

    //Getters
    public int Damage => weaponData != null ? weaponData.Damage : 0;
    public float AttackRange => weaponData != null ? weaponData.Range : 0f;
    public int Ammo => weaponData != null ? weaponData.Ammo : 0;
    #endregion
}
