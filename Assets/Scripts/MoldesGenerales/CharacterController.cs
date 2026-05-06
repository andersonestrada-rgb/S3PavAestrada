using UnityEngine;



public class CharacterController : BaseEntity, IDamageable
{






















    public void TakeDamage()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ICollectable>() != null)
        {
            collision.gameObject.GetComponent<ICollectable>().Collect();
        }

    }
}