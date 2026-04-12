using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Patrón singleton para acceder desde cualquier lugar (como Score.Instance)
    public static Spawner Instance { get; private set; }

    [Header("Configuración de Spawn")]
    [SerializeField] private GameObject[] SpawnPrefabEnemy;
    [SerializeField] private GameObject[] xpOrbPrefab; // Prefab de EsferaXP para instanciar al morir enemigos
    [SerializeField] private float spawnRadius; 
    [SerializeField] private float SpawnIntervalue;

    private void Awake()
    {
        // Configuración del Singleton 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {      
        InvokeRepeating(nameof(Spawn), 0, SpawnIntervalue); 
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Gizmos.DrawWireSphere(player.transform.position, spawnRadius);
        }
        else
        {
            // si no hay player, dibujar en la posición del spawner
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }

    //Método para spawnear enemigos alrededor del jugador 
    public void Spawn()
    {
        if (SpawnPrefabEnemy == null)
        {
            Debug.LogWarning("Spawner: SpawnPrefabEnemy no asignado.");
             return;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Spawner: Player no encontrado.");
             return;
        }

        Vector2 randomPoint = Random.insideUnitCircle;
        if (randomPoint == Vector2.zero) // Evitar el caso raro de un punto exactamente en el centro
            randomPoint = Vector2.right; // Cualquier dirección es válida. Rigth = (1,0)
        Vector2 edgePoint = randomPoint.normalized * spawnRadius; 
        Vector3 spawnPos = player.transform.position + new Vector3(edgePoint.x, edgePoint.y, 0f);
        int randomIndex = Random.Range(0, SpawnPrefabEnemy.Length);
        Instantiate(SpawnPrefabEnemy[randomIndex], spawnPos, Quaternion.identity);        
    }

    // Instancia una esfera de XP en la posición indicada (llamada cuando un enemigo muere)
    public void DropXPOrb(Vector3 position)
    {
        if (xpOrbPrefab == null)
        {
            Debug.LogWarning("Spawner: xpOrbPrefab no asignado.");
            return;
        }

        int randomIndex = Random.Range(0, xpOrbPrefab.Length);
        var xpOrb = Instantiate(xpOrbPrefab[randomIndex], position, Quaternion.identity);
        Debug.Log($"Esfera de XP creada en {position}");
    }

    IEnumerator Spawn2() //Este médoto es opcional
    {
        if (SpawnPrefabEnemy == null)
        {
            Debug.LogWarning("Spawner: SpawnPrefabEnemy no asignado.");
            yield break;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Spawner: Player no encontrado.");
            yield break;
        }

        Vector2 randomPoint = Random.insideUnitCircle;
        if (randomPoint == Vector2.zero)
            randomPoint = Vector2.right;
        Vector2 edgePoint = randomPoint.normalized * spawnRadius;
        Vector3 spawnPos = player.transform.position + new Vector3(edgePoint.x, edgePoint.y, 0f);
        int randomIndex = Random.Range(0, SpawnPrefabEnemy.Length);
        Instantiate(SpawnPrefabEnemy[randomIndex], spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(spawnRadius);
    }   
}
