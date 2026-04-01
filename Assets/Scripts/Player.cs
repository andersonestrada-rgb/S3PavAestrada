/*
2️ Sistema de daño y dependencias
Player y Enemy deben tener métodos 
para recibir daño. Player debe tener 
un método SetWeapon(WeaponData weaponData). 
El daño aplicado depende del WeaponData. 
La vida no debe modificarse directamente, 
solo mediante métodos.

3️ Sistema de ataque automático
Player debe implementar un método Attack 
que busque enemigos con FindObjectsWithTag, 
calcule distancia y aplique daño a todos los 
que estén dentro del rango respetando el 
tiempo entre ataques.
*/


using UnityEngine;

public class Player : MonoBehaviour
{
    private BaseStats stats;
    private WeaponData weaponData;

    public float range;

    private void Awake()
    {
        stats = new BaseStats(10, 10, 5, 1, 20);
        weaponData = new WeaponData(10, 10, 10);
        //print(stats.Power);
    }

    void Start()
    {
        InvokeRepeating("AutoAttackEnemies", 1f, 1f);
    }

    void Update()
    {

    }
    //   this.stats.TakePower(weaponData.Damage);
  

    public void AutoAttackEnemies()
    {
        print("ATAQUE!");

        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in allEnemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance <= range) 
                enemy.GetComponent<Enemy>().TakeDamage(this);
        }

    }

    public void TakeDamage(Enemy enemy)
    {
        stats.TakeDamage(enemy.Power);        
        if (stats.Health <= 0)
        {
            Destroy(gameObject);
        }
    }


    public int Health => stats.Health;
    public int Power => stats.Power;
    public int Speed => stats.Speed;
    public int Knockback => stats.Knockback;
    public int XP => stats.XP;

    public int Damage => weaponData.Damage;
    public int Range => weaponData.Range;
    public int Ammo => weaponData.Ammo;

    private void OnDestroy()
    {
        Debug.Log("oh no me cancelaron");
    }
}
