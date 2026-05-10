using UnityEngine;

public interface IDamageable
{   
    void TakeDamage(int amount);
}

public interface IInteractable
{
    public void Interact();
}

public interface ICollectable
{ 
    void Collect();
}