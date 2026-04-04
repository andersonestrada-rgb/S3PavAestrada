using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject SpawnPrefabEnemy;
    [SerializeField] private float spawnRadius; // radio alrededor del player

    void Start()
    {      
        InvokeRepeating(nameof(Spawn), 0, 2f); 
    }

    //Dibujo del radio de spawn (spawnRadius)
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
        Instantiate(SpawnPrefabEnemy, spawnPos, Quaternion.identity);
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
        Instantiate(SpawnPrefabEnemy, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(spawnRadius);
    }   
}
