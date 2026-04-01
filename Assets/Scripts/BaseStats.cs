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

public class BaseStats
{
    // Relación de Composición
    private int health;
    private int power;
    private int speed;
    private int knockback;
    private int xp;

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
        if (power <= 0)
            power = 0;
        this.power = power;
    }
    public void SetHealth(int health)
    {
        if (health <= 0)
            health = 0;
        this.health = health;
    }
    public void SetSpeed(int speed)
    {
        if (speed <= 0)
            speed = 0;
        this.speed = speed;
    }
    public void SetKnockback(int knockback)
    {
        if (knockback <= 0)
            knockback = 0;
        this.knockback = knockback;
    }
    public void SetXP(int xp)
    {
        if (xp <= 0)
            xp = 0;
        this.xp = xp;
    }

    public void TakeDamage(int amount) => SetHealth(Health - amount);   // Resta vida
    public void TakePower(int amount) => SetPower(Power + amount);      // Aumenta poder en base a tu tipo de arma



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
