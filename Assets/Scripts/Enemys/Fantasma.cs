using UnityEngine;

public class Fantasma : BaseEntity
{

    private void Awake()
    {
        stats = new BaseStats(17, 18, 1, 1, 26);
    }

    void Start()
    {
        
    }

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
                damager *= 5;
                break;
            case Elements.Earth:
                damager *= 2;
                break;
            case Elements.Air:
                damager = 0;
                break;
            default:
                break;
        }
        print($"{entityName} ha sufrido {damager} punto de daño");
    }


}
