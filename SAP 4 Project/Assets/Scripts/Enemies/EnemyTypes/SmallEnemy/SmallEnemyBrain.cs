using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI/SmallEnemyBrain")]
public class SmallEnemyBrain : EnemyBehaviour
{
    EnemyCombat combat;
    EnemyStats stats;

    SmallEnemyBehaviour behaviour;

    public override void Initialize(EnemyCombat enemyCombat, EnemyStats enemyStats)
    {
        combat = enemyCombat;
        stats = enemyStats;

        behaviour = combat.GetComponent<SmallEnemyBehaviour>();
        behaviour.Initialize(combat);
    }

    public override void Action(EnemyCombat.EnemyState state)
    {
        if (state == EnemyCombat.EnemyState.Idle)
        {
            behaviour.IdleBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.Follow)
        {
            behaviour.FollowBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.InRange)
        {
            behaviour.InRangeBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.Died)
        {
            behaviour.DiedBehaviour();
        }
    }
}
