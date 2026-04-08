using UnityEngine;

public class MagoMalo : BaseEntity
{
    private void Awake()
    {
        stats = new BaseStats(9, 25, 3, 1, 30);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = new BaseStats(9, 10, 3, 1, 20);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void TakeDamage(BaseEntity damage)
    {
        base.TakeDamage(damage);

        Debug.Log(damage.Element);

        int damager = damage.Stats.Power;

        switch (damage.Element)
        {
            case Elements.None:
                //damage = damage;
                break;
            case Elements.Fire:
                damager = 0;
                break;
            case Elements.Water:
                damager = 0;
                break;
            case Elements.Earth:
                damager = 0;
                break;
            case Elements.Air:
                damager /= 2;
                break;
            default:
                break;
        }
        print($"{entityName} ha sufrido {damager} punto de daño");
    }



}
