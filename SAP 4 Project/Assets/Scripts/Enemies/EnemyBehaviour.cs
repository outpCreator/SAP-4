using UnityEngine;

public abstract class EnemyBehaviour : ScriptableObject
{
    public abstract void Initialize(EnemyCombat combat, EnemyStats stats);
    public abstract void Action(EnemyCombat.EnemyState state);
}
