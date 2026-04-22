using UnityEngine;

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
    public int Duration;
    public ProyectileType Type;
    public float speed;
    public float RotationSpeed;

    public Vector2 dir;


    void Start()
    {
        Destroy(gameObject, Duration);
        dir = randomDirection();
    }
    void Update()
    {
        switch (Type)
        {
            case ProyectileType.None:
                break;
            case ProyectileType.Spin:
                {
                    transform.position += transform.up * speed * Time.deltaTime;
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
                    transform.position += (Vector3)dir * speed * Time.deltaTime;
                    transform.eulerAngles += Vector3.forward * RotationSpeed * Time.deltaTime;
                }
                break;
            case ProyectileType.Ghosting:
                {
                    
                }
                break;
            default:
                break;
        }
    }
    public Vector2 randomDirection()
    {
        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return randomDir.normalized;
    }

                
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {

        }
    }
}