using UnityEngine;

public interface IDamageable
{
    public void TakeDamage();
    // public abstract void TakeDamage();
}
public interface IInteractable
{
    public void Interact();

}
public interface ICollectable
{
    public void Collect();
}