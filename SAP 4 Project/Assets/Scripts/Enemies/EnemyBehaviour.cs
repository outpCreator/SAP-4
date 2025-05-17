using UnityEngine;

public abstract class EnemyBehaviour : ScriptableObject
{
    public abstract void Initialize(EnemyCombat combat, EnemyStats stats);
    public abstract void Idle();

    public abstract void Follow();

    public abstract void InRange();

    public abstract void Attack();

    public abstract void Die();
}
