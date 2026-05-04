using System.Collections;
using UnityEngine;

public class BurnAbility : BaseAbility
{
    public override void Execute(BaseEntity target)
    {
        // Color rojo durante 3 segundos
        target.ApplyVisualEffect(Color.red, 3f);
        target.StartCoroutine(BurnRoutine(target));
    }

    private IEnumerator BurnRoutine(BaseEntity target)
    {
        // Quema: Daño por 3 segundos
        for (int i = 0; i < 3; i++)
        {
            if (target == null) yield break; // Si el enemigo muere, detenemos
            target.TakeDamage(2, Elements.Fire);
            yield return new WaitForSeconds(1f);
        }
    }
}

public class FreezeAbility : BaseAbility
{
    public override void Execute(BaseEntity target)
    {
        // Color cyan durante 2 segundos
        target.ApplyVisualEffect(Color.cyan, 2f);
        target.StartCoroutine(FreezeRoutine(target));
    }

    private IEnumerator FreezeRoutine(BaseEntity target)
    {
        // Velocidad a 0 (congelado)
        target.SetSpeedMultiplier(0f);
        yield return new WaitForSeconds(2f);

        if (target != null)
            target.SetSpeedMultiplier(1f); // Restaura velocidad normal
    }
}

public class PoisonAbility : BaseAbility
{
    public override void Execute(BaseEntity target)
    {
        // Color verde oscuro por 5 segundos
        target.ApplyVisualEffect(new Color(0.2f, 0.8f, 0.2f), 5f);
        target.StartCoroutine(PoisonRoutine(target));
    }

    private IEnumerator PoisonRoutine(BaseEntity target)
    {
        // Veneno: Daño prolongado de 5 tics
        for (int i = 0; i < 5; i++)
        {
            if (target == null) yield break;
            target.TakeDamage(1, Elements.Earth); // O element None
            yield return new WaitForSeconds(1f);
        }
    }
}

public class ShockAbility : BaseAbility
{
    public override void Execute(BaseEntity target)
    {
        // Color amarillo brillante
        target.ApplyVisualEffect(Color.yellow, 1.5f);
        target.StartCoroutine(ShockRoutine(target));
    }

    private IEnumerator ShockRoutine(BaseEntity target)
    {
        // Electrocución: Stun breve e intermitente
        for (int i = 0; i < 3; i++)
        {
            if (target == null) yield break;
            target.SetSpeedMultiplier(0f); // Se detiene
            target.TakeDamage(1, Elements.Air);
            yield return new WaitForSeconds(0.25f);

            if (target == null) yield break;
            target.SetSpeedMultiplier(1f); // Camina un poco
            yield return new WaitForSeconds(0.25f);
        }
    }
}

public class SlowAbility : BaseAbility
{
    public override void Execute(BaseEntity target)
    {
        // Color azul oscuro o magenta
        target.ApplyVisualEffect(new Color(0.5f, 0.5f, 1f), 3f);
        target.StartCoroutine(SlowRoutine(target));
    }

    private IEnumerator SlowRoutine(BaseEntity target)
    {
        // Ralentización: Corta la velocidad a la mitad
        target.SetSpeedMultiplier(0.5f);
        yield return new WaitForSeconds(3f);

        if (target != null)
            target.SetSpeedMultiplier(1f); // Restaura
    }
}