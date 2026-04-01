using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject SpawnPrefabEnemy;
    [SerializeField] private float spawnRadius = 5f; // radio alrededor del player

    void Start()
    {
        //StartCoroutine(Spawn());

        InvokeRepeating(nameof(Spawn), 0, 5f);
    }

    void Update()
    {


    }

    //Llama a este método para spawnear enemigos alrededor del jugador
    public void Spawn()
    {
        if (SpawnPrefabEnemy == null)
        {
            Debug.LogWarning("Spawner: SpawnPrefabEnemy no asignado.");
            return;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = player.transform.position + new Vector3(randomPoint.x, randomPoint.y, 0f);
        Instantiate(SpawnPrefabEnemy, spawnPos, Quaternion.identity);
    }

    IEnumerator Spawn2()
    {
        if (SpawnPrefabEnemy == null)
        {
            Debug.LogWarning("Spawner: SpawnPrefabEnemy no asignado.");
            yield break;
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = player.transform.position + new Vector3(randomPoint.x, randomPoint.y, 0f);
        Instantiate(SpawnPrefabEnemy, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(spawnRadius);   
    }



}
