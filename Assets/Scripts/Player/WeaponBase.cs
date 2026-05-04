using UnityEngine;
using UnityEngine.Events;
public enum ProyectileType
{
    None,
    Spin,
    Throw,
    Falling,
    Ghosting,
}

public class WeaponBase : MonoBehaviour
{
    [Header("Configuración General")]
    public int Duration;
    public ProyectileType Type;
    public Estados estadoAlImpactar; // <-- NUEVA VARIABLE AÑADIDA
    public float speed;
    public float RotationSpeed;
    public int damage;

    [Header("Configuración de Throw (Arco)")]
    Vector3 velocity;
    float gravity = -9.8f;

    [Header("Configuración de Ghosting")]
    [SerializeField] private float ghostingSearchRange = 15f;
    [SerializeField] private float ghostingTurnSpeed = 5f;

    public Vector2 dir;   
    private BaseEntity targetEnemy;
    private bool hasHit = false;
    public UnityEvent effectorAction;

    void Start()
    {
        Destroy(gameObject, Duration);
        dir = randomDirection();
        
        velocity = dir.normalized * speed;
        velocity.y = 5f; // fuerza inicial hacia arriba
    }

    void Update()
    {
        switch (Type)
        {
            case ProyectileType.None:
                break;
            case ProyectileType.Spin:
                {
                    transform.position += (Vector3)dir * speed * Time.deltaTime;
                    transform.eulerAngles += Vector3.forward * RotationSpeed * Time.deltaTime;
                }
                break;
            case ProyectileType.Throw:
                {
                    transform.position += (Vector3)dir * speed * Time.deltaTime;
                    transform.eulerAngles += Vector3.forward * RotationSpeed * Time.deltaTime;
                }
                break;
            case ProyectileType.Falling:
                {
                    velocity.y += gravity * Time.deltaTime;
                    transform.position += velocity * Time.deltaTime;

                    transform.eulerAngles += Vector3.forward * RotationSpeed * Time.deltaTime;
                }
                break;
            case ProyectileType.Ghosting:
                {
                    GhostingMovement();
                }
                break;
            default:
                break;
        }
    }

    private void GhostingMovement()
    {
        // Buscar enemigo si no tenemos target
        if (targetEnemy == null || !targetEnemy.gameObject.activeInHierarchy)
        {
            targetEnemy = FindNearestEnemy();
        }

        // Si tenemos un enemigo, movernos hacia él
        if (targetEnemy != null)
        {
            Vector2 directionToEnemy = ((Vector2)targetEnemy.transform.position - (Vector2)transform.position).normalized;

            // Rotar gradualmente hacia el enemigo (giro suave)
            dir = Vector2.Lerp(dir, directionToEnemy, ghostingTurnSpeed * Time.deltaTime);
            dir = dir.normalized;

            // Mover hacia la dirección
            transform.position += (Vector3)dir * speed * Time.deltaTime;

            // Rotar el sprite para que apunte hacia el movimiento
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle - 90f);
        }
        else
        {
            // Si no hay enemigos, moverse en dirección aleatoria
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }

    private BaseEntity FindNearestEnemy()
    {
        BaseEntity[] allEnemies = FindObjectsOfType<BaseEntity>();
        BaseEntity nearest = null;
        float minDistance = ghostingSearchRange;

        foreach (BaseEntity enemy in allEnemies)
        {
            // Ignorar si es el Player
            if (enemy is Player)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }

    public Vector2 randomDirection()
    {
        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return randomDir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si ya chocó y NO es Spin, ignoramos.
        // Si ES Spin, permitimos que siga procesando colisiones para rebotar varias veces.
        if (hasHit && Type != ProyectileType.Spin) return;

        var enemy = collision.GetComponent<BaseEntity>();
        if (enemy != null && !(enemy is Player))
        {
            // 1. Aplicar daño al enemigo
            enemy.TakeDamage(damage, Elements.None);

            // 2. APLICAR EL ESTADO (NUEVA LÓGICA)
            if (estadoAlImpactar != Estados.None)
            {
                enemy.ApplyState(estadoAlImpactar);
                // Nota: Asumo que crearás un método ApplyState en BaseEntity
            }
            // 2. Verificamos qué tipo de bala es para decidir si rebota o se destruye
            if (Type == ProyectileType.Spin)
            {
                // REBOTE: Dirección desde el centro del enemigo hacia el proyectil
                Vector2 normal = (transform.position - enemy.transform.position).normalized;

                // Aplicamos la reflexión a la dirección actual
                dir = Vector2.Reflect(dir, normal).normalized;

                // Nota: NO llamamos a Destroy() ni a hasHit = true
            }
            else
            {
                // Si es cualquier otro tipo de proyectil (Throw, Falling, etc.)
                hasHit = true;
                Destroy(gameObject);
            }
        }
    }
   
}