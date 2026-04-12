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

[System.Serializable]
public class BaseStats
{
    // Relación de Composición

    [SerializeField] private int health;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int knockback;
    [SerializeField] private int xp;

    public BaseStats(int health, int power, int speed, int knockback, int xp)
    {
        SetHealth(health);
        SetPower(power);
        SetSpeed(speed);
        SetKnockback(knockback);
        SetXP(xp);
    }

    public void SetPower(int power)
    {
        if (power <= 0) power = 0;
        this.power = power;
    }

    public void SetHealth(int health)
    {
        if (health <= 0) health = 0;
        this.health = health;
    }

    public void SetSpeed(int speed)
    {
        if (speed <= 0) speed = 0;
        this.speed = speed;
    }

    public void SetKnockback(int knockback)
    {
        if (knockback <= 0) knockback = 0;
        this.knockback = knockback;
    }

    public void SetXP(int xp)
    {
        if (xp <= 0) xp = 0;
        this.xp = xp;
    }

    public void TakeDamage(int amount) => SetHealth(Health - amount);
    public void HealHealth(int amount) => SetHealth(Health + amount);
    public void TakePower(int amount) => SetPower(Power + amount);

    public int Health => health;
    public int Power => power;
    public int Speed => speed;
    public int Knockback => knockback;
    public int XP => xp;

    ~BaseStats()
    {
        Debug.Log("Eliminado por el garbage collector");
    }
}
