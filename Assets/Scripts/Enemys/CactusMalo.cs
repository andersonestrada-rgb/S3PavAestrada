using UnityEngine;

public class CactusMalo : BaseEntity
{
    private void Awake()
    {
        stats = new BaseStats(15, 12, 3, 1, 10);
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
                damager *= 3;
                break;
            case Elements.Water:
                damager = 0;
                break;
            case Elements.Earth:
                damager *= 2;
                break;
            case Elements.Air:
                damager /= 4;
                break;
            default:
                break;
        }

    }


}
