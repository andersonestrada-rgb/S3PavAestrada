using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] protected string collectableName;
    [SerializeField] protected string collectableDescription;
    [SerializeField] protected float value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificamos si el que colisionó es el Colector
        var colector = collision.GetComponent<Colector>();

        // 2. Si es el Colector y tiene un Player asignado, aplicamos el efecto
        if (colector != null && colector.player != null)
        {
            Collect(colector.player);

            // 3. Destruimos el objeto para que no se recoja más de una vez
            Destroy(gameObject);
        }
    }

    protected abstract void Collect(Player player);

    public string CollectableName => collectableName;
    public float Value => value;
}