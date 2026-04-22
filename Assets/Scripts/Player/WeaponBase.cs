using UnityEngine;
public enum ProyectileType
{
    None,
    Spin,
    Throw,
    Falling,
    // Agrega más tipos de armas según sea necesario
}


public class WeaponBase : MonoBehaviour
{
    public ProyectileType Type;
    public int Duration;
    public float Speed;
    public Vector2 dass;

    void Start()
    {
        Destroy(gameObject, Duration);
    }

    void Update()
    {
        switch (Type)
        {
            case ProyectileType.None:
                break;
            case ProyectileType.Spin:
                transform.position += (Vector3)randownDirection() * Speed * Time.deltaTime;
                transform.eulerAngles += Vector3.forward * Speed * Time.deltaTime; // Gira el proyectil
                break;
            case ProyectileType.Throw:
                transform.position += (Vector3)randownDirection() * Speed * Time.deltaTime;
                break;
            case ProyectileType.Falling:
                break;
            default:
                break;
        }
    }


    public Vector2 randownDirection()
    {
        Vector2 randownDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return randownDir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

      
    }



}
