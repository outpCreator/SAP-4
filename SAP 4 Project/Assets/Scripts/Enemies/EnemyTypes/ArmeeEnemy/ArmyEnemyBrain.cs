using System.Drawing.Printing;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAI/ArmyEnemyBrain")]
public class ArmyEnemyBrain : EnemyBehaviour
{
    [Header("Army Settings")]
    public int maxSplits = 5;
    public int minSplits = 1;

    EnemyCombat combat;
    EnemyStats stats;
    ArmyController army;

    public override void Initialize(EnemyCombat enemyCombat, EnemyStats enemyStats)
    {
        combat = enemyCombat;
        stats = enemyStats;
        army = combat.GetComponent<ArmyController>();

        army.Initialize(combat);
    }

    public override void Action(EnemyCombat.EnemyState state)
    {
        if (state == EnemyCombat.EnemyState.Idle)
        {
            army.IdleBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.Follow)
        {
            army.FollowBehaviour();
        }
        else if (state == EnemyCombat.EnemyState.InRange)
        {

        }
        else if (state == EnemyCombat.EnemyState.Died)
        {

        }
    }
}
