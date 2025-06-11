using UnityEngine;

public abstract class EnemyBehaviourDefinition : ScriptableObject
{
    public abstract void Action(SmallEnemyBehaviour behaviour, EnemyCombat.EnemyState state);
}
