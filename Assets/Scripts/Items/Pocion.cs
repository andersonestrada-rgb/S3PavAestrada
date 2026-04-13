using UnityEngine;

public class Pocion : Collectable
{
    // Cuando el Player recoge la poción, restaura su vida
    protected override void Collect(Player player)
    {
        int healthRestored = (int)value;
        Debug.Log($"Player recogió {collectableName} y restauró {healthRestored} de vida");

        player.HealHealth(healthRestored);
    }
}
