using UnityEngine;

public class EsferaXP : Collectable
{
    // Cuando el Player recoge la esfera, suma su valor al puntaje global
    protected override void Collect(Player player)
    {
        Debug.Log($"Player recogió {collectableName} +{value} XP");

        if (Score.Instance != null)
        {
            Score.Instance.AddScore((int)value);
        }
    }
}
