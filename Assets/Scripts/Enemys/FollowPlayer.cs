using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class EnemyFollow : MonoBehaviour
{
    private BaseEntity myEntity; // Referencia a sus propias estadísticas
    private Player player; // Referencia al objetivo

    private void Awake()
    {
        myEntity = GetComponent<BaseEntity>();
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }        

    void Update()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;

        transform.position += (Vector3)(direction * myEntity.Stats.Speed * Time.deltaTime);
    }
}