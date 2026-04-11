using UnityEngine;

public class RadiusColllector : MonoBehaviour
{
    [SerializeField] private float collectRadius = 5f;
    private Player player;
    private CircleCollider2D radiusCollider;

    private void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
            player = GetComponentInParent<Player>();

        // Crear un collider de radio para detectar collectables
        radiusCollider = gameObject.AddComponent<CircleCollider2D>();
        radiusCollider.radius = collectRadius;
        radiusCollider.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Detectar collectables dentro del radio
        if (collision.CompareTag("Collectable") || collision.GetComponent<Collectable>() != null)
        {
            var collectable = collision.GetComponent<Collectable>();
            if (collectable != null && player != null)
            {
                collectable.CollectBy(player);
                Destroy(collision.gameObject);
            }
        }
    }

    public void SetCollectRadius(float newRadius)
    {
        collectRadius = newRadius;
        if (radiusCollider != null)
            radiusCollider.radius = collectRadius;
    }
}
