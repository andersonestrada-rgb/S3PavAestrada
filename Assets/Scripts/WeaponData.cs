/*
1️ Modelado de datos y entidades
Crear BaseStats y WeaponData como 
clases puras con constructores 
parametrizados. Crear Player y Enemy 
como MonoBehaviour, ambos deben tener 
una instancia de BaseStats e 
inicializarla en Awake.
*/


using UnityEngine;

public class WeaponData
{

    // Relación de Composición
    private int damage;
    private int range;
    private int ammo;    

    public WeaponData(int damage, int range, int ammo)
    {
        SetDamage(damage);
        SetRange(range);
        SetAmmo(ammo);
    }

    public void SetDamage(int damage)
    {
        if (damage <= 0)
            damage = 0;
        this.damage = damage;
    }

    public void SetRange(int range)
    {
        if (range <= 0)
            range = 0;
        this.range = range;
    }

    public void SetAmmo(int ammo)
    {
        if (ammo <= 0)
            ammo = 0;
        this.ammo = ammo;
    }

    public int Damage => damage;
    public int Range => range;
    public int Ammo => ammo;


    ~WeaponData()
    {
        Debug.Log("Eliminado por el garbage collector");
    }
}
