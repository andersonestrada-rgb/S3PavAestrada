using UnityEngine;

[DisallowMultipleComponent]
public class Score : MonoBehaviour
{    
    [SerializeField] private int score = 0;       

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Ya existe una instancia de Score en la escena. Se destruirá la nueva.");
            Destroy(gameObject);
            return;
        }

        Instance = this; // Asignar la instancia singleton
    }      
    
    public void AddScore(int amount)
    {
        if (amount <= 0) return;
        score += amount;
        Debug.Log($"Puntaje aumentado: +{amount} => {score}");
    }

    public static Score Instance { get; private set; }    // Instancia singleton de acceso rápido
    public int CurrentScore => score;
}