using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] private GameObject Recolector; // Objeto que detecta la colisión para recoger el collectable (puede ser el player o un objeto hijo)
    [SerializeField] protected string collectableName;
    [SerializeField] protected string collectableDescription;
    [SerializeField] protected float value; //Sube la vida, el mana o la experiencia dependiendo del tipo de collectable       

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null || Recolector != null)
        {
           var recolector = collision.gameObject; 
            Destroy(gameObject);
        }
    }


    if (collision.CompareTag("Recolectador"))
        {           
            var player = collision.GetComponent<Player>();
            if (player == null)
                player = collision.GetComponentInParent<Player>();

            if (player != null)
            {
                CollectBy(player);
    Destroy(gameObject);
}
        }

    /// <summary>
    /// Método público que permite ser recolectado por RadiusCollector o contacto directo
    /// </summary>
    public void CollectBy(Player player)
    {
        if (player != null)
        {
            Collect(player);
        }
    }

    // Cada tipo de collectable define su propio efecto al ser recogido
    protected abstract void Collect(Player player);


    public string CollectableName => collectableName;
    public float Value => value;

}
