using UnityEngine;

public class OrbitWeapon : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public Transform player;       // Referencia al transform del jugador
    public float orbitSpeed = 100f; // Velocidad de giro
    public float radius = 2f;      // Distancia al jugador

    [Header("Configuración de Combate")]
    public int damage = 10;
    public Elements element = Elements.None; // Usando tu enum de elementos

    private float currentAngle;

    void Update()
    {
        if (player == null) return;

        // 1. Calcular la posición en el círculo con Seno y Coseno
        currentAngle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;

        // 2. Actualizar posición relativa al jugador
        transform.position = new Vector3(player.position.x + x, player.position.y + y, 0);

        // 3. Hacer que el arma siempre mire hacia afuera o rote sobre sí misma
        transform.Rotate(Vector3.forward * 500f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar si chocamos con un enemigo
        var enemy = collision.GetComponent<BaseEntity>();

        if (enemy != null && !(enemy is Player))
        {
            enemy.TakeDamage(damage, element);
            // Aquí puedes añadir efectos visuales o sonidos
            Debug.Log("Arma orbitante golpeó a: " + collision.name);
        }
    }
}