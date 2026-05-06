using System;
using UnityEngine;

public abstract class Collectable : MonoBehaviour , ICollectable
{
    [SerializeField] protected string collectableName;
    [SerializeField] protected string collectableDescription;
    [SerializeField] protected float value;

    [Header("Desaparición después de recoger")]
    [SerializeField] private float fadeDuration = 1.5f;   // tiempo total del fade en segundos
    [SerializeField] private float fadeInterval = 0.05f;  // intervalo entre pasos (usado por InvokeRepeating)

    private bool isFading = false;
    private SpriteRenderer[] spriteRenderers;
    private float[] initialAlphas;
    private float[] alphaStep;

    private void Awake()
    {
        // Cachea los SpriteRenderer para usar en el fade
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificamos si el que colisionó es el Colector
        var colector = collision.GetComponent<Colector>();

        // 2. Si es el Colector y tiene un Player asignado, aplicamos el efecto
        if (colector != null && colector.player != null && !isFading)
        {
            Collect(colector.player);

            // Inicia el fade y destrucción usando InvokeRepeating (sin IEnumerator)
            StartFade();
        }
    }

    protected abstract void Collect(Player player);

    private void StartFade()
    {
        if (isFading)
            return;

        isFading = true;

        // Deshabilita el collider para evitar recollects
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Si no hay SpriteRenderer, destruye después de fadeDuration
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            Invoke(nameof(DestroyNow), fadeDuration);
            return;
        }

        // Prepara arrays para pasos exactos
        int steps = Mathf.Max(1, Mathf.RoundToInt(fadeDuration / Mathf.Max(0.0001f, fadeInterval)));
        initialAlphas = new float[spriteRenderers.Length];
        alphaStep = new float[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            initialAlphas[i] = spriteRenderers[i].color.a;
            alphaStep[i] = initialAlphas[i] / steps;
        }

        // Inicia la repetición que hará el fade paso a paso
        InvokeRepeating(nameof(FadeStep), 0f, fadeInterval);
    }

    private void FadeStep()
    {
        bool anyVisible = false;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            var sr = spriteRenderers[i];
            Color c = sr.color;
            float newAlpha = Mathf.Max(0f, c.a - alphaStep[i]);
            c.a = newAlpha;
            sr.color = c;

            if (newAlpha > 0f) anyVisible = true;
        }

        if (!anyVisible)
        {
            // Terminó el fade: cancela y destruye
            CancelInvoke(nameof(FadeStep));
            DestroyNow();
        }
    }

    private void DestroyNow()
    {
        Destroy(gameObject);
    }

    public string CollectableName => collectableName;
    public float Value => value;


    public void Collect()
    {
        //-> sumar puntos
        //->poner un sonido
        //->ser destruida
        print("has coleccionado una moneda de valor :" + value);
        Destroy(gameObject);
    }

}