using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Referencias del Sistema")]
    [SerializeField] private Player player; // Arrastra a tu Player aquí

    [Header("Referencias de UI")]
    [SerializeField] private TextMeshProUGUI healthText; // Arrastra tu Text_Health aquí
    [SerializeField] private TextMeshProUGUI xpText;     // Arrastra tu Text_XP aquí

    void Update()
    {
        // Validamos que el player y los textos existan para evitar errores
            if (player == null || healthText == null || xpText == null)
            return;

        // Leemos las propiedades públicas de tu BaseEntity
        healthText.text = $"Vida: {player.Health}";

        // Muestra la XP del player y el puntaje actual (Score). Maneja Score.Instance nulo.
        int currentScore = Score.Instance != null ? Score.Instance.CurrentScore : 0;
        xpText.text = $"Experiencia: {player.XP}  |  Puntaje: {currentScore}";
    }
}